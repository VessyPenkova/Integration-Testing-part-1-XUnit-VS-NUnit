using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Business;
using LibroConsoleAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibroConsoleAPI.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public  class SearchByMethodTests
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
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
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
            var searchedBook = await bookManager.SearchByTitleAsync("Test Book");

            // Assert
            Assert.That(searchedBook.Count, Is.EqualTo(1));
        }

        [Test]
        //Negative
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
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
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => bookManager.SearchByTitleAsync("Hohoh"));


            // Assert
            Assert.That(exception.Message, Is.EqualTo("No books found with the given title fragment."));
        }
    }
}
