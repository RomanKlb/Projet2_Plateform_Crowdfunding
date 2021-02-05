using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.Models
{
    public class DalCatalogue : IDalCatalogue
    {

        private projet2Entities CatalogueBdd;

        public DalCatalogue()
        {
            CatalogueBdd = new projet2Entities();
        }

        public void Dispose()
        {
            CatalogueBdd.Dispose();
        }

        public List<projet> ObtenirTousLesProjets()
        {
            return CatalogueBdd.projets.ToList();
        }

        public medium GetPic(int id)
        {
            var picture = CatalogueBdd.media.Find(id);
            return picture;
        }

        public medium GetId_Media(int id_projet)
        {
            return CatalogueBdd.media.FirstOrDefault(p => p.id_projet == id_projet && p.lib_image == "principale");
        }

    }
}