using AutoMapper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.Product;
using Ecommorce.Core.interfaces;
using Ecommorce.Core.Services;
using Ecommorce.Core.Sharing;
using Ecommorce_.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageMangmentService _imageMangmentService;

        public ProductRepository(ApplicationDbContext context, IMapper mapper, IImageMangmentService imageMangmentService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _imageMangmentService = imageMangmentService;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products
                .Include(m=>m.Category)
                .Include(m=>m.Photos)
                .AsNoTracking();
            if(productParams.CategoryId.HasValue){
                query = query.Where(m => m.CategoryId == productParams.CategoryId);

            }
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAsc" => query.OrderBy(m => m.NewPrice),
                    "PriceDsc" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name) // this now works even if sort is null or ""
                };


            }
            
            query = query.Skip((productParams.pageSize) * (productParams.PageNumber - 1)).Take(productParams.pageSize);
            var result = _mapper.Map<List<ProductDTO>>(query);
            return result;
        }
        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;

            // Map and create the product
            var product = _mapper.Map<Product>(productDTO);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync(); // Now product.Id is available

            // Save images physically and return relative paths
            var imagePaths = await _imageMangmentService.AddImageAsync(productDTO.Photo, productDTO.Name);

            // Map image paths to Photo entities
            var photos = imagePaths.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();

            // Save photo records
            await _context.Photos.AddRangeAsync(photos);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
                return false;

            var FindProduct = await _context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == updateProductDTO.Id);

            if (FindProduct is null)
                return false;

            // ✅ Mapping into existing entity (no await!)
            _mapper.Map(updateProductDTO, FindProduct);

            // 🧹 Delete old photos
            var FindPhoto = await _context.Photos
                .Where(m => m.ProductId == updateProductDTO.Id)
                .ToListAsync();

            foreach (var item in FindPhoto)
            {
                 _imageMangmentService.DeleteImageAsync(item.ImageName);
            }

            _context.Photos.RemoveRange(FindPhoto);

            // 📸 Add new photos
            var ImagePath = await _imageMangmentService.AddImageAsync(updateProductDTO.Photo, updateProductDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = updateProductDTO.Id, // ✅ Corrected from `Id =` to `ProductId =`
            }).ToList();

            await _context.Photos.AddRangeAsync(photo);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(Product product)
        {
            var photo = await _context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();
            foreach(var item in photo)
            {
                _imageMangmentService.DeleteImageAsync(item.ImageName);

            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }




    }
}
