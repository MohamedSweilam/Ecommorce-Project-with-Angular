using AutoMapper;
using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.interfaces;
using Ecommorce.Core.Services;
using Ecommorce_.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
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
        private readonly IConnectionMultiplexer _redis;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signIn;
        private readonly IGenerateToken token;

        public UnitOfWork(ApplicationDbContext context, IMapper mapper, IImageMangmentService imageMangmentService,
            IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signIn, IGenerateToken token)
        {
            this.userManager = userManager;
            _mapper = mapper;
            _imageMangmentService = imageMangmentService;
            this.token = token;
            _redis = redis;
            _context = context;
            this.emailService = emailService;
            this.signIn = signIn;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context, _mapper, _imageMangmentService);
            PhotoRepository = new PhotoRepository(_context);
            CustomerBasket = new CustomerBasketRepository(_redis);
            Auth = new AuthRepository(userManager, emailService, signIn, token , context);
        }

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public ICustomerBasketRepository CustomerBasket { get; }

        public IAuth Auth { get; }
    }
}
