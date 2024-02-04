using LibroConsoleAPI.Business;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class AddBookMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public AddBookMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        //Positive Test
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDbUsingBookManager = _bookManager.GetSpecificAsync(newBook.ISBN);
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal(newBook.Title, bookInDb.Title);
            Assert.Equal(newBook.Author, bookInDb.Author);
        }

        [Fact]
        //Negative Test
        public async Task AddBookAsync_Should_Throw_Exception_Longer_Title_When_Pass_Invalid_Title()
        {
            // Arrange
            var newBook = new Book
            {
                Title = new string('V', 500),
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);

        }

        [Fact]
        //Negative Test
        public async Task AddBookAsync_Should_Throw_Exception_When_Pass_Invalid_ISBN()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Valid Book Title",
                Author = "John Doe",
                ISBN = "kihjuk",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
            var bookInDB = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Fact]
        //Negative Test

        public async Task AddBookAsync_TryToAddBookWithInvalidYearMin_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Valid Book Title",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 1600,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
            var bookInDB = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }
        [Fact]
        //Negative Test
        public async Task AddBookAsync_TryToAddBookWithInvalidYearMax_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Valid Book Title",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2025,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
            var bookInDB = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Fact]
        //Negative Test
        public async Task AddBookAsync_TryToAddBookWithInvalidGenreMaxLength_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Valid Book Title",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = new string('V', 500),
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
            var bookInDB = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

    }
}
