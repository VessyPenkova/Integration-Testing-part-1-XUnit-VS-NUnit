using LibroConsoleAPI.Business;
using LibroConsoleAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class GetSpecificMethodTest : IClassFixture<BookManagerFixture>
    {

        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public GetSpecificMethodTest(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        //Positive
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnCorrectBook()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var result = await _bookManager.GetSpecificAsync("9780385487256");

            // Assert
            Assert.Equal("9780385487256", result.ISBN);
            Assert.Equal("To Kill a Mockingbird", result.Title);
            Assert.Equal("Harper Lee", result.Author);
            Assert.Equal(1960, result.YearPublished);
            Assert.Equal("Novel", result.Genre);
            Assert.Equal(336, result.Pages);
            Assert.Equal(10.99, result.Price);
        }

        [Fact]
        //Negative
        public async Task GetSpecificAsync_WithStringEmptyPassed_As_Isbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.GetSpecificAsync(""));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }

        [Fact]
        //Negative
        public async Task GetSpecificAsync_WithWhiteSpacesPassed_As_Isbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.GetSpecificAsync("    "));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }

        [Fact]
        //Negative
        public async Task GetSpecificAsync_WithNullPassed_As_Isbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _bookManager.GetSpecificAsync(null));

            // Assert
            Assert.Equal("ISBN cannot be empty.", exception.Result.Message);
        }
    }
}
