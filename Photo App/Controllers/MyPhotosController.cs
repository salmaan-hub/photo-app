using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Photo_App.Models;
using Microsoft.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Photo_App.Controllers
{
    public class MyPhotosController : Controller
    {
        private readonly AlbumDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        
      

        public MyPhotosController(AlbumDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: MyPhotos
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyPhotos.ToListAsync());

        }

        // GET: MyPhotos/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var myPhotos = await _context.MyPhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myPhotos == null)
            {
                return NotFound();
            }

            return View(myPhotos);
        }

        // GET: MyPhotos/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
       

        // POST: MyPhotos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,PhotoName,ImageFile,Date")] MyPhotos myPhotos)
        {
            if (ModelState.IsValid)
            {
                String wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(myPhotos.ImageFile.FileName);
                string extensioName = Path.GetExtension(myPhotos.ImageFile.FileName);
                myPhotos.PhotoName = fileName = fileName + extensioName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                myPhotos.Url = path;
                myPhotos.Email = @User.Identity.Name;
                using (var fileStream =  new FileStream(path,FileMode.Create)) {
                    await myPhotos.ImageFile.CopyToAsync(fileStream);                }

                

                    _context.Add(myPhotos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myPhotos);
        }

        // GET: MyPhotos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myPhotos = await _context.MyPhotos.FindAsync(id);
            if (myPhotos == null)
            {
                return NotFound();
            }
            return View(myPhotos);
        }

        // POST: MyPhotos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhotoName,Url,Date")] MyPhotos myPhotos)
        {
            if (id != myPhotos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myPhotos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                        throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(myPhotos);
        }

        // GET: MyPhotos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var myPhotos = await _context.MyPhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myPhotos == null)
            {
                return NotFound();
            }

            return View(myPhotos);
        }

        // POST: MyPhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var MyPhotos = await _context.MyPhotos.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", MyPhotos.PhotoName);
            if(System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);


            var myPhotos = await _context.MyPhotos.FindAsync(id);
            _context.MyPhotos.Remove(myPhotos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyPhotosExists(int id)
        {
            return _context.MyPhotos.Any(e => e.Id == id);
        }
    }
}
