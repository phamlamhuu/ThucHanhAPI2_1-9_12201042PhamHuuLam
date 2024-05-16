using Microsoft.EntityFrameworkCore;
using ThucHanhAPI2.Data;
using ThucHanhAPI2.Model.Domain;
using ThucHanhAPI2.Model.DTO;
using ThucHanhAPI2.Respository;

namespace ThucHanhAPI2.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BookDTO> GetAllBooks(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allBooks = _dbContext.Books.Select(Books => new BookDTO()
            {
                Id = Books.Id,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.IsRead ? Books.DateRead.Value : null,
                Rate = Books.IsRead ? Books.Rate.Value : null,
                Genre = Books.Genre,
                CoverUrl = Books.CoverUrl,
                PublisherName = Books.Publisher.Name,
                AuthorName = Books.Book_Author.Select(n => n.Author.FullName).ToList()
            }).AsQueryable();
            if (string.IsNullOrWhiteSpace(filterOn) == false &&
           string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Title.Contains(filterQuery));
                }
            }
            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending ? allBooks.OrderBy(x => x.Title) :
                   allBooks.OrderByDescending(x => x.Title);
                }
            }
            //pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return allBooks.Skip(skipResults).Take(pageSize).ToList();
        }
        public BookDTO GetBookById(int id)
        {
            var bookWithDomain = _dbContext.Books.Where(n => n.Id == id);
            //Map Domain Model to DTOs
            var bookWithIdDTO = bookWithDomain.Select(book => new BookDTO()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
            }).FirstOrDefault();
            return bookWithIdDTO;

        }
        public AddBookRequestDTO AddBookById(AddBookRequestDTO addBookRequestDTO)
        {
            var bookDomainModel = new Book
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherID = addBookRequestDTO.PublisherID
            };
            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();
            foreach (var id in addBookRequestDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = bookDomainModel.Id,
                    Id = id
                };
                _dbContext.Books_Author.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return addBookRequestDTO;
        }
        public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                bookDomain.Title = bookDTO.Title;
                bookDomain.Description = bookDTO.Description;
                bookDomain.IsRead = bookDTO.IsRead;
                bookDomain.DateRead = bookDTO.DateRead;
                bookDomain.Rate = bookDTO.Rate;
                bookDomain.Genre = bookDTO.Genre;
                bookDomain.CoverUrl = bookDTO.CoverUrl;
                bookDomain.DateAdded = bookDTO.DateAdded;
                bookDomain.PublisherID = bookDTO.PublisherID;
                _dbContext.SaveChanges();
            }
            var authorDomain = _dbContext.Books_Author.Where(a => a.BookId == id).ToList();
            if (authorDomain != null)
            {
                _dbContext.Books_Author.RemoveRange(authorDomain);
                _dbContext.SaveChanges();
            }
            foreach (var authorid in bookDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = id,
                    Id = authorid
                };
                _dbContext.Books_Author.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return bookDTO;
        }
        public Book? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
            return bookDomain;
        }

        public AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}

