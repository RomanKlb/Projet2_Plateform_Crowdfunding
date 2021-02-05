using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.Models
{
    interface IDalDonProjet : IDisposable
    {
        int CreerDon(string email, string nom, string prenom, decimal montant, string num_autorisation, int id_projet);
        don ObtenirDons(short id_don, string email, string prenom, decimal montant, string num_autorisation, System.DateTime date_heure, int id_projet);
        int CreerPortefeuilleFacture(short id_don, int id_facture, string libelle);
        int CreerFacture(short id_facture, short id_don, int id_portfeuille, string libelle, decimal montant, string numero);
        utilisateur ObtenirUser(string email);
        int GetId_Media(int id_projet);
        projet get_Projet(int id);
        void ModifierMontantCollecte(int id_projet, decimal montant);
        medium GetPic(int id);
        List<don> ObtenirTousLesDons();
        porfefeuille_factures GetPortefeuille_Don(int id_don);
    }
}