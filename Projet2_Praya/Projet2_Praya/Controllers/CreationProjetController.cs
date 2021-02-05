using Projet2_Praya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Projet2_Praya.Controllers
{
    [Authorize]
    public class CreationProjetController : Controller
    {
        private IDalCreationProjet dalCProj;

        public IEnumerable<object> Controls { get; private set; }

        public CreationProjetController() : this(new DalCreationProjet())
        {

        }

        public CreationProjetController(DalCreationProjet dalCreate)
        {
            dalCProj = dalCreate;
        }


        // GET: CreationAsso
        public ActionResult Index()
        {
            association ThisAsso = dalCProj.ObtenirRespAsso(HttpContext.User.Identity.Name);
            if (ThisAsso != null)
            {
                return RedirectToAction("ModifierAssociation", new { id = ThisAsso.id_association });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(association asso)
        {
            if (String.IsNullOrEmpty(asso.nom_association))
            {
                ModelState.AddModelError("nom_association", "Nom non renseigné");
                return View(asso);
            }

            if (dalCProj.NomAssoExiste(asso.nom_association))
            {
                ModelState.AddModelError("Nom", "Ce nom d'association existe déjà");
                return View(asso);
            }

            if (string.IsNullOrEmpty(asso.RNA))
            {
                ModelState.AddModelError("RNA", "Le numéro RNA ne peut pas être nul !");
                return View(asso);
            }
            if (asso.RNA.Length > 10)
            {
                ModelState.AddModelError("RNA", "Le numéro RNA ne peut dépasser les 10 caractères !");
                return View(asso);
            }
            if (asso.RNA.Length != 10)
            {
                ModelState.AddModelError("RNA", "Le numéro RNA doit être constitué de 10 caractères !");
                return View(asso);
            }


            if (dalCProj.RNAAssoExiste(asso.RNA))
            {
                ModelState.AddModelError("RNA", "Ce RNA d'association existe déjà");
                return View(asso);
            }

            if (string.IsNullOrEmpty(asso.IBAN))
            {
                ModelState.AddModelError("IBAN", "L'IBAN ne peut pas être nul !");
                return View(asso);
            }
            if (asso.IBAN.Length > 34 || asso.IBAN.Length < 16)
            {
                ModelState.AddModelError("IBAN", "Le numéro IBAN ne peut pas être inferieur à 16 ou dépasser 34 caractères");
                return View(asso);
            }

            if (ModelState.IsValid)
            {
                int IdAssos = dalCProj.CreerAssociation(asso.nom_association, asso.RNA, asso.IBAN);


                dalCProj.TestCreatePortefeuille(HttpContext.User.Identity.Name, "Defaut", "1", (short)IdAssos);
            }

            return RedirectToAction("CreationProjet");
        }


        public ActionResult ModifierAssociation(int? id)
        {
            if (id.HasValue)
            {
                association MyAsso = dalCProj.ObtenirAsso((int)id);
                if (MyAsso == null)
                {
                    return View("Erreur");
                }
                return View(MyAsso);
            }
            return View("Erreur");
        }


        [HttpPost]
        public ActionResult ModifierAssociation(association MyAsso)
        {
            if (String.IsNullOrEmpty(MyAsso.nom_association))
            {
                ModelState.AddModelError("nom_association", "Nom non renseigné");
                return View(MyAsso);
            }

            if (String.IsNullOrEmpty(MyAsso.RNA))
            {
                ModelState.AddModelError("RNA", "RNA non renseigné");
                return View(MyAsso);
            }

            if (String.IsNullOrEmpty(MyAsso.IBAN))
            {
                ModelState.AddModelError("IBAN", "IBAN non renseigné");
                return View(MyAsso);
            }
            dalCProj.ModifierAsso(MyAsso.id_association, MyAsso.nom_association, MyAsso.RNA, MyAsso.IBAN);

            dalCProj.TestCreatePortefeuille(HttpContext.User.Identity.Name, "Defaut", "1", MyAsso.id_association);

            return RedirectToAction("CreationProjet");
        }


        // GET: CreationProjet
        public ActionResult CreationProjet()
        {
            get_type_projets();

            //Fin de la création

            return View();
        }

        [HttpPost]
        public ActionResult CreationProjet(projet projet)
        {
            get_type_projets();

            if (String.IsNullOrEmpty(projet.libelle))
            {
                ModelState.AddModelError("libelle", "Le Libellé n'est pas renseigné !");
                return View(projet);
            }


            if (dalCProj.ProjetExiste(projet.libelle))
            {
                ModelState.AddModelError("libelle", "Ce projet existe déjà ! Veuillez modifier le titre de votre projet");
                return View(projet);
            }
            if (projet.type_du_projet == null)
            {
                ModelState.AddModelError("type_du_projet", "Séléctionnez un type proposé");
                return View(projet);
            }

            if (projet.montant_attendu <= 100)
            {
                ModelState.AddModelError("montant_attendu", "Le montant attendu doit etre superieur à 100 !");
                return View(projet);
            }

            if (String.IsNullOrEmpty(projet.description))
            {
                ModelState.AddModelError("description", "La description n'est pas renseignée !");
                return View(projet);
            }
            //   return View(projet);
            if (!ModelState.IsValid)
                return View(projet);

            int projid = dalCProj.CreerProjet(projet.libelle, projet.description, projet.type_du_projet,
                  projet.montant_attendu, (DateTime)projet.date_debut, (DateTime)projet.date_butoir, HttpContext.User.Identity.Name);
            return RedirectToAction("Index", "Media", new { id = projid });
        }


        // Modification etat d'un projet
        public ActionResult ModifierStatProjet(int? id, string stat = "P", string orig = "admf")
        {
            if (id.HasValue)
            {
                projet projet = dalCProj.get_Projet(id.Value);
                if (projet == null)
                    return View("Erreur");

                if (projet.etat_projet == stat)
                {
                    return View("Erreur");
                }
                dalCProj.ModifierProjetStat(id.Value, stat);
                if (orig == "admf")
                {
                    return RedirectToAction("AfficheAdminListProjets", "Admin");
                }
                else if (orig == "admu")
                {
                    return RedirectToAction("AfficheUserListProjets", "CreationProjet");
                }

                return RedirectToAction("Index", "Accueil"); ;
            }
            else
                return View("Erreur");
        }

        //Get: ModifierProjet
        public ActionResult ModifierProjet(int? id)
        {
            get_type_projets();
            if (id.HasValue)
            {
                projet projet = dalCProj.ObtenirTousLesProjets().FirstOrDefault(p => p.id_projet == id.Value);
                if (projet == null)
                    return View("Erreur");
                return View(projet);
            }
            else
                return View("Erreur");
        }

        [HttpPost]
        public ActionResult ModifierProjet(projet projet)
        {
            get_type_projets();


            if (String.IsNullOrEmpty(projet.libelle))
            {
                ModelState.AddModelError("libelle", "Le Libellé n'est pas renseigné !");
                return View(projet);
            }

            if (projet.type_du_projet == null)
            {
                ModelState.AddModelError("type_du_projet", "Séléctionnez un type proposé");
                return View(projet);
            }

            if (projet.montant_attendu <= 100)
            {
                ModelState.AddModelError("montant_attendu", "Le montant attendu doit etre superieur à 100 !");
                return View(projet);
            }

            if (String.IsNullOrEmpty(projet.description))
            {
                ModelState.AddModelError("description", "La description n'est pas renseignée !");
                return View(projet);
            }

            if (!ModelState.IsValid)
                return View(projet);
            dalCProj.ModifierProjet(projet.id_projet, projet.libelle, projet.description, projet.type_du_projet, projet.montant_attendu, projet.date_debut, projet.date_butoir);
            return RedirectToAction("Index", "Media", new { id = projet.id_projet });
        }



        //Get : Afficher la Liste des Projets pour L'utilisateur courant 
        public ActionResult AfficheUserListProjets()
        {

            List<projet> listUserProj = dalCProj.ObtenirTousLesProjets();
            List<DisplayInformationProjet> ListeDIP = new List<DisplayInformationProjet>();

            foreach (projet Projet in listUserProj)
            {

                string LibEtat = "Etat_" + Projet.etat_projet;
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
                    continue;
                }


                portefeuille_projet MyWallet = dalCProj.GetPortefeuille(Projet.id_portefeuille);
                if (MyWallet == null)
                    continue;
                if (MyWallet.resp_projet != HttpContext.User.Identity.Name)
                    continue;

                ListeDIP.Add(new DisplayInformationProjet
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
                    lib_portefeuille = MyWallet.libelle,
                    date_debut = Projet.date_debut

                });
            }
            return View(ListeDIP);
        }



        void get_type_projets()
        {
            List<type_projet> TypesProjet = dalCProj.ObtientTypeProjet();
            List<SelectListItem> TypesSelect = new List<SelectListItem>();
            foreach (type_projet IdType in TypesProjet)
            {
                TypesSelect.Add(new SelectListItem { Text = IdType.type, Value = IdType.type });
            }

            // SelectList SlTypeP = new SelectList(TypesProjet, "Value", "Text");
            ViewBag.TypesProjet = TypesSelect;
        }
    }
}