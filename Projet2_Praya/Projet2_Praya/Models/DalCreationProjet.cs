using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Projet2_Praya.Models
{
    public class DalCreationProjet : IDalCreationProjet
    {
        private projet2Entities CprojBdd;

        public DalCreationProjet()
        {
            CprojBdd = new projet2Entities();
        }


        public List<projet> ObtenirTousLesProjets()
        {
            return CprojBdd.projets.ToList();
        }


        public bool ProjetExiste(string description)
        {
            projet myproj = CprojBdd.projets.FirstOrDefault(p => p.libelle == description);
            if (myproj == null)
            {
                return false;
            }
            return true;
        }

        public bool TestCreateAssociation(List<association> listeAsso)
        {
            association AssociationTrouve = CprojBdd.associations.FirstOrDefault(association => association.id_association == association.id_association);

            resp_projet IdAssoTrouve = CprojBdd.resp_projet.FirstOrDefault(id_association => id_association.id_association == id_association.id_association);

            if (IdAssoTrouve != null)
            {
                return true;
            }
            return false;
        }


        public association ObtenirAssociation(short id_association, string nom_association, string RNA, string IBAN)
        {
            return CprojBdd.associations.FirstOrDefault(u => u.id_association == id_association);
        }

        public void TestCreatePortefeuille(string resp_projet, string libelle, string niveau_habilitation, short Id_association)
        {

            resp_projet testResp = TestCreateResponsableP(resp_projet);

            if (testResp == null)
                CreerRespProj(resp_projet, niveau_habilitation, Id_association);

            portefeuille_projet portefeuilleTrouve = CprojBdd.portefeuille_projet.FirstOrDefault
                (portefeuille => portefeuille.libelle == libelle && portefeuille.resp_projet == resp_projet);

            if (portefeuilleTrouve == null)
            {
                CreerPortefeuilleProj(resp_projet, libelle);
            }
            /*if (portefeuilleTrouve != null)

                ComboxPortefeuille.SelectedIndex = ComboxPortefeuille.FindStringExact("Portefeuille Ecotourisme");
            return ComboxPortefeuille.SelectedIndex;*/
        }

        public int CreerAssociation(string nom_association, string RNA, string IBAN)
        {
            association Asso = new association()
            {
                nom_association = nom_association,
                RNA = RNA,
                IBAN = IBAN

            };

            CprojBdd.associations.Add(Asso);
            CprojBdd.SaveChanges();

            return Asso.id_association;

        }

        public int AddMedia(Byte[] media, string lien, int id_projet, string type_media, string lib_image)
        {
            medium MyMedia = new medium()
            {
                media = media,
                lien = lien,
                id_projet = id_projet,
                type_media = type_media,
                lib_image = lib_image
            };

            CprojBdd.media.Add(MyMedia);

            CprojBdd.SaveChanges();

            return MyMedia.id_media;

        }

        public int DeleteMedia(int id)

        {
            medium media = CprojBdd.media.Find(id);
            if (media != null)
            {
                int id_proj = media.id_projet;
                CprojBdd.media.Remove(media);
                CprojBdd.SaveChanges();
                return id_proj;
            }
            else
            {
                return -1;
            }
        }

        public medium GetPic(int id)
        {
            var picture = CprojBdd.media.Find(id);
            return picture;
        }
        public int GetId_Media(int id_projet)
        {
            medium monmedia = CprojBdd.media.FirstOrDefault(p => p.id_projet == id_projet && p.lib_image == "principale");
            if (monmedia == null)
            {
                return -1;
            }
            else
            {
                return monmedia.id_media;
            }
        }

        public medium GetMedia(int id_projet)
        {
            return CprojBdd.media.FirstOrDefault(p => p.id_projet == id_projet && p.lib_image == "principale");
        }

        public int GetId_Media_all(int id_projet, string lib_image)
        {
            medium monmedia = CprojBdd.media.FirstOrDefault(
                p => p.id_projet == id_projet && p.lib_image == lib_image);
            if (monmedia == null)
            {
                return -1;
            }
            else
            {
                return monmedia.id_media;
            }
        }

        public void SaveFirstForm(string RNA, string IBAN, string nom_association)
        {
            association Asso = new association()
            {
                RNA = RNA,
                IBAN = IBAN,
                nom_association = nom_association
            };

            CprojBdd.associations.Add(Asso);
            CprojBdd.SaveChanges();
        }

        public bool NomAssoExiste(string nom)
        {
            return CprojBdd.associations.Any(asso => string.Compare(asso.nom_association, nom, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public bool RNAAssoExiste(string RNA)
        {
            return CprojBdd.associations.Any(asso => string.Compare(asso.RNA, RNA, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public projet get_Projet(int id)

        {
            projet monprojet = CprojBdd.projets.Find(id);
            return monprojet;
        }
        public int CreerProjet(string libelle, string description, string type_du_projet, decimal montant_attendu, System.DateTime date_debut, System.DateTime date_butoir, string resp_projet)
        {
            portefeuille_projet PortefeuilleP = CprojBdd.portefeuille_projet.FirstOrDefault
                (P => P.resp_projet == resp_projet && P.libelle == "Defaut");

            projet myprojet = new projet()
            {
                libelle = libelle,
                description = description,
                type_du_projet = type_du_projet,
                montant_attendu = montant_attendu,
                montant_collecte = 0.0M,
                date_debut = date_debut,
                etat_projet = "C",
                id_portefeuille = PortefeuilleP.id_portefeuille,
                date_butoir = date_butoir
            };
            CprojBdd.projets.Add(myprojet);

            CprojBdd.SaveChanges();

            return myprojet.id_projet;

        }

        public void AjouterMedia(byte[] media, string lien, int id_projet, string type_media, string lib_image)
        {
            CprojBdd.media.Add(new medium { media = media, lien = lien, id_projet = id_projet, type_media = type_media, lib_image = lib_image });
            CprojBdd.SaveChanges();
        }

        public int CreerPortefeuilleProj(string resp_proj, string libelle)
        {
            portefeuille_projet portefeuille = new portefeuille_projet()
            {
                resp_projet = resp_proj,
                libelle = libelle,
            };

            CprojBdd.portefeuille_projet.Add(portefeuille);
            CprojBdd.SaveChanges();

            return portefeuille.id_portefeuille;
        }

        public resp_projet TestCreateResponsableP(string resp_projet)
        {

            resp_projet ResponsableTrouve = CprojBdd.resp_projet.FirstOrDefault(r => r.resp_email == resp_projet);

            return ResponsableTrouve;
        }

        public string CreerRespProj(string resp_email, string niveau_habilitation, short Id_association)
        {

            resp_projet ResponsableProjet = new resp_projet()
            {
                resp_email = resp_email,
                niveau_habilitation = niveau_habilitation,
                id_association = Id_association
            };

            CprojBdd.resp_projet.Add(ResponsableProjet);
            CprojBdd.SaveChanges();


            return ResponsableProjet.resp_email;
        }

        public void ModifierProjetStat(int id_projet, string newstat)
        {
            projet projetTrouve = CprojBdd.projets.Find(id_projet);
            if (projetTrouve != null)
            {
                newstat = newstat.Substring(0, 1);
                if (projetTrouve.etat_projet != newstat)
                {
                    projetTrouve.etat_projet = newstat;

                    CprojBdd.SaveChanges();
                }
            }
        }

    public void ModifierProjet(int id_projet, string libelle, string description, string type_du_projet, decimal montant_attendu, System.DateTime date_debut, System.DateTime date_butoir)
        {
            projet projetTrouve = CprojBdd.projets.FirstOrDefault(projet => projet.id_projet == id_projet);
            if (projetTrouve != null)
            {
                projetTrouve.libelle = libelle;
                projetTrouve.description = description;
                projetTrouve.type_du_projet = type_du_projet;
                projetTrouve.montant_attendu = montant_attendu;
                projetTrouve.date_debut = date_debut;
                projetTrouve.date_butoir = date_butoir;

                CprojBdd.SaveChanges();

            }
        }


        public void Dispose()
        {
            CprojBdd.Dispose();
        }

        public List<type_projet> ObtientTypeProjet()
        {
            return CprojBdd.type_projet.ToList();
        }


        public type_projet GetTypeProjet(string type)
        {
            return CprojBdd.type_projet.FirstOrDefault(t => t.type == type);
        }

        public association ObtenirAsso(int id_asso)
        {
            return CprojBdd.associations.FirstOrDefault(i => i.id_association == id_asso);

        }

        public association ObtenirRespAsso(string email)
        {
            resp_projet MyResp = CprojBdd.resp_projet.FirstOrDefault(i => i.resp_email == email);
            if (MyResp == null)
            {
                return null;
            }
            association MyAsso = CprojBdd.associations.FirstOrDefault(i => i.id_association == MyResp.id_association);
            return MyAsso;
        }

        public void ModifierAsso(int id_association, string nom_association, string RNA, string IBAN)
        {
            association AssoTrouve = CprojBdd.associations.Find(id_association);
            if (AssoTrouve != null)
            {
                AssoTrouve.nom_association = nom_association;
                AssoTrouve.RNA = RNA;
                AssoTrouve.IBAN = IBAN;
                CprojBdd.SaveChanges();

            }
        }

        public portefeuille_projet GetPortefeuille(int id_portefeuille)
        {
            return CprojBdd.portefeuille_projet.Find(id_portefeuille);

        }

        public association GetAssociation(int id_portefeuille)
        {
            portefeuille_projet MonPortfeuille = CprojBdd.portefeuille_projet.FirstOrDefault(p => p.id_portefeuille == id_portefeuille);
            if (MonPortfeuille == null) return null;
            resp_projet MyResp = CprojBdd.resp_projet.FirstOrDefault(p => p.resp_email == MonPortfeuille.resp_projet);
            if (MyResp == null) return null;
            return CprojBdd.associations.FirstOrDefault(a => a.id_association == MyResp.id_association);
        }

    }
}