using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public  class IntegrationTests
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
     

       

     

    }
}
