using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Business;
using LibroConsoleAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public  class DeleteBookMethodTests
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
                Pages = 200,
                Price = 19.99
            };
            await bookManager.AddAsync(newBook);

            // Act
            await bookManager.DeleteAsync(newBook.ISBN);

            // Assert
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Test]
        //Negative
        public async Task DeleteBookAsync_TryToDeleteWithStringEmptyISBN_ShouldThrowException()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() => bookManager.DeleteAsync(""));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("ISBN cannot be empty."));
        }


        [Test]
        //Negative
        public async Task DeleteBookAsync_TryToDeleteWithNullISBN_ShouldThrowException()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() => bookManager.DeleteAsync(null));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("ISBN cannot be empty."));
        }


        [Test]
        //Negative
        public async Task DeleteBookAsync_TryToDeleteWithWhiteSpaceISBN_ShouldThrowException()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() => bookManager.DeleteAsync("   "));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("ISBN cannot be empty."));
        }
    }
}
