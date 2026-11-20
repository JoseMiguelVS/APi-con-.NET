using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.Entities;

namespace UTTStudents.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentsDbContext _context;
        // Constructor
        public StudentsController(StudentsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: api/Students
        public async Task<ActionResult<IEnumerable<StudentsDTO>>> GetAllStudents()
        {
            var students = await _context.Students
                .Select(s => new StudentsDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    LastName = s.LastName,
                    SecondLastName = s.SecondLastName,
                    Email = s.Email,
                    CreateDate = s.CreateDate
                })
                .ToListAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentsDTO>> getStudents(int id)
        {
            var student = await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentsDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    LastName = s.LastName,
                    SecondLastName = s.SecondLastName,
                    Email = s.Email,
                    CreateDate = s.CreateDate
                })
                .FirstOrDefaultAsync();
            return Ok(student);
        }
        [HttpPost]
        public async Task<ActionResult<Students>> InsertStudent(StudentCreateDTO createDTO)
        {
            var insertStudent = new Students
            {
                Name = createDTO.Name,
                LastName = createDTO.LastName,
                SecondLastName = createDTO.SecondLastName,
                Email = createDTO.Email
            };

            _context.Students.Add(insertStudent);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(int id, UpdateDTO updateDTO)
        {
            var updateStudent = await _context.Students.FindAsync(id);

            if (updateStudent == null)
            {
                return NotFound();
            }

            updateStudent.Name = updateDTO.Name;
            updateStudent.LastName = updateDTO.LastName;
            updateStudent.SecondLastName = updateDTO.SecondLastName;
            updateStudent.Email = updateDTO.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var deleteStudent = await _context.Students.FindAsync(id);
            if (deleteStudent == null)
            {
                return NotFound();
            }
            _context.Students.Remove(deleteStudent);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
