using DT191GProjekt.Data;
using DT191GProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics;

namespace DT191GProjekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/gallery")]
        public IActionResult Art()
        {
            //Testing ViewModel with ArtViewModel and connect to DB. Works!
            //Get all art from DB.
            var model = new ArtViewModel();
            model.Art = _context.Art.ToList();
            return View(model);
        }

        [Route("/gallery/item")]
        public async Task<IActionResult> Details(int? id)
        {
            //Doing same route as ArtController via DB and it also works!
            //Get art details by id (ArtID)
            if (id == null || _context.Art == null)
            {
                return NotFound();
            }

            var art = await _context.Art
                .FirstOrDefaultAsync(m => m.ArtID == id);
            if (art == null)
            {
                return NotFound();
            }

            return View(art);
        }

        [Route("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}