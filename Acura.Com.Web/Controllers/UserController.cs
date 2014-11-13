using Acura.Com.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Acura.Com.Web.Controllers
{
    public class UserController : Controller
    {

        public PartialViewResult Technologies()
        {
            // Logica de negocio
            // Obtener datos de SC
            var technologyList = new List<Technology>();
            technologyList.Add(new Technology { Icon = "desktop", Name = "Responsive", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse interdum erat et neque tincidunt volutpat. Cras eget augue id dui varius pretium." });
            technologyList.Add(new Technology { Icon = "css3", Name = "CSS3", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse interdum erat et neque tincidunt volutpat. Cras eget augue id dui varius pretium." });
            technologyList.Add(new Technology { Icon = "lightbulb-o", Name = "Javascript", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse interdum erat et neque tincidunt volutpat. Cras eget augue id dui varius pretium." });

            return PartialView(technologyList);
        }

    }
}