using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        //Adding to Cart
        public virtual void AddItem(Book book, int qty)
        {
            CartLine line = Lines //cartline is our container
                .Where(p => p.Book.BookId == book.BookId) //looks at list and checks if id passed in exists. if so, grab first one from group
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Book = book,
                    Quantity = qty,
                });
            }
            else
            {
                line.Quantity += qty;
            }
        }

        //Removing from Cart
        public virtual void RemoveLine(Book book) =>
            Lines.RemoveAll(x => x.Book.BookId == book.BookId);

        public virtual void Clear() => Lines.Clear();

        
        public decimal ComputeTotalSum() => (decimal)Lines.Sum(e => e.Book.Price * e.Quantity); //returns sum

        public class CartLine
        {
            public int CartLineID { get; set; }
            public Book Book { get; set; }
            public int Quantity { get; set; }
        }
    }
}
