using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EConference.DataAccess.Data;
using EConference.Models;

namespace EConference.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegisterPapersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterPapersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RegisterPapers
        public async Task<IActionResult> Index()
        {
            return View(await _context.PapersRegistered.ToListAsync());
        }

        // GET: Admin/RegisterPapers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerPaper = await _context.PapersRegistered
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registerPaper == null)
            {
                return NotFound();
            }

            return View(registerPaper);
        }

        // GET: Admin/RegisterPapers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/RegisterPapers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PaperId,PaperTopic,PaperTitle,Publisher,Participant")] RegisterPaper registerPaper)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registerPaper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registerPaper);
        }

        // GET: Admin/RegisterPapers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerPaper = await _context.PapersRegistered.FindAsync(id);
            if (registerPaper == null)
            {
                return NotFound();
            }
            return View(registerPaper);
        }

        // POST: Admin/RegisterPapers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PaperId,PaperTopic,PaperTitle,Publisher,Participant")] RegisterPaper registerPaper)
        {
            if (id != registerPaper.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registerPaper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegisterPaperExists(registerPaper.Id))
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
            return View(registerPaper);
        }

        // GET: Admin/RegisterPapers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerPaper = await _context.PapersRegistered
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registerPaper == null)
            {
                return NotFound();
            }

            return View(registerPaper);
        }

        // POST: Admin/RegisterPapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registerPaper = await _context.PapersRegistered.FindAsync(id);
            _context.PapersRegistered.Remove(registerPaper);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegisterPaperExists(int id)
        {
            return _context.PapersRegistered.Any(e => e.Id == id);
        }
    }
}
