using Projet2_Praya.Models;
using Projet2_Praya.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Projet2_Praya.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // connexion à a DAL 
        private IDalAdmin dal;
        public AdminController() : this(new DalAdmin())
        {

        }

        private AdminController(IDalAdmin dalIoc)
        {
            dal = dalIoc;
        }

        // Menu index Admin
        public ActionResult Index()
        {
            if (!ChkAdmin())
                return View("ErreurNotAdm");

            utilisateur Utilisateur = dal.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            ViewBag.utilisateur = Utilisateur;
            ViewBag.curdate = DateTime.Now;
            return View();
        }

        // liste des utilisateurs
        public ActionResult AfficheUtilisateurs()
        {
            if (!ChkAdmin())
                return View("ErreurNotAdm");


            List<utilisateur> listeUtilisateurs = dal.ObtientTousLesUtilisateurs();
            ViewBag.curdate = DateTime.Now;
            return View(listeUtilisateurs);
        }


        // Ajouter un utilisateur
        [HttpGet]
        public ActionResult AjouterUtilisateur()
        {
            if (!ChkAdmin())
                return View("ErreurNotAdm");

            return View();
        }

        [HttpPost]
        public ActionResult AjouterUtilisateur(utilisateur utilisateur)
        {
            if (String.IsNullOrEmpty(utilisateur.utilisateur_email))
            {
                ModelState.AddModelError("utilisateur_email", "Email non rensigné");
                return View(utilisateur);
            }
            Regex pattern = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$");
            if (!pattern.IsMatch(utilisateur.utilisateur_email))
            {
                ModelState.AddModelError("utilisateur_email", "adresse email invalide");
                return View(utilisateur);
            }

            if (String.IsNullOrEmpty(utilisateur.mot_de_passe))
            {
                ModelState.AddModelError("mot_de_passe", "Mot de passe absent");
                return View(utilisateur);
            }
            if (String.IsNullOrEmpty(utilisateur.nom))
            {
                ModelState.AddModelError("nom", "Nom non renseigné");
                return View(utilisateur);
            }
            if (String.IsNullOrEmpty(utilisateur.prenom))
            {
                ModelState.AddModelError("prenom", "prenom non renseigné");
                return View(utilisateur);
            }
            if (ModelState.IsValid)
            {
                if (dal.AjouterUtilisateur(utilisateur.utilisateur_email, utilisateur.mot_de_passe,
                    utilisateur.nom, utilisateur.prenom))
                {
                    return RedirectToAction("AfficheUtilisateurs");
                    // return Redirect("/");
                }
                ModelState.AddModelError("utilisateur_email", "Compte déjà existant !");
            }
            return View(utilisateur);
        }

        // Modifier un utilisateur
        [HttpGet]
        public ActionResult ModifierUtilisateur(string id)
        {
            if (!ChkAdmin())
                return View("ErreurNotAdm");



            utilisateur user = dal.ObtenirUtilisateur(id);
            if (user == null)
                return View("Erreur");

            return View(user);
        }

        // Modifier un utilisateur
        [HttpPost]
        public ActionResult ModifierUtilisateur(utilisateur user)
        {
            if (String.IsNullOrEmpty(user.mot_de_passe))
            {
                ModelState.AddModelError("mot_de_passe", "Mot de passe absent");
                return View(user);
            }
            if (String.IsNullOrEmpty(user.nom))
            {
                ModelState.AddModelError("nom", "Nom non renseigné");
                return View(user);
            }
            if (String.IsNullOrEmpty(user.prenom))
            {
                ModelState.AddModelError("prenom", "prenom non renseigné");
                return View(user);
            }
            dal.EditUtilisateur(user.utilisateur_email, user.mot_de_passe, user.nom, user.prenom);
            ViewBag.curdate = DateTime.Now;
            return RedirectToAction("AfficheUtilisateurs");
        }

        // liste des types projets
        public ActionResult AfficheTypeProjet()
        {
            if (!ChkAdmin())
                return View("ErreurNotAdm");


            List<type_projet> listeTypesProjet = dal.ObtientTousLesTypesProjets();
            ViewBag.curdate = DateTime.Now;
            return View(listeTypesProjet);
        }


        // Modifier un type de projet
        [HttpGet]
        public ActionResult ModifierTypeProjet(string id)
        {
            if (!ChkAdmin())
                return View("ErreurNotAdm");



            type_projet typeproj = dal.ObtenirTypeProjet(id);
            if (typeproj == null)
                return View("Erreur");

            return View(typeproj);
        }

        // Modifier un type de projet
        [HttpPost]
        public ActionResult ModifierTypeProjet(type_projet typeProj)
        {
            if (String.IsNullOrEmpty(typeProj.libelle))
            {
                ModelState.AddModelError("libelle", "veuillez renseigner le libellé du type de projet");
                return View(typeProj);
            }
            dal.ModifierTypeProjet(typeProj.type, typeProj.libelle);
            ViewBag.curdate = DateTime.Now;
            return RedirectToAction("AfficheTypeProjet");
        }

        // créer type projet
        public ActionResult AjouterTypeProjet()
        {
            if (!ChkAdmin())
                return View("ErreurNotAdm");

            return View();
        }

        [HttpPost]
        public ActionResult AjouterTypeProjet(type_projet typeProj)
        {
            if (String.IsNullOrEmpty(typeProj.type))
            {
                ModelState.AddModelError("type", "veuillez renseigner le type de projet");
                return View(typeProj);
            }
            if (String.IsNullOrEmpty(typeProj.libelle))
            {
                ModelState.AddModelError("libelle", "veuillez renseigner le libellé du type de projet");
                return View(typeProj);
            }
            if (ModelState.IsValid)
            {
                if (dal.CreerTypeProjet(typeProj.type, typeProj.libelle))
                {
                    return RedirectToAction("AfficheTypeProjet");
                    // return Redirect("/");
                }
                ModelState.AddModelError("type", "Type de projet déjà existant !");
            }
            return View(typeProj);
        }

        //  Afficher la Liste des Projets pour l'Admin , get
        public ActionResult AfficheAdminListProjets()
        {
            ViewBag.curdate = DateTime.Now;
            if (!ChkAdmin())
                return View("ErreurNotAdm");

            AdmVueProjets mavue = lstprojetCrit("*");
            return View(mavue);
        }


        //  Afficher la Liste des Projets pour l'Admin , post
        [HttpPost]
        public ActionResult AfficheAdminListProjets(AdmVueProjets myCrit)
        {
            ViewBag.curdate = DateTime.Now;

            if (!ChkAdmin())
                return View("ErreurNotAdm");
            
            AdmVueProjets mavue = lstprojetCrit(myCrit.EtatProj);

            return View(mavue);
        }

        // check user connecté et reconnu comme admin
        private bool ChkAdmin()
        {
            administrateur Admin = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Admin = dal.ObtenirAdmin(HttpContext.User.Identity.Name);
            }
            if (Admin == null)
            {
                return false;
            }
            return true;
        }

        // ramene une liste de projets selon critère
        private AdmVueProjets lstprojetCrit(String Crit)
        {
            List<projet> listAdmProj = dal.ObtenirTousLesProjets();
            List<DetailsInformationProjet> ListeProj = new List<DetailsInformationProjet>();

            foreach (projet Projet in listAdmProj)
            {
                if (Crit != null)
                {
                    if (Crit == "*")
                    {
                        if (Projet.etat_projet == "D")
                            continue;
                    }
                    else
                    {
                        if (Projet.etat_projet != Crit)
                            continue;
                    }
                }
                string LibEtat = "";
                if (Projet.etat_projet == "C")
                {
                    LibEtat = "Créé";
                }
                else if (Projet.etat_projet == "W")
                {
                    LibEtat = "Attente";
                }
                else if (Projet.etat_projet == "R")
                {
                    LibEtat = "Rejetté";
                }
                else if (Projet.etat_projet == "A")
                {
                    LibEtat = "Approuvé";
                }
                else if (Projet.etat_projet == "P")
                {
                    LibEtat = "Publié";
                }
                else if (Projet.etat_projet == "D")
                {
                    LibEtat = "Supprimé";
                }
                else
                {
                    LibEtat = "Etat_" + Projet.etat_projet;
                }


                portefeuille_projet MyWallet = dal.GetPortefeuille(Projet.id_portefeuille);
                string userProj = "???";
                if (MyWallet != null)
                    userProj = MyWallet.resp_projet;

                ListeProj.Add(new DetailsInformationProjet
                {
                    id_projet = Projet.id_projet,
                    libelle = Projet.libelle,
                    description = Projet.description,
                    type_du_projet = Projet.type_du_projet,
                    lib_etat = LibEtat,
                    montant_attendu = Projet.montant_attendu,
                    montant_collecte = Projet.montant_collecte,
                    date_maj = Projet.date_maj,
                    date_butoir = Projet.date_butoir,
                    adm_email = userProj,
                    date_debut = Projet.date_debut

                });
            }

            SelectList maListe = new SelectList(
                new List<SelectListItem>
                {
                        new SelectListItem {Text = "Actif", Value = "*"},
                        new SelectListItem {Text = "Créé", Value = "C"},
                        new SelectListItem {Text = "Attente", Value = "W"},
                        new SelectListItem {Text = "Publié", Value = "P"},
                        new SelectListItem {Text = "Rejeté", Value = "R"},
                        new SelectListItem {Text = "Supprimé", Value = "D"},
                }, "Value", "Text");

            ViewBag.LibEtats = maListe;

            AdmVueProjets mavue = new AdmVueProjets()
            {
                EtatProj = Crit,
                lstprojets = ListeProj
            };

            return mavue;
        }
        
    }
}