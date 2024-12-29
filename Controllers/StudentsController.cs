using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reactCrudBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reactCrudBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet("students-list")]
        public async Task<IActionResult> GetStudentList()
        {
            try
            {
                var students = await _context.Students.ToListAsync();
                return Ok(students);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception (e.g., using a logging framework like Serilog)
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/students
        [HttpPost("create-student")]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Student data is null.");
            }

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetStudentList), new { id = student.StudentId }, student);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
