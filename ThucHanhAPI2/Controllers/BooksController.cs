using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ThucHanhAPI2.CustomActionFilter;
using ThucHanhAPI2.Data;
using ThucHanhAPI2.Model.Domain;
using ThucHanhAPI2.Model.DTO;

using ThucHanhAPI2.Respository;

namespace ThucHanhAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;
        public BooksController(AppDbContext dbContext, IBookRepository bookRepository, ILogger<BooksController> logger)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [HttpGet("get-all-books")]
        [Authorize(Roles = "Read")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,[FromQuery] string? sortBy, [FromQuery] bool isAscending,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            _logger.LogInformation("GetAll Book Action method was invoked");
            _logger.LogWarning("This is a warning log");
            _logger.LogError("This is a error log");
            // su dung reposity pattern 
            var allBooks = _bookRepository.GetAllBooks(filterOn, filterQuery, sortBy,
           isAscending, pageNumber, pageSize);

            //debug
            _logger.LogInformation($"Finished GetAllBook request with data {{JsonSerializer.Serialize(allBooks)}}");
            return Ok(allBooks);
        }

        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            return Ok(bookWithIdDTO);
        }
        [HttpPost("add - book")]
        [Authorize(Roles ="Write")]
        [ValidateModel]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            if (!ValidateAddBook(addBookRequestDTO))
            {
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
                return Ok(bookAdd);
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("update-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult UpdateBooksById(int id, [FromBody] AddBookRequestDTO bookDTO)
        {
            var updateBook = _bookRepository.UpdateBookById(id, bookDTO);
            return Ok(updateBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult DeleteBooksById(int id)
        {
            var deleteBook = _bookRepository.DeleteBookById(id);
            return Ok(deleteBook);
        }
        #region Private methods 
        private bool ValidateAddBook(AddBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), $"Please add book data"); 
            return false;
            }
            // kiem tra Description NotNull
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description),
               $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }
            // kiem tra rating (0,5) 
            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate),
               $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}

