using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Projet2_Praya.Models
{
    public class DalAdmin : IDalAdmin
    {

        private projet2Entities DBcontext;

        public DalAdmin()
        {
            DBcontext = new projet2Entities();
        }
        public Boolean AjouterUtilisateur(string email, string motDePasse, string nom, string prenom)
        {
            utilisateur tstUser = ObtenirUtilisateur(email);

            if (tstUser == null)
            {
                string motDePasseEncode = EncodeMD5(motDePasse);
                utilisateur utilisateur = new utilisateur()
                {
                    utilisateur_email = email,
                    mot_de_passe = motDePasseEncode,
                    nom = nom,
                    prenom = prenom,
                    date_maj = DateTime.Now
                };

                DBcontext.utilisateurs.Add(utilisateur);
                DBcontext.SaveChanges();
                return true;
            }
            return false;
        }

        public Boolean EditUtilisateur(string email, string motDePasse, string nom, string prenom)
        {
            utilisateur User = DBcontext.utilisateurs.Find(email);
            if (User != null)
            {
                string motDePasseEncode = EncodeMD5(motDePasse);

                User.nom = nom;
                User.prenom = prenom;
                User.mot_de_passe = motDePasseEncode;
                User.date_maj = DateTime.Now;

                DBcontext.SaveChanges();
                return true;
            }
            return false;
        }

        public utilisateur Authentifier(string email, string motDePasse)
        {
            string motDePasseEncode = EncodeMD5(motDePasse);
            return DBcontext.utilisateurs.FirstOrDefault(u => u.utilisateur_email == email
            && u.mot_de_passe == motDePasseEncode);
        }


        public Boolean CreerTypeProjet(string type, string libelle)
        {
            type_projet TypeProj = DBcontext.type_projet.Find(type);
            if (TypeProj == null)
            {
                type_projet newtype = new type_projet()
                {
                    type = type,
                    libelle = libelle
                };

                DBcontext.type_projet.Add(newtype);
                DBcontext.SaveChanges();
                return true;
            }
            return false;
        }

        public Boolean ModifierTypeProjet(string type, string libelle)
        {
            type_projet TypeProj = DBcontext.type_projet.Find(type);
            if (TypeProj != null)
            {
                TypeProj.libelle = libelle;

                DBcontext.SaveChanges();
                return true;
            }
            return false;
        }

        public utilisateur ObtenirUtilisateur(string email)
        {
            return DBcontext.utilisateurs.FirstOrDefault(u => u.utilisateur_email == email);
        }

        public administrateur ObtenirAdmin(string email)
        {
            return DBcontext.administrateurs.FirstOrDefault(u => u.administrateur_email == email);
        }

        public List<type_projet> ObtientTousLesTypesProjets()
        {
            return DBcontext.type_projet.ToList();
        }

        public type_projet ObtenirTypeProjet(string type)
        {
            return DBcontext.type_projet.FirstOrDefault(t => t.type == type);
        }

        public List<utilisateur> ObtientTousLesUtilisateurs()
        {
            return DBcontext.utilisateurs.ToList();
        }

        public List<projet> ObtenirTousLesProjets()
        {
            return DBcontext.projets.ToList();
        }


        public portefeuille_projet GetPortefeuille(int id_portefeuille)
        {
            return DBcontext.portefeuille_projet.Find(id_portefeuille);

        }

        public resp_projet GetResponsableProjet(string resp_email)
        {
            return DBcontext.resp_projet.Find(resp_email);
        }

        public don GetDon(string email)
        {
            return DBcontext.dons.FirstOrDefault(t => t.email == email);
        }


        private string EncodeMD5(string motDePasse)
        {
            string motDePasseSel = "Projet2" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
        }

            public void Dispose()
        {
            DBcontext.Dispose();
        }
    }
}