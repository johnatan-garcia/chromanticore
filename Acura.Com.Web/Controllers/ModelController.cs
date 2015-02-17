using Acura.Com.Web.Controllers.Abstracts;
using Acura.Com.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace Acura.Com.Web.Controllers
{
    [RoutePrefix("api/model")]
    public class ModelController : BaseController
    {

        [Route("carmodels")]
        public JsonResult GetCarModels() {
            var items = Sitecore.Context.Database
                .GetItem("/sitecore/content/metadata/models")
                .Children
                .Select(x => new CarModel(x));

            return Json(items);
        }

    }
}