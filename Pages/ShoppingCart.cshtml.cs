using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Pages
{
    public class ShoppingCartModel : PageModel
    {
        //create instance of repository
        private IBookStoreRepository repository;

        //constructor
        public ShoppingCartModel(IBookStoreRepository repo, Cart cartService)
        {
            repository = repo;
            Cart = cartService;
        }

        //properties
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }

        //methods
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        //post method
        public IActionResult OnPost(long bookid, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(p => p.BookId == bookid);

            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            Cart.AddItem(book, 1); //adds quantity of one

            HttpContext.Session.SetJson("cart", Cart);

            return RedirectToPage(new { returnUrl = returnUrl }); //first returnUrl belongs to object
        }

        public IActionResult OnPostRemove(long bookId, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);

            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            Cart.RemoveLine(book);

            //Cart.RemoveLine(Cart.Lines.First(cl => cl.Book.BookId == bookId).Book);

            HttpContext.Session.SetJson("cart", Cart);

            return RedirectToPage(new { returnUrl = returnUrl });
        }

    }
}
