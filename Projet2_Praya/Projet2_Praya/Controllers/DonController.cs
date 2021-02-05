using Projet2_Praya.Models;
using Projet2_Praya.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet2_Praya.Controllers
{
    public class DonController : Controller
    {

        private IDalDonProjet dalDProj;

        public IEnumerable<object> Controls { get; private set; }

        public DonController() : this(new DalDonProjet())
        {

        }

        public DonController(DalDonProjet dalCreate)
        {
            dalDProj = dalCreate;
        }



        // GET: Don
        public ActionResult Index(int? id)

        {
            if (id.HasValue)
            {
                projet monprojet = dalDProj.get_Projet(id.Value);
                if (monprojet == null)
                    return View("Erreur");
                ViewBag.monprojet = monprojet;

                ViewBag.monmedia = dalDProj.GetId_Media(id.Value);

                don MyDonation = null;

                if (CheckUser())
                {


                    utilisateur Utilisateur = dalDProj.ObtenirUser(HttpContext.User.Identity.Name);
                    ViewBag.utilisateur = Utilisateur;
                    if (Utilisateur == null)
                        return View("Erreur");

                    MyDonation = new don()
                    {
                        id_don = 0,
                        email = Utilisateur.utilisateur_email,
                        nom = Utilisateur.nom,
                        prenom = Utilisateur.prenom,
                        montant = 0,
                        id_projet = id.Value,

                    };
                }
                else
                {
                    MyDonation = new don()
                    {
                        id_don = 0,
                        email = null,
                        nom = null,
                        prenom = null,
                        montant = 0,
                        id_projet = id.Value,
                    };
                }

                return View(MyDonation);
            }
            else
                return View("Erreur");
        }

        [HttpPost]

        public ActionResult Index(don don)
        {


            if (ModelState.IsValid)
            {
                int MyAuthNumber = RandomNumber(1000, 1000000);
                int IdDon = dalDProj.CreerDon(don.email, don.prenom, don.nom, don.montant, MyAuthNumber.ToString(), don.id_projet);
                dalDProj.ModifierMontantCollecte(don.id_projet, don.montant);
            }

            return RedirectToAction("Index", "Catalogue");
        }

        private readonly Random _random = new Random();
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        private bool CheckUser()
        {
            utilisateur User = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User = dalDProj.ObtenirUser(HttpContext.User.Identity.Name);
            }
            if (User == null)
            {
                return false;
            }
            return true;
        }

        [Authorize]
        public ActionResult AfficheDonsDonateur()
        {

            List<don> listDonsDonateur = dalDProj.ObtenirTousLesDons();
            List<AfficherDonViewModel> ListeDID = new List<AfficherDonViewModel>();

            foreach (don Dons in listDonsDonateur)
            {
                /* porfefeuille_factures MyWallet = dalDProj.GetPortefeuille_Don(Dons.id_don);
                 if (MyWallet == null)
                     continue;*/

                if (Dons.email != HttpContext.User.Identity.Name)
                    continue;

                ListeDID.Add(new AfficherDonViewModel
                {
                    id_don = Dons.id_don,
                    libelle = Dons.projet.libelle,
                    email = Dons.email,
                    montant = Dons.montant,
                    date_facture = Dons.date_heure

                });
            }
            return View(ListeDID);
        }


        public ActionResult getMedia(int id)
        {

            var picture = dalDProj.GetPic(id);
            return File(picture.media, "image/jpg");

        }
    }
}