using Projet2_Praya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.ViewModels
{
    public class UtilisateurViewModel
    {
        public utilisateur Utilisateur { get; set; }
        public bool Authentifie { get; set; }
    }
}