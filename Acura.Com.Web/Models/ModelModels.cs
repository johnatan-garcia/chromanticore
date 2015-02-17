using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Razorfish.Extensions;

namespace Acura.Com.Web.Models {

    public class CarModel {

        public string Name { get; set; }

        public string Tagline { get; set; }

        public string PricingTagline { get; set; }

        public string SmallPhoto { get; set; }

        public CarModel(Item item) {
            if (item != null) {
                this.Name = item.Fields["Name"].Value;
                this.Tagline = item.Fields["Tagline"].Value;
                this.PricingTagline = item.Fields["Pricing Tagline"].Value;
                this.SmallPhoto = item.GetImageUrl("Small Photo");
            }
        }

    }
}