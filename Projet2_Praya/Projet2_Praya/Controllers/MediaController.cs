using Projet2_Praya.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet2_Praya.Controllers
{
    [Authorize]
    public class MediaController : Controller
    {
        private IDalCreationProjet dalCProj;


        public IEnumerable<object> Controls { get; private set; }

        public MediaController() : this(new DalCreationProjet())
        {

        }

        public MediaController(DalCreationProjet dalCreate)
        {
            dalCProj = dalCreate;
        }


        // GET: Media
        public ActionResult Index(int? id, string err)
        {
            if (id.HasValue)
            {

                // recuperation infos sur projer
                projet monprojet = dalCProj.get_Projet(id.Value);
                if (monprojet == null)
                    return View("Erreur");

                ViewBag.monprojet = monprojet;

                // Recuperation infos sur Medias principal
                ViewBag.monmedia = dalCProj.GetId_Media(id.Value);

                // recuperation des autres images projets 
                ViewBag.image2 = dalCProj.GetId_Media_all(id.Value, "image_2");
                ViewBag.image3 = dalCProj.GetId_Media_all(id.Value, "image_3");
                ViewBag.image4 = dalCProj.GetId_Media_all(id.Value, "image_4");

                // message d'erreur
                string libsel = "principale";
                string msgLibel = "";
                string msgfile = "";
                if (err != null)
                {
                    if (err == "err1")
                    {
                        msgLibel = "Séléctionnez un libelle valide";
                        libsel = null;
                    }
                    else if (err.Substring(0, 4) == "errl")
                    {
                        string zonerr = err.Substring(4);

                        msgLibel = "Il y a déjà une image pour le libellé " + zonerr;
                        libsel = zonerr;
                    }
                    else if (err == "errf")
                    {
                        msgfile = "Pas de fichier selectionné pour le téléchargement";
                    }
                    else if (err == "errfl")
                    {
                        msgfile = "fichier selectionné trop volumineux";
                    }
                }
                ViewBag.errlibel = msgLibel;
                ViewBag.errfile = msgfile;

                SelectList maListe = new SelectList(
                     new List<SelectListItem>
                     {
                        new SelectListItem {Text = "Principale", Value = "Principale"},
                        new SelectListItem {Text = "Image 2", Value = "Image_2"},
                        new SelectListItem {Text = "Image 3", Value = "Image_3"},
                        new SelectListItem {Text = "Image 4", Value = "Image_4"},
                     }, "Value", "Text");

                ViewBag.LibImage = maListe;

                // ViewBag.monId = (int)id;

                medium monMedia = new medium()
                {
                    id_media = -1,
                    id_projet = id.Value,
                    lien = null,
                    media = null,
                    type_media = "JPEG",
                    lib_image = libsel
                };

                return View(monMedia);
            }
            else
            {
                return View("Erreur");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMedia(medium createMedia, HttpPostedFileBase fileToUpload)
        {
            if (ModelState.IsValid)
            {
                if (createMedia.lib_image == null)
                {
                    return Redirect("/media/index?id=" + createMedia.id_projet + "&err=err1");
                }

                int tstimage = dalCProj.GetId_Media_all(createMedia.id_projet, createMedia.lib_image);
                if (tstimage != -1)
                {
                    return Redirect("/media/index?id=" + createMedia.id_projet + "&err=errl" + createMedia.lib_image);
                }

                if (fileToUpload == null)
                {
                    return Redirect("/media/index?id=" + createMedia.id_projet + "&err=errf");
                }

                if (fileToUpload.ContentLength > 2500000)
                {
                    return Redirect("/media/index?id=" + createMedia.id_projet + "&err=errfl");
                }

                if (fileToUpload.ContentLength > 0)
                {
                    // convert file choose by user into byte[]
                    byte[] imagePrincipale = null;
                    using (var binaryReader = new BinaryReader(fileToUpload.InputStream))
                    {
                        imagePrincipale = binaryReader.ReadBytes(fileToUpload.ContentLength);
                    }

                    dalCProj.AddMedia(imagePrincipale, null, createMedia.id_projet, "JPG", createMedia.lib_image);
                }

                // Ramener Idprojet et libImage 

                return RedirectToAction("Index", new { id = createMedia.id_projet });
            }
            return View(createMedia);
        }


        public ActionResult DeleteMedia(int? id)
        {
            if (id.HasValue)
            {
                int id_proj = dalCProj.DeleteMedia((int)id);
                if (id_proj == -1)
                {
                    return View("Erreur");
                }
                else
                {
                    return RedirectToAction("Index", new { id = id_proj });
                }

            }

            return View("Erreur");

        }


        public ActionResult GetPicture(int id)
        {
            var mymedia = dalCProj.GetPic(id);
            return File(mymedia.media, "image/jpg");
        }

    }
}