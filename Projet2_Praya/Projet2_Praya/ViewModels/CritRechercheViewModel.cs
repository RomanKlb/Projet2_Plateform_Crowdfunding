using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.ViewModels
{
    public class CritRechercheViewModel
    {
        public string libelle { get; set; }
        public string description { get; set; }
        public string type_du_projet { get; set; }
        public decimal montant_attenduMin { get; set; }
        public decimal montant_attenduMax { get; set; }
        public decimal montant_collecteMin { get; set; }
        public decimal montant_collecteMax { get; set; }
        public System.DateTime date_butoirMin { get; set; }
        public System.DateTime date_butoirMax { get; set; }
        public string nom_association { get; set; }

    }
}