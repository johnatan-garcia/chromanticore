﻿using Acura.Com.Web.App_Start;
using Acura.Com.Web.App_Start.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(Startup), "PreInit")]
namespace Acura.Com.Web.App_Start {
    /// <summary>
    /// A startup.
    /// </summary>
    public class Startup : BaseStartup<Startup> {
        /// <summary>
        /// Will run when the application is starting (same as Application_Start)
        /// Called by the assembly PreApplicationStartMethod attribute.
        /// </summary>
        public static void PreInit() {
            // Call base registeration
            RegisterStartup();
        }

        /// <summary>
        /// Executes the start action.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnStart(HttpApplication context) {
            base.OnStart(context);

            // Register web services configurations and routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
