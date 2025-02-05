using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Models;
using System.Security.Claims;

namespace SoftUniBazar.Controllers
{
    public class AdController : Controller
    {
        private readonly BazarDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdController(BazarDbContext context, UserManager<IdentityUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<CategoryViewModel> categories = await _context
                .Categories
                .Select(c =>new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            AddAdViewModel model = new AddAdViewModel();
            model.Categories = categories;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAdViewModel model)
        {
            if (ModelState.IsValid)
            {
                Ad ad = new Ad
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    ImageUrl = model.ImageUrl,
                    CreatedOn = DateTime.Now,
                    CategoryId = model.CategoryId
                };

                await _context.AddAsync(ad);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            List<AllAdsViewModel> models = await _context
                .Ads
                .Include(ad => ad.Category)
                .Include(ad => ad.Owner)
                .Where(ad => ad.adsBuyers
                    .Select(b => b.BuyerId)
                    .Any(b => !b.Contains(GetUserId())))
                .Select(ad => new AllAdsViewModel
                {
                    Id = ad.Id,
                    Name = ad.Name,
                    Description = ad.Description,
                    ImageUrl = ad.ImageUrl,
                    Price = ad.Price,
                    Category = ad.Category.Name,
                    Owner = ad.Owner.UserName,
                    CreatedOn = ad.CreatedOn
                })
                .ToListAsync();

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            List<MyCartViewModel> models = await _context
                .Ads
                .Include(ad => ad.Category)
                .Include(ad => ad.Owner)
                .Where(ad => ad.adsBuyers
                    .Select(b => b.BuyerId)
                    .Any(b => b.Contains(GetUserId())))
                .Select(ad => new MyCartViewModel
                {
                    Id = ad.Id,
                    Name = ad.Name,
                    Description = ad.Description,
                    ImageUrl = ad.ImageUrl,
                    Price = ad.Price,
                    Category = ad.Category.Name,
                    Owner = ad.Owner.UserName,
                    CreatedOn = ad.CreatedOn
                })
                .ToListAsync();

            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            Ad? ad = await _context
                .Ads
                .Include(ad => ad.adsBuyers)
                .FirstOrDefaultAsync(ad => ad.Id == id && ad.OwnerId != GetUserId());

            if(ad == null)
            {
                throw new NullReferenceException(nameof(ad));
            }

            AdBuyer model = new AdBuyer
            {
                BuyerId = GetUserId(),
                AdId = id
            };

            ad.adsBuyers.Add(model);

            await _context.SaveChangesAsync();

            return RedirectToAction("Cart", "Ad");
        }
        //-----------------------------------------------------
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        
        public string? GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
