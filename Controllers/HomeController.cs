using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models.ViewModels;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IBookStoreRepository _repository;

        //making variable to indicate how many books to display for pagination
        public int PageSize = 5;

        public HomeController(ILogger<HomeController> logger, IBookStoreRepository repository) //pass repository
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index(string category, int page = 1) //pretend two is passed in
        {
            //allows iqueryable to be passed in
            return View(new BookListViewModel
            {
                Books = _repository.Books
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.BookId) // orderby is written in language called linq, which helps you query the database
                    .Skip((page - 1) * PageSize) //2-1. then does 1 times 20 (assuming we set it to 20)
                    .Take(PageSize) //takes next 20 to display
                ,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalNumItems = category == null ? _repository.Books.Count() :
                        _repository.Books.Where(x => x.Category == category).Count()
                },
                Type = category
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
