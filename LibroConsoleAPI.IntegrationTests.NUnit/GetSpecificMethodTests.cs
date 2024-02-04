using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Business;
using LibroConsoleAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.DataAccess.Contracts;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public  class GetSpecificMethodTests
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
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
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
            var result = await bookManager.GetSpecificAsync("1234567890123");

            // Assert
            Assert.That(result.ISBN, Is.EqualTo("1234567890123"));
        }

        [Test]
        //Negative
        public async Task GetSpecificAsync_WithEmptyStringIsbn_ShouldThrowKeyNotFoundException()
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
            var exception = Assert.ThrowsAsync< ArgumentException> (()=> bookManager.GetSpecificAsync(""));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("ISBN cannot be empty."));
        }

        [Test]
        public async Task GetSpecificAsync_WithNullIsbn_ShouldThrowKeyNotFoundException()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() => bookManager.GetSpecificAsync(null));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("ISBN cannot be empty."));
        }

        [Test]
        public async Task GetSpecificAsync_WithWhiteSpaceIsbn_ShouldThrowKeyNotFoundException()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() => bookManager.GetSpecificAsync("   "));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("ISBN cannot be empty."));
        }

    }
}
