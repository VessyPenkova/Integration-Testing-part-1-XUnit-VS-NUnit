using LibroConsoleAPI.Business;
using LibroConsoleAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class SearchByTitleMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public SearchByTitleMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }


        [Fact]
        //Positive
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var result = await _bookManager.SearchByTitleAsync("To Kill a Mockingbird");

            // Assert
            Assert.Equal("To Kill a Mockingbird", result.FirstOrDefault().Title);

        }

        [Fact]
        //Negative
        public async Task SearchByTitleAsync_WithEmptyTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception  = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.SearchByTitleAsync(""));      

            // Assert
            Assert.Equal("Title fragment cannot be empty.", exception.Result.Message);
        }

        [Fact]
        //Negative
        public async Task SearchByTitleAsync_WithNoExistingTitleInTheDataBase_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.SearchByTitleAsync("First Steps in Automation QA"));

            // Assert
            Assert.Equal("No books found with the given title fragment.", exception.Result.Message);
        }
    }
}
