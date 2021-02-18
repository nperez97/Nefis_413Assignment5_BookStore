using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class EFBookStoreRepository : IBookStoreRepository
    {
        private BookStoreDbContext _context;

        //Constructor
        public EFBookStoreRepository(BookStoreDbContext context) //calling class passes in context
        {
            _context = context;
        }
        public IQueryable<Book> Books => _context.Books;
    }
}
