using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Variant5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly MyLabNewContext _context;
        public ChartsController(MyLabNewContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var faculties = _context.Faculties.Include(b => b.Teachers).ToList();
            List<object> facTeacher = new List<object>();

            facTeacher.Add(new[] { "Факультет", "Кількість викладачів" });

            foreach (var c in faculties)
            {
                facTeacher.Add(new object[] { c.FacultyName, c.Teachers.Count() });
            }
            return new JsonResult(facTeacher);
        }
    }
}
