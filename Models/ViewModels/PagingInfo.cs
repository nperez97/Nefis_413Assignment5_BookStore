using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class PagingInfo //this model is meant to create models specifically for the view
    {
        public int TotalNumItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        //tag helpers help dynamically render html
        public int TotalPages => (int)(Math.Ceiling((decimal)TotalNumItems / ItemsPerPage)); //force as decimal. divide. round. then cast to int so it doesn't break
    }
}
