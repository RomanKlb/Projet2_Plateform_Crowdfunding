using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projet2_Praya.Models;

namespace Projet2_Praya.ViewModels
{
    public class CataloguePageList
    {
        public List<DisplayInformationCatalogue> DICList { get; set; }
        public Pagination pagination { get; set; }

    }


    public class Pagination
    {
        public int TotalitemsNumber;
        public int CurrentPage;
        public int FirstPage;
        public int LastPage;
        public int PageSize;

        public Pagination(int totalNumberItem, int pageSize)
        {
            PageSize = pageSize;
            TotalitemsNumber = totalNumberItem;
            CurrentPage = 1;
            LastPage = totalNumberItem / PageSize;
            FirstPage = 1;
        }
    }
}