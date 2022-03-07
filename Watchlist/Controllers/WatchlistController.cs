using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Watchlist.Data;
using Watchlist.Models;

namespace Watchlist.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WatchlistController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            return user?.Id;
        }

        public async Task<IActionResult> Index()
        {
            var id = await GetCurrentUserId();
            var userMovies = _context.UserMovies.Where(x => x.UserId == id);
            var model = userMovies.Select(x => new MovieViewModel
            {
                MovieId = x.MovieId,
                Title = x.Movie.Title,
                Year = x.Movie.Year,
                Watched = x.Watched,
                InWatchlist = true,
                Rating = x.Rating
            }).ToList();

            return View(model);
        }
    }
}
