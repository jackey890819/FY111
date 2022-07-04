using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;

namespace FY111.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassQuestionsController : ControllerBase
    {
        private readonly FY111Context _context;

        public ClassQuestionsController(FY111Context context)
        {
            _context = context;
        }

        // GET: api/ClassQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassQuestion>>> GetClassQuestions()
        {
            return await _context.ClassQuestions.ToListAsync();
        }

        // GET: api/ClassQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassQuestion>> GetClassQuestion(int id)
        {
            var classQuestion = await _context.ClassQuestions.FindAsync(id);

            if (classQuestion == null)
            {
                return NotFound();
            }

            return classQuestion;
        }

        // PUT: api/ClassQuestions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassQuestion(int id, ClassQuestion classQuestion)
        {
            if (id != classQuestion.Id)
            {
                return BadRequest();
            }

            _context.Entry(classQuestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassQuestionExists(id))
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

        // POST: api/ClassQuestions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ClassQuestion>> PostClassQuestion(ClassQuestion classQuestion)
        {
            _context.ClassQuestions.Add(classQuestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassQuestion", new { id = classQuestion.Id }, classQuestion);
        }

        // DELETE: api/ClassQuestions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClassQuestion>> DeleteClassQuestion(int id)
        {
            var classQuestion = await _context.ClassQuestions.FindAsync(id);
            if (classQuestion == null)
            {
                return NotFound();
            }

            _context.ClassQuestions.Remove(classQuestion);
            await _context.SaveChangesAsync();

            return classQuestion;
        }

        private bool ClassQuestionExists(int id)
        {
            return _context.ClassQuestions.Any(e => e.Id == id);
        }
    }
}
