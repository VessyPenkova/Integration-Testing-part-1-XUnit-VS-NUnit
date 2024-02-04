using LibroConsoleAPI.Business;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class GetAllBooksMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public GetAllBooksMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        //Positive
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var result = await _bookManager.GetAllAsync();

            // Assert
            Assert.Equal(10, result.ToList().Count);
        }

        [Fact]
        //Negative
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
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
            await _bookManager.DeleteAsync("1234567890123");

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetAllAsync());

            // Assert
            Assert.Equal("No books found.",exception.Result.Message);
        }
    }
}
