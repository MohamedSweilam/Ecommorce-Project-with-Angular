using AutoMapper;
using Ecommorce.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommorce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitofwork _unitwork;
        protected readonly IMapper _mapper;

        public BaseController(IUnitofwork unitwork,IMapper mapper)
        {
            _unitwork = unitwork;
            _mapper = mapper;
        }
    }
}
