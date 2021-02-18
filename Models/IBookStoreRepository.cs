using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public interface IBookStoreRepository //use word interface to make it a template
    {
        IQueryable<Book> Books { get; }
    }
}
