using LibroConsoleAPI.Business;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class DeleteBookMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public DeleteBookMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        //Positive Case
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
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
            await _bookManager.AddAsync(newBook);

            // Act
            await _bookManager.DeleteAsync(newBook.ISBN);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDb);
        }


        [Fact]
        //Positive Case
        public async Task DeleteBookAsync_ShouldRemoveBookFromDb()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            //var bookToDelete =  await _bookManager.SearchByTitleAsync("To Kill a Mockingbird");
            await _bookManager.DeleteAsync("9780385487256");

            // Assert
            var listOfbBooksInDb = _dbContext.Books.ToList();
            Assert.Equal(9, listOfbBooksInDb.Count);
        }

      
        [Fact]
        //Negative
        public async Task DeleteBookAsync_TryToDeleteWithNullISBN_ShouldThrowException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.DeleteAsync(null));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }


        [Fact]
        //Negative
        public async Task DeleteBookAsync_TryToDeleteWithWhiteSpaceISBN_ShouldThrowException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.DeleteAsync("  "));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }


    }
}
