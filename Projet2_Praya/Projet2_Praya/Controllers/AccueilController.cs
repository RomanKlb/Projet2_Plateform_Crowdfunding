using Projet2_Praya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet2_Praya.Controllers
{
    public class AccueilController : Controller
    {
        // connexion à a DAL 
        private IDalAdmin dal;
        public AccueilController() : this(new DalAdmin())
        {

        }

        private AccueilController(IDalAdmin dalIoc)
        {
            dal = dalIoc;
        }

        // GET: Index
        public ActionResult Index()
        {
            utilisateur Utilisateur = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Utilisateur = dal.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            }

            ViewBag.curdate = DateTime.Now;
            ViewBag.utilisateur = Utilisateur;
            if (ChkAdmin())
                ViewBag.administrateur = 1;
            else
                ViewBag.administrateur = -1;

            if (ChkProjet())
                ViewBag.resp_projet = 1;
            else
                ViewBag.resp_projet = 0;

            if (ChkDon())
                ViewBag.donateur = 1;
            else
                ViewBag.donateur = 0;

            return View();
        }

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

        private bool ChkProjet()
        {
            resp_projet ResponsableProjet = null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ResponsableProjet = dal.GetResponsableProjet(HttpContext.User.Identity.Name);
            }
            if (ResponsableProjet == null)
            {
                return false;
            }
            return true;
        }

        private bool ChkDon()
        {
            don monDon = null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                monDon = dal.GetDon(HttpContext.User.Identity.Name);
            }
            if (monDon == null)
            {
                return false;
            }
            return true;
        }

        // GET: CommentCaMarche
        public ActionResult GestionProjet()
        {
            return View();
        }
        public ActionResult Don()
        {
            return View();
        }
        public ActionResult QuiSommesNous()
        {
            return View();
        }

    }
} 