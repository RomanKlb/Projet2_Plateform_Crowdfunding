using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.Models
{
    interface IDalCreationProjet : IDisposable
    {

        bool ProjetExiste(string description);
        List<projet> ObtenirTousLesProjets();

        int CreerProjet(string libelle, string description, string type_du_projet, decimal montant_attendu, System.DateTime date_debut, System.DateTime date_butoir, string resp_projet);
        void ModifierProjet(int id_projet, string libelle, string description, string type_du_projet, decimal montant_attendu, DateTime date_debut, DateTime date_butoir);

        bool TestCreateAssociation(List<association> listeAsso);
        List<type_projet> ObtientTypeProjet();

        bool NomAssoExiste(string nom);
        bool RNAAssoExiste(string RNA);
        void SaveFirstForm(string RNA, string IBAN, string nom_association);
        int CreerAssociation(string RNA, string IBAN, string nom_association);

        void AjouterMedia(byte[] media, string lien, int id_projet, string type_media, string lib_image);
        int AddMedia(Byte[] media, string lien, int id_projet, string type_media, string lib_image);
        int DeleteMedia(int id);
        medium GetPic(int id);
        medium GetMedia(int id_projet);
        int GetId_Media(int id_projet);
        int GetId_Media_all(int id_projet, string lib_image);
        projet get_Projet(int id);
        void TestCreatePortefeuille(string resp_projet, string libelle, string niveau_habilitation, short Id_association);
        int CreerPortefeuilleProj(string resp_proj, string libelle);

        string CreerRespProj(string resp_email, string niveau_habilitation, short Id_association);
        resp_projet TestCreateResponsableP(string resp_projet);

        association ObtenirAssociation(short id_association, string nom_association, string RNA, string IBAN);

        type_projet GetTypeProjet(string type);
        association GetAssociation(int id_portefeuille);
        void ModifierProjetStat(int id_projet, string newstat);

        association ObtenirRespAsso(string email);
        association ObtenirAsso(int id_asso);
        void ModifierAsso(int id_association, string nom_association, string RNA, string IBAN);
        portefeuille_projet GetPortefeuille(int id_portefeuille);
    }
}