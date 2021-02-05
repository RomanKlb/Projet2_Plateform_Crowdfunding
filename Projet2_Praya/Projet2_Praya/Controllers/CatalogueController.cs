using PagedList;
using Projet2_Praya.Models;
using Projet2_Praya.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Projet2_Praya.Controllers
{
    [AllowAnonymous]
    public class CatalogueController : Controller
    {
        DalCreationProjet dal = new DalCreationProjet();

        // GET: Catalogue

        [AllowAnonymous]
        public ActionResult Index(int page = 1, int pageSize = 6)
        {

            List<projet> listeProjets = dal.ObtenirTousLesProjets();

            List<DisplayInformationCatalogue> ListeDIC = new List<DisplayInformationCatalogue>();
            //int nbItem = 0;
          

            foreach (projet monProjet in listeProjets)
            {
                medium monMedia = dal.GetMedia(monProjet.id_projet);
                if (monMedia == null)
                    continue;

                if (monProjet.etat_projet != "P")
                {
                    continue;
                }

                ListeDIC.Add(new DisplayInformationCatalogue
                {
                    Id_projet = monProjet.id_projet,
                    Libelle = monProjet.libelle,
                    Id_media = monMedia.id_media,
                    Lib_image = monMedia.lib_image

                    
                });

            }

            int itemsNumbers = ListeDIC.Count();

            PagedList<DisplayInformationCatalogue> model = new PagedList<DisplayInformationCatalogue>(ListeDIC, page, pageSize);
            return View(model);
        }

        // GET: Vue Projet
        public ActionResult VueProjet(int? id, string act = "don")
        {
            if (id.HasValue)
            {

                // recuperation infos sur projet
                projet monprojet = dal.get_Projet(id.Value);
                if (monprojet == null)
                    return View("Erreur");

                if (monprojet.etat_projet == "D" && act != "admf")
                    return View("Erreur");

                // ViewBag.monprojet = monprojet;
                // recuperation association & type de projet
                ViewBag.asso = dal.GetAssociation(monprojet.id_portefeuille);
                ViewBag.type = dal.GetTypeProjet(monprojet.type_du_projet);

                // Recuperation infos sur Medias principal
                ViewBag.monmedia = dal.GetId_Media(id.Value);

                // recuperation des autres images projets 
                ViewBag.image2 = dal.GetId_Media_all(id.Value, "image_2");
                ViewBag.image3 = dal.GetId_Media_all(id.Value, "image_3");
                ViewBag.image4 = dal.GetId_Media_all(id.Value, "image_4");

                ViewBag.typact = act;

                string LibEtat = "Etat_" + monprojet.etat_projet;
                if (monprojet.etat_projet == "C")
                {
                    LibEtat = "Créé";
                }
                else if (monprojet.etat_projet == "W")
                {
                    LibEtat = "Attente Publication";
                }
                else if (monprojet.etat_projet == "R")
                {
                    LibEtat = "Rejetté";
                }
                else if (monprojet.etat_projet == "A")
                {
                    LibEtat = "Approuvé";
                }
                else if (monprojet.etat_projet == "P")
                {
                    LibEtat = "Publié";
                }
                else if (monprojet.etat_projet == "D")
                {
                    LibEtat = "Supprimé";
                }
                else
                {
                    LibEtat = "Etat_" + monprojet.etat_projet;
                }
                ViewBag.Libetat = LibEtat;

                return View(monprojet);
            }
            else
            {
                return View("Erreur");
            }

        }

        //Get : Search
        public ActionResult SearchProj()
        {
            get_type_projets();
            return View();
        }

        //Send Search Query
        [HttpPost]
        public ActionResult LibSearchProj(ProjetPageListViewModel searchString, int page = 1, int pageSize = 6)
        {
            get_type_projets();
            List<projet> listeProjets = dal.ObtenirTousLesProjets();
            List<DisplayInformationCatalogue> ListeDIC = new List<DisplayInformationCatalogue>();

            if (!string.IsNullOrEmpty(searchString.CritRecherche.libelle))
            {
                List<projet> SearchedP = (from s in listeProjets
                                          where s.libelle.ToUpper().Contains(searchString.CritRecherche.libelle.ToUpper())
                                          select s).ToList();


                foreach (projet ProjetRech in SearchedP)
                {
                    if (ProjetRech.etat_projet != "P")
                        continue;

                    medium monmedia = dal.GetMedia(ProjetRech.id_projet);
                    if (monmedia == null)
                        continue;


                    ListeDIC.Add(new DisplayInformationCatalogue
                    {
                        Id_projet = ProjetRech.id_projet,
                        Libelle = ProjetRech.libelle,
                        Description = ProjetRech.description,
                        Id_media = monmedia.id_media,
                        Lib_image = monmedia.lib_image

                    });


                }

                int itemsSNumbers = SearchedP.Count();
                PagedList<DisplayInformationCatalogue> Searchmodel = new PagedList<DisplayInformationCatalogue>(ListeDIC, page, pageSize);
                ViewBag.test = Searchmodel;

                ProjetPageListViewModel VM = new ProjetPageListViewModel()
                {
                    CritRecherche = searchString.CritRecherche,
                    SearchModel = Searchmodel
                };
                return View(VM);
            }


            return RedirectToAction("Index");

        }


        [HttpPost]
        public ActionResult SearchProj(ProjetPageListViewModel searchName, int page = 1, int pageSize = 6)
        {
            get_type_projets();
            List<projet> listeProjets = dal.ObtenirTousLesProjets();
            List<DisplayInformationCatalogue> ListeDIC = new List<DisplayInformationCatalogue>();

            List<projet> SearchedP = new List<projet>();
            if (!String.IsNullOrEmpty(searchName.CritRecherche.libelle))
            {
                SearchedP = (from s in listeProjets
                             where s.libelle.ToUpper().Contains(searchName.CritRecherche.libelle.ToUpper())
                             select s).ToList();

            }
            else
            {
                SearchedP = listeProjets;
            }


            foreach (projet CurrProj in SearchedP)
            {


                if (CurrProj.etat_projet != "P")
                    continue;


                if (searchName.CritRecherche.type_du_projet != null)
                {

                    if (CurrProj.type_du_projet != searchName.CritRecherche.type_du_projet)
                        continue;
                }

                if (!String.IsNullOrEmpty(searchName.CritRecherche.nom_association))
                {
                    association monAsso = dal.GetAssociation(CurrProj.id_portefeuille);
                    if (monAsso != null)
                    {
                        if (monAsso.nom_association != searchName.CritRecherche.nom_association)
                            continue;
                    }
                }

                if (searchName.CritRecherche.date_butoirMin != DateTime.Parse("0001-01-01"))
                {

                    if (CurrProj.date_butoir < searchName.CritRecherche.date_butoirMin)
                        continue;
                }

                if (searchName.CritRecherche.date_butoirMax != DateTime.Parse("0001-01-01"))
                {

                    if (CurrProj.date_butoir > searchName.CritRecherche.date_butoirMax)
                        continue;
                }

                if (searchName.CritRecherche.montant_attenduMin != 0)
                {
                    if (CurrProj.montant_attendu < searchName.CritRecherche.montant_attenduMin)
                        continue;
                }

                if (searchName.CritRecherche.montant_attenduMax != 0)
                {
                    if (CurrProj.montant_attendu > searchName.CritRecherche.montant_attenduMax)
                        continue;
                }

                medium monmedia = dal.GetMedia(CurrProj.id_projet);
                if (monmedia == null)
                    continue;


                ListeDIC.Add(new DisplayInformationCatalogue
                {
                    Id_projet = CurrProj.id_projet,
                    Libelle = CurrProj.libelle,
                    Description = CurrProj.description,
                    Id_media = monmedia.id_media,
                    Lib_image = monmedia.lib_image


                });
            }



            int itemsSNumbers = ListeDIC.Count();
            PagedList<DisplayInformationCatalogue> Searchmodel = new PagedList<DisplayInformationCatalogue>(ListeDIC, page, pageSize);
            //   ViewBag.test = Searchmodel;

            ProjetPageListViewModel VM = new ProjetPageListViewModel()
            {
                CritRecherche = searchName.CritRecherche,
                SearchModel = Searchmodel
            };
            return View(VM);



            return RedirectToAction("Index");

        }




        public ActionResult getMedia(int id)
        {

            var picture = dal.GetPic(id);
            return File(picture.media, "image/jpg");

        }


        private void get_type_projets()
        {
            List<type_projet> TypesProjet = dal.ObtientTypeProjet();
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