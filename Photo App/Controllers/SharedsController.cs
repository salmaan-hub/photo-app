using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Photo_App.Models;

namespace Photo_App.Controllers
{

    public class SharedsController : Controller
    {
        private readonly AlbumDBContext _context;
         

        public SharedsController(AlbumDBContext context)
        {
            _context = context;
        }

        // GET: Shareds
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shared.ToListAsync());
        }
        [Authorize]
        public IActionResult Share(string sharedName) {

            var newShare = new Shared() {
                
                PhotoName = sharedName
            };
            _context.Shared.Add(newShare);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = newShare.Id });
        }
        // GET: Shareds/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
           

            var shared = await _context.Shared
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shared == null)
            {
                return NotFound();
            }

            return View(shared);
        }

       
        // GET: Shareds/Create
        public IActionResult Create(int str)
        {
            return View();
        }

        // POST: Shareds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Email,PhotoName,ShareName")] Shared shared)
        {
            
            
            if (ModelState.IsValid)
            {
                
                _context.Add(shared);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(shared);
        }

        // GET: Shareds/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            

            var shared = await _context.Shared.FindAsync(id);
            if (shared == null)
            {
                return NotFound();
            }
            return View(shared);
        }

        // POST: Shareds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,PhotoName,ShareName")] Shared shared)
        {
            if (id != shared.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shared);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                        throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shared);
        }


        // GET: Shareds/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var shared = await _context.Shared
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shared == null)
            {
                return NotFound();
            }

            return View(shared);
        }

        // POST: Shareds/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shared = await _context.Shared.FindAsync(id);
            _context.Shared.Remove(shared);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SharedExists(int id)
        {
            return _context.Shared.Any(e => e.Id == id);
        }
    }
}
