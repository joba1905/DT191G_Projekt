using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DT191GProjekt.Data;
using DT191GProjekt.Models;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DT191GProjekt.Controllers
{
    public class ArtController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArtController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Art
        [Authorize]
        [Route("/admin")]
        public async Task<IActionResult> Index()
        {
              return _context.Art != null ? 
                          View(await _context.Art.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Art'  is null.");
        }

        // GET: Art/Details/5
        [Authorize]
        [Route("/admin/details")]
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Art/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Art/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtID,Title,Description,ImageFile,AltText")] Art art)
        {
            if (ModelState.IsValid)
            {
                //Check if post contains image
                if (art.ImageFile != null)
                {
                    //Variables for search path and filename of image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string filename = Path.GetFileNameWithoutExtension(art.ImageFile.FileName);
                    string extension = Path.GetExtension(art.ImageFile.FileName);

                    //Add a unique image filename with a string based on current timestamp 
                    art.ImageName = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;

                    string path = Path.Combine(wwwRootPath + "/img/", filename);
                    
                    //Use function Filestream to save image to wwwRoot
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await art.ImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(art);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(art);
        }

        // GET: Art/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Art == null)
            {
                return NotFound();
            }

            var art = await _context.Art.FindAsync(id);
            if (art == null)
            {
                return NotFound();
            }
            return View(art);
        }

        // POST: Art/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtID,Title,Description,ImageName,AltText")] Art art)
        {
            if (id != art.ArtID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(art);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtExists(art.ArtID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(art);
        }

        // GET: Art/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Art/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Art == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Art'  is null.");
            }
            var art = await _context.Art.FindAsync(id);
            if (art != null)
            {
                //Check if image file exists in wwwroot and delete it
                //Path to wwwroot and image-file
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var imagepath = Path.Combine(wwwRootPath + "/img/",art.ImageName);

                //If image exist then delete
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                } 

                //Delete data from database
                _context.Art.Remove(art);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtExists(int id)
        {
          return (_context.Art?.Any(e => e.ArtID == id)).GetValueOrDefault();
        }
    }
}
