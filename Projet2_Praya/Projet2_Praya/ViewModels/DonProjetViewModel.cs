using Projet2_Praya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.ViewModels
{
    public class DonProjetViewModel
    {

        public short id_don { get; set; }
        public string email { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public decimal montant { get; set; }
        public string num_autorisation { get; set; }
        public System.DateTime date_heure { get; set; }
        public int id_projet { get; set; }

        public virtual projet projet { get; set; }
        public virtual justificatif justificatif { get; set; }
        public virtual ICollection<porfefeuille_factures> porfefeuille_factures { get; set; }

    }
}