using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.ViewModels
{
    public class AfficherDonViewModel
    {
        public short id_don { get; set; }
        public string email { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public decimal montant { get; set; }
        public string num_autorisation { get; set; }
        public int id_projet { get; set; }
        public System.DateTime date_facture { get; set; }
        public string libelle { get; set; }
    }
}