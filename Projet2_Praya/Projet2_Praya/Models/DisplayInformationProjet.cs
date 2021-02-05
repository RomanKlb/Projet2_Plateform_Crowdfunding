using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.Models
{
    public class DisplayInformationProjet
    {
        public int id_projet { get; set; }
        public string libelle { get; set; }
        public string description { get; set; }
        public string type_du_projet { get; set; }
        public string lib_etat { get; set; }
        public decimal montant_attendu { get; set; }
        public decimal montant_collecte { get; set; }
        public System.DateTime date_butoir { get; set; }
        public System.DateTime date_maj { get; set; }
        public string lib_portefeuille { get; set; }
        public System.DateTime date_debut { get; set; }

    }
}