using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Razorfish.Controls
{
    public class ImageLink : Sitecore.Web.UI.HtmlControls.Control
    {

        private string fieldName = string.Empty;
        private string itemID = string.Empty;
        private string source = string.Empty;
        private bool isEvent = true;

        public string FieldName
        {

            get
            {

                return fieldName;

            }

            set
            {

                fieldName = value;

            }

        }



        public string ItemID
        {

            get
            {

                return itemID;

            }

            set
            {

                itemID = value;

            }

        }



        public string Source
        {

            get
            {

                return StringUtil.GetString(source);

            }

            set
            {



                if (value.IndexOf('&') > -1)
                {

                    source = value.Substring(0, value.IndexOf('&'));

                    if (value.ToLower().IndexOf("separator", value.IndexOf('&')) > -1)
                    {

                        string[] parameters = value.Split('&');

                        for (int i = 1; i < parameters.Length; i++)
                        {

                            if (parameters[i].ToLower().IndexOf("separator") > -1)
                            {

                                Separator = parameters[i].Substring((parameters[i].IndexOf("=") > -1) ? parameters[i].IndexOf("=") + 1 : 0);

                            }

                        }

                    }

                }

                else
                {

                    source = value;

                }

            }

        }



        public string Separator
        {

            get
            {

                if (ViewState[this.ClientID + "_separator"] != null)
                {

                    return ViewState[this.ClientID + "_separator"].ToString();

                }

                else
                {

                    return ", ";

                }

            }

            set
            {

                ViewState[this.ClientID + "_separator"] = value;

            }

        }



        public bool TrackModified
        {

            get
            {

                return base.GetViewStateBool("TrackModified", true);

            }

            set
            {

                base.SetViewStateBool("TrackModified", value, true);

            }

        }

        public ImageLink()
        {
            this.Class = "scContentControl";
            base.Activation = true;
        } 

    }
}
