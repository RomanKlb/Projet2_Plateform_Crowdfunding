using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.Models
{
    interface IDalAdmin : IDisposable
    {


        Boolean AjouterUtilisateur(string email, string motDePasse, string nom, string prenom);
        Boolean EditUtilisateur(string email, string motDePasse, string nom, string prenom);
        utilisateur Authentifier(string email, string motDePasse);
        utilisateur ObtenirUtilisateur(string email);
        List<utilisateur> ObtientTousLesUtilisateurs();

        List<type_projet> ObtientTousLesTypesProjets();
        Boolean CreerTypeProjet(string type, string libelle);
        Boolean ModifierTypeProjet(string type, string libelle);
        type_projet ObtenirTypeProjet(string type);

        List<projet> ObtenirTousLesProjets();
        portefeuille_projet GetPortefeuille(int id_portefeuille);

        administrateur ObtenirAdmin(string email);
        resp_projet GetResponsableProjet(string resp_email);

        don GetDon(string email);
    }
}