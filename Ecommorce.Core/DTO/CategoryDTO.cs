using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.Core.DTO
{
    public record CategoryDTO
    (string Name,string Description);
    public record UpdateDTO(string Name,string Description,int Id);
}
