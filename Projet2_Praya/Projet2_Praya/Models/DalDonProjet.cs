using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.Models
{
    public class DalDonProjet : IDalDonProjet
    {
        private projet2Entities DprojBdd;

        public DalDonProjet()
        {
            DprojBdd = new projet2Entities();
        }

        public void Dispose()
        {
            DprojBdd.Dispose();
        }
        public int CreerDon(string email, string nom, string prenom, decimal montant, string num_autorisation, int id_projet)
        {

            don MyDonation = new don()
            {
                email = email,
                nom = nom,
                prenom = prenom,
                montant = montant,
                num_autorisation = num_autorisation,
                date_heure = DateTime.Now,
                id_projet = id_projet,

            };
            DprojBdd.dons.Add(MyDonation);

            DprojBdd.SaveChanges();

            return MyDonation.id_don;

        }

        public int CreerFacture(short id_facture, short id_don, int id_portfeuille, string libelle, decimal montant, string numero)
        {

            facture MyBill = new facture()

            {
                libelle = libelle,
                montant = montant,
                numero = numero,
            };
            DprojBdd.factures.Add(MyBill);

            DprojBdd.SaveChanges();

            return MyBill.id_facture;
        }

        public int CreerPortefeuilleFacture(short id_don, int id_facture, string libelle)
        {
            porfefeuille_factures MyBillWallet = new porfefeuille_factures()

            {
                id_facture = (short)id_facture,
                id_don = id_don,
                libelle = libelle
            };

            DprojBdd.porfefeuille_factures.Add(MyBillWallet);
            DprojBdd.SaveChanges();

            return MyBillWallet.id_portfeuille;
        }

        public void ReferencerFacture(short id_don, int id_facture, int id_portfeuille, string libelle, decimal montant, string numero)
        {

            int MyBillWallet = CreerPortefeuilleFacture(id_don, id_facture, libelle);

            facture MyBill = DprojBdd.factures.FirstOrDefault(f => f.id_facture == id_facture);
            don MyDonation = DprojBdd.dons.FirstOrDefault(d => d.id_don == id_don);


            /*  
              {
                  id_don = MyDonation.id_don;
                  libelle = libelle;
                  montant = montant,
                  numero = numero,
              };
              DprojBdd.porfefeuille_factures.Add(MyBill.id_facture);*/

            DprojBdd.SaveChanges();

        }

        public facture GetFacture(int id_portefeuille)
        {
            porfefeuille_factures MyWallet = DprojBdd.porfefeuille_factures.FirstOrDefault(p => p.id_portfeuille == id_portefeuille);
            if (MyWallet == null) return null;

            facture MyBill = DprojBdd.factures.FirstOrDefault(p => p.id_facture == MyWallet.id_facture);
            if (MyBill == null) return null;

            return DprojBdd.factures.FirstOrDefault(p => p.id_facture == MyBill.id_facture);

        }

        public porfefeuille_factures GetPortefeuille_Don(int id_don)
        {
            return DprojBdd.porfefeuille_factures.FirstOrDefault(d => d.id_don == id_don);
        }

        public don GetDon(int id_don)
        {
            porfefeuille_factures MyWallet = DprojBdd.porfefeuille_factures.FirstOrDefault(p => p.id_don == id_don);
            if (MyWallet == null) return null;

            don MyDonation = DprojBdd.dons.FirstOrDefault(d => d.id_don == MyWallet.id_don);
            if (MyDonation == null) return null;

            return DprojBdd.dons.FirstOrDefault(p => p.id_don == MyDonation.id_don);

        }

        public don ObtenirDons(short id_don, string email, string prenom, decimal montant, string num_autorisation, System.DateTime date_heure, int id_projet)
        {
            return DprojBdd.dons.FirstOrDefault(u => u.id_don == id_don);
        }

        public utilisateur ObtenirUser(string email)
        {
            return DprojBdd.utilisateurs.FirstOrDefault(u => u.utilisateur_email == email);
        }

        public projet get_Projet(int id)
        {
            projet monprojet = DprojBdd.projets.Find(id);
            return monprojet;
        }

        public int GetId_Media(int id_projet)
        {
            medium monmedia = DprojBdd.media.FirstOrDefault(p => p.id_projet == id_projet && p.lib_image == "principale");
            if (monmedia == null)
            {
                return -1;
            }
            else
            {
                return monmedia.id_media;
            }
        }

        public medium GetPic(int id)
        {
            var picture = DprojBdd.media.Find(id);
            return picture;
        }

        public void ModifierMontantCollecte(int id_projet, decimal montant)
        {
            projet projetTrouve = DprojBdd.projets.Find(id_projet);
            if (projetTrouve != null)
            {
                decimal TotalCollecte = projetTrouve.montant_collecte + montant;
                projetTrouve.montant_collecte = TotalCollecte;

                DprojBdd.SaveChanges();

            }
        }

        public porfefeuille_factures GetPortefeuilleD(int id_portfeuille)
        {
            return DprojBdd.porfefeuille_factures.Find(id_portfeuille);
        }

        public List<don> ObtenirTousLesDons()
        {
            return DprojBdd.dons.ToList();
        }
    }
}