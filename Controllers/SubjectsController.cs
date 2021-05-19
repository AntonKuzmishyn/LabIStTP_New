using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Variant5;

namespace Variant5.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly MyLabNewContext _context;

        public SubjectsController(MyLabNewContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            var myLabNewContext = _context.Subjects.Include(s => s.Day).Include(s => s.Group).Include(s => s.Room).Include(s => s.Teacher);
            return View(await myLabNewContext.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Day)
                .Include(s => s.Group)
                .Include(s => s.Room)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "DayName");
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "GroupName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            ViewBag.ProposedId = _context.Subjects.Count() + 1;

            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TeacherId,RoomId,GroupId,DayId,LecNum")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "DayName", subject.DayId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "GroupName", subject.GroupId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", subject.RoomId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", subject.TeacherId);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "DayName", subject.DayId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "GroupName", subject.GroupId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", subject.RoomId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", subject.TeacherId);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TeacherId,RoomId,GroupId,DayId,LecNum")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "DayName", subject.DayId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "GroupName", subject.GroupId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", subject.RoomId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name", subject.TeacherId);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Day)
                .Include(s => s.Group)
                .Include(s => s.Room)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
