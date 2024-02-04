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
    public class UpdateMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public UpdateMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Valid Book Title",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2024,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };
            await _bookManager.AddAsync(newBook);
            newBook.Title = "UpdatedTitle";

            // Act
            await _bookManager.UpdateAsync(newBook);

            // Assert
            var bookInDb = _dbContext.Books.FirstOrDefaultAsync(b =>b.Title== newBook.Title);
            Assert.Equal("UpdatedTitle", bookInDb.Result.Title);
            Assert.Equal("John Doe", bookInDb.Result.Author);
            Assert.Equal("1234567890123", bookInDb.Result.ISBN);
            Assert.Equal(2024, bookInDb.Result.YearPublished);
            Assert.Equal("Fiction", bookInDb.Result.Genre);
            Assert.Equal(100, bookInDb.Result.Pages);
            Assert.Equal(19.99, bookInDb.Result.Price);
        }

        [Fact]
        //Negative
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
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
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.UpdateAsync(newBook));
            // Assert

            Assert.Equal("Book is invalid.", exception.Result.Message);
        }
    }
}
