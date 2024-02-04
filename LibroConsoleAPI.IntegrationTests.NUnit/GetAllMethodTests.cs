using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Business;
using LibroConsoleAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibroConsoleAPI.Data.Models;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public class GetAllMethodTests
    {
        private TestLibroDbContext dbContext;
        private IBookManager bookManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestLibroDbContext();
            this.bookManager = new BookManager(new BookRepository(this.dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        //Positive
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 200,
                Price = 19.99
            };
            await bookManager.AddAsync(newBook);

            // Act
            var result = await  bookManager.GetAllAsync();

            // Assert
            Assert.That(1, Is.EqualTo(result.ToList().Count));
        }

        [Test]
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
                Pages = 200,
                Price = 19.99
            };
            await bookManager.AddAsync(newBook);
            await bookManager.DeleteAsync("1234567890123");

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => bookManager.GetAllAsync());

            // Assert
            Assert.That(exception.Message,Is.EqualTo( "No books found."));
        }
    }
}
