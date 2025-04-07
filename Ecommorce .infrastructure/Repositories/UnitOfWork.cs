using Ecommorce.Core.interfaces;
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

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context);
            PhotoRepository = new PhotoRepository(_context);

        }

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }
    }
}
