using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace Acura.Com.Web.App_Start {

    public class RouteConfig {
        /// <summary>
        /// Registers the routes described by routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes) {
            // Used for web services
            RouteTable.Routes.MapMvcAttributeRoutes();
        }
    }

}