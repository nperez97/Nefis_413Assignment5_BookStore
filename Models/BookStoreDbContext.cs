using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookStoreDbContext : DbContext //inherit db context into bookstore context
    {
        //constructor
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) //takes from base(options)
        {

        }
        //property
        public DbSet<Book> Books { get; set; }
    }
}
