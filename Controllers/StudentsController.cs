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

        // PUT: api/students/update-student/{id}
        [HttpPut("update-student/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            if (id != updatedStudent.StudentId)
            {
                return BadRequest("Student ID mismatch.");
            }

            if (updatedStudent == null)
            {
                return BadRequest("Updated student data is null.");
            }

            try
            {
                var existingStudent = await _context.Students.FindAsync(id);

                if (existingStudent == null)
                {
                    return NotFound($"Student with ID {id} not found.");
                }

                // Update the fields of the existing student
                existingStudent.StudentName = updatedStudent.StudentName;
                existingStudent.StudentEmail = updatedStudent.StudentEmail;

                _context.Entry(existingStudent).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(existingStudent);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, $"Concurrency error: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/students/{studentId}
        [HttpGet("get-student-from-id/{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            try
            {
                var student = await _context.Students
                                            .FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (student == null)
                {
                    return NotFound($"Student with ID {studentId} not found.");
                }

                return Ok(student);
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

        // DELETE: api/students/delete-student/{id}
        [HttpDelete("delete-student/{studentToDelete}")]
        public async Task<IActionResult> DeleteStudent(int studentToDelete)
        {
            try
            {
                var student = await _context.Students.FindAsync(studentToDelete);

                if (student == null)
                {
                    return NotFound($"Student with ID {studentToDelete} not found.");
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content response indicating successful deletion
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
