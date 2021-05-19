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
    public class TeachersController : Controller
    {
        private readonly MyLabNewContext _context;

        public TeachersController(MyLabNewContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(int? id, string? name)
        {
            /*var MyLabNewContext = _context.Teachers.Include(t => t.Chair).Include(t => t.Faculty).Include(t => t.Subject);
            return View(await MyLabNewContext.ToListAsync());*/


            if (id == null) return RedirectToAction("Faculties", "Index");

            ViewBag.FacultyId = id;
            ViewBag.FacultyName = name;
            var teachersByFaculty = _context.Teachers.Where(b => b.FacultyId == id).Include(b => b.Faculty).Include(b => b.Chair).Include(b => b.Subject);
            return View(await teachersByFaculty.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Chair)
                .Include(t => t.Faculty)
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create(int facultyId)
        {
            ViewData["ChairId"] = new SelectList(_context.Chairs, "Id", "ChairName");

            //ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "FacultyName");


            ViewBag.FacultyId = facultyId;
            ViewBag.FacultyName = _context.Faculties.Where(c => c.Id == facultyId).FirstOrDefault().FacultyName;
            ViewBag.ProposedId = _context.Teachers.Count() + 1;
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int facultyId, [Bind("Id,Name,ChairId,SubjectId")] Teacher teacher)
        {
            teacher.FacultyId = facultyId;
            //Console.WriteLine(teacher.SubjectId.ToString());
            //Console.WriteLine(teacher.ChairId.ToString());
            ViewData["ChairId"] = new SelectList(_context.Chairs, "Id", "ChairName", teacher.ChairId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", teacher.SubjectId);

            //if (ModelState.IsValid)
            //{
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Teachers", new { id = facultyId, name = _context.Faculties.Where(c => c.Id == facultyId).FirstOrDefault().FacultyName });

            //}
            //ViewData["ChairId"] = new SelectList(_context.Chairs, "Id", "ChairName", teacher.ChairId);
            // ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "FacultyName", teacher.FacultyId);
            //ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", teacher.SubjectId);
            //return View(teacher);

            //return RedirectToAction("Index", "Teachers", new { id = facultyId, name = _context.Faculties.Where(c => c.Id == facultyId).FirstOrDefault().FacultyName });

        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewData["ChairId"] = new SelectList(_context.Chairs, "Id", "ChairName", teacher.ChairId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "FacultyName", teacher.FacultyId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", teacher.SubjectId);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FacultyId,ChairId,SubjectId")] Teacher teacher)
        {
            var facultyId = teacher.FacultyId;
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Teachers", new { id = facultyId, name = _context.Faculties.Where(c => c.Id == facultyId).FirstOrDefault().FacultyName });
            }
            ViewData["ChairId"] = new SelectList(_context.Chairs, "Id", "ChairName", teacher.ChairId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "FacultyName", teacher.FacultyId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name", teacher.SubjectId);
            _context.Update(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Teachers", new { id = facultyId, name = _context.Faculties.Where(c => c.Id == facultyId).FirstOrDefault().FacultyName });
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Chair)
                .Include(t => t.Faculty)
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            var facultyId = teacher.FacultyId;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Teachers", new { id = facultyId, name = _context.Faculties.Where(c => c.Id == facultyId).FirstOrDefault().FacultyName });
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
