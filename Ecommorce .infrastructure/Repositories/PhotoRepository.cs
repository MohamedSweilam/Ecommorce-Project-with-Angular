using Ecommorce.Core.Entities.Product;
using Ecommorce.Core.interfaces;
using Ecommorce_.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>,IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
