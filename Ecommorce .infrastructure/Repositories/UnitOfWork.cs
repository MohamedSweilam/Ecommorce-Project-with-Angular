using AutoMapper;
using Ecommorce.Core.interfaces;
using Ecommorce.Core.Services;
using Ecommorce_.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories
{
    public class UnitOfWork : IUnitofwork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageMangmentService _imageMangmentService;

        public UnitOfWork(ApplicationDbContext context, IMapper mapper, IImageMangmentService imageMangmentService)
        {
            _mapper = mapper;
            _imageMangmentService = imageMangmentService;
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context,_mapper,_imageMangmentService);
            PhotoRepository = new PhotoRepository(_context);
        }

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }
    }
}
