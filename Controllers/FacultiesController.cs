using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Variant5;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace Variant5.Controllers
{
    //[Authorize(Roles = "admin, user")]
    public class FacultiesController : Controller
    {
        private readonly MyLabNewContext _context;

        public FacultiesController(MyLabNewContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faculties.ToListAsync());
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Teachers", new { id = faculty.Id, name = faculty.FacultyName });
        }

        // GET: Faculties/Create
        public IActionResult Create()
        {
            ViewBag.ProposedId = _context.Faculties.Count() + 1;

            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FacultyName")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FacultyName")] Faculty faculty)
        {
            if (id != faculty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.Id))
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
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(int id)
        {
            return _context.Faculties.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            var faculty_iterator = 0;
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {

                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Faculty newfac;
                                var c = (from cat in _context.Faculties
                                         where cat.FacultyName.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newfac = c[0];
                                    Console.WriteLine("c.Count: {0}", c.Count);
                                }
                                else
                                {
                                    faculty_iterator = _context.Faculties.Count() + 1;
                                    newfac = new Faculty();
                                    newfac.Id = faculty_iterator;
                                    newfac.FacultyName = worksheet.Name;
                                    //newfac.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Faculties.Add(newfac);
                                }
                                //перегляд усіх рядків
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {

                                    Teacher teacher;
                                    var t = (from tec in _context.Teachers
                                             where tec.Name.Contains(row.Cell(1).Value.ToString())
                                             select tec).ToList();
                                    if (t.Count > 0)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        teacher = new Teacher();
                                        teacher.Id = _context.Teachers.Count() + row.RowNumber();
                                        teacher.Name = row.Cell(1).Value.ToString();// Cell(1)
                                        teacher.FacultyId = newfac.Id;

                                        if (row.Cell(2).Value.ToString().Length > 0) //Cell(2)
                                        {
                                            Chair chair;
                                            var a = (from cha in _context.Chairs
                                                     where
                                                     cha.ChairName.Contains(row.Cell(2).Value.ToString())
                                                     select cha).ToList();
                                            if (a.Count > 0)
                                            {
                                                chair = a[0];
                                            }
                                            else
                                            {
                                                chair = new Chair();
                                                chair.Id = _context.Chairs.Count() + row.RowNumber();
                                                chair.ChairName = row.Cell(2).Value.ToString();
                                                //додати в контекст
                                                _context.Add(chair);
                                            }
                                            teacher.ChairId = chair.Id;
                                        }
                                        if (row.Cell(3).Value.ToString().Length > 0) //Cell(3)
                                        {
                                            Subject subject;
                                            var a = (from sub in _context.Subjects
                                                     where
                                                     sub.Name.Contains(row.Cell(3).Value.ToString())
                                                     select sub).ToList();
                                            if (a.Count > 0)
                                            {
                                                subject = a[0];
                                            }
                                            else
                                            {
                                                subject = new Subject();
                                                subject.Id = _context.Subjects.Count() + row.RowNumber();
                                                subject.Name = row.Cell(3).Value.ToString();
                                                //додати в контекст
                                                _context.Add(subject);
                                            }
                                            teacher.SubjectId = subject.Id;
                                        }
                                    }
                                        

                                        _context.Teachers.Add(teacher);

                                    
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var faculties = _context.Faculties.Include("Teachers").ToList();
                
                foreach (var c in faculties)
                {
                    var worksheet = workbook.Worksheets.Add(c.FacultyName); //назва факультету - назва листка
                    worksheet.Cell("A1").Value = "Ім'я викладача";
                    worksheet.Cell("B1").Value = "Кафедра";
                    worksheet.Cell("C1").Value = "Предмет";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var teachers = c.Teachers.ToList();
                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < teachers.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = teachers[i].Name;
                        var a = _context.Chairs.Where(c => c.Id == teachers[i].ChairId).ToList();
                        var teachers_chairs = a[0].ChairName.ToString();
                        worksheet.Cell(i + 2, 2).Value = teachers_chairs;
                        var b = _context.Subjects.Where(c => c.Id == teachers[i].SubjectId).ToList();
                        var teachers_subjects = b[0].Name.ToString();
                        worksheet.Cell(i + 2, 3).Value = teachers_subjects;

                        //var ab = _context.AuthorsBooks.Where(a => a.BookId ==
                        //books[i].Id).Include("Author").ToList();
                        //більше 4-ох нікуди писати
                        /*int j = 0;
                        foreach (var a in ab)
                        {
                            if (j < 5)
                            {
                                worksheet.Cell(i + 2, j + 2).Value = a.Author.Name;
                                j++;
                            }
                        }*/
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName =
                    $"teachers_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
