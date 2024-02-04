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
    public class AddBookMethodTest
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
        public async Task AddBookAsync_ShouldAddBook()
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
            await bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.That(bookInDb.Title, Is.EqualTo("Test Book"));
            Assert.That(bookInDb.Author, Is.EqualTo("John Doe"));
            Assert.That(bookInDb.ISBN, Is.EqualTo("1234567890123"));
            Assert.That(bookInDb.YearPublished, Is.EqualTo(2021));
            Assert.That(bookInDb.Pages, Is.EqualTo(100));
            Assert.That(bookInDb.Price, Is.EqualTo(19.99));

        }

        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithLongerTitle_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = new string('V', 500),
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 200,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithLongerAutor_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = new string('V', 500),
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 200,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithISBNonDigit_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "abcdefghi",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 200,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithLowerYear_ShouldThrowException()
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
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithHigherYear_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2030,
                Genre = "Fiction",
                Pages = 200,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithLongerGenre_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2030,
                Genre = new string('V', 500),
                Pages = 200,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }

        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithInvalidPages_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = -100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert          
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }
        [Test]
        //Negative
        public async Task AddBookAsync_TryToAddBookWithNegativePrice_ShouldThrowException()
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
                Price = -19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(newBook));

            // Assert          
            Assert.That(exception.ValidationResult.ErrorMessage, Is.EqualTo("Book is invalid."));
            var bookInDB = await dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDB);
        }
    }
}
