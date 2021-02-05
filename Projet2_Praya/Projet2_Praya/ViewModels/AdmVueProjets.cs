using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projet2_Praya.ViewModels
{
    public class AdmVueProjets
    {
        public string EtatProj { get; set; }

        public List<DetailsInformationProjet> lstprojets { get; set; }
    }
}