using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.Models
{
    public interface IDalCatalogue : IDisposable
    {

        List<projet> ObtenirTousLesProjets();

        medium GetPic(int id);
        medium GetId_Media(int id_projet);
    }
}