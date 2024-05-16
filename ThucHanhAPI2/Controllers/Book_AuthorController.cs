using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThucHanhAPI2.Data;
using ThucHanhAPI2.Model.Domain;

namespace ThucHanhAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Book_AuthorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Book_AuthorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book_Author>>> GetBooks_Author()
        {
            return await _context.Books_Author.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book_Author>> GetBook_Author(int id)
        {
            var book_Author = await _context.Books_Author.FindAsync(id);

            if (book_Author == null)
            {
                return NotFound();
            }

            return book_Author;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook_Author(int id, Book_Author book_Author)
        {
            if (id != book_Author.Id)
            {
                return BadRequest();
            }

            _context.Entry(book_Author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Book_AuthorExists(id))
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


        [HttpPost]
        public async Task<ActionResult<Book_Author>> PostBook_Author(Book_Author book_Author)
        {
            _context.Books_Author.Add(book_Author);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Book_AuthorExists(book_Author.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBook_Author", new { id = book_Author.Id }, book_Author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook_Author(int id)
        {
            var book_Author = await _context.Books_Author.FindAsync(id);
            if (book_Author == null)
            {
                return NotFound();
            }

            _context.Books_Author.Remove(book_Author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Book_AuthorExists(int id)
        {
            return _context.Books_Author.Any(e => e.Id == id);
        }
    }
}
