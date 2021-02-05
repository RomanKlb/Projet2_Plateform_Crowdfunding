using PagedList;
using Projet2_Praya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.ViewModels
{
    public class ProjetPageListViewModel
    {
        //  public projet Projet { get; set; }

        public PagedList<DisplayInformationCatalogue> SearchModel { get; set; }

        public CritRechercheViewModel CritRecherche { get; set; }

    }
}