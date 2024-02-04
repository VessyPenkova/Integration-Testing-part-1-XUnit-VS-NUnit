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
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public  class UpdateAsyncMethodTests
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
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
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
            newBook.Title = "Updated Title";

            // Act
            await bookManager.UpdateAsync(newBook);

            // Assert
            var bookInDb = dbContext.Books.FirstOrDefaultAsync(b => b.Title == newBook.Title);
            Assert.That(bookInDb.Result.Title, Is.EqualTo("Updated Title"));
            Assert.That(bookInDb.Result.Author, Is.EqualTo("John Doe"));
            Assert.That(bookInDb.Result.ISBN, Is.EqualTo("1234567890123"));
            Assert.That(bookInDb.Result.YearPublished, Is.EqualTo(2021));
            Assert.That(bookInDb.Result.Genre, Is.EqualTo("Fiction"));
            Assert.That(bookInDb.Result.Pages, Is.EqualTo(200));
            Assert.That(bookInDb.Result.Price, Is.EqualTo(19.99));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 1600,
                Genre = "Fiction",
                Pages = 200,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.UpdateAsync(newBook));
            // Assert

            Assert.That("Book is invalid.", Is.EqualTo(exception.Message));
        }
    }
}
