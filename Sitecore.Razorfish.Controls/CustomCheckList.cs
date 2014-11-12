using System;
using System.Web.UI;
using System.Collections;

using Sitecore;
using Sitecore.Text;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Web.UI.HtmlControls;

namespace Sitecore.Razorfish.Controls {
	public class CustomChecklist: Sitecore.Web.UI.HtmlControls.Control {

        #region Private Variables
        private string _fieldName = string.Empty;
        private string _itemID = string.Empty;
        private string _source = string.Empty;
        private bool _isEvent = true;
        #endregion Private Variables

        #region Properties
        public string FieldName {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public string ItemID {
            get { return _itemID; }
            set { _itemID = value; }
        }

        public string Source {
            get { return StringUtil.GetString(_source); }
            set {
                if (value.IndexOf('&') > -1) {
                    _source = value.Substring(0, value.IndexOf('&'));
                    if (value.ToLower().IndexOf("separator", value.IndexOf('&')) > -1) {
                        string[] parameters = value.Split('&');
                        
                        for (int i = 1; i < parameters.Length; i++) {
                            if (parameters[i].ToLower().IndexOf("separator") > -1) {
                                Separator = parameters[i].Substring((parameters[i].IndexOf("=") > -1) ? parameters[i].IndexOf("=")+1 : 0);
                            }
                        }
                    }
                }
                else {
                    _source = value;
                }
            }
        }

        public string Separator {
            get {
                if (ViewState[this.ClientID + "_separator"] != null) {
                    return ViewState[this.ClientID+"_separator"].ToString();
                }
                else {
                    return ", ";
                }
            }
            set {
                ViewState[this.ClientID+"_separator"] = value;
            }
        }

        public bool TrackModified {
            get {
                return base.GetViewStateBool("TrackModified", true);
            }
            set {
                base.SetViewStateBool("TrackModified", value, true);
            }
        }
        #endregion Properties

        #region Constructor
        public CustomChecklist() {
            this.Class = "scContentControl";
            base.Activation = true;
        }
        #endregion Constructor

        #region Overrides
        protected override void OnLoad(EventArgs e) {
            // For the sake of performance controls should only be created once (during the first load)
            if (!Sitecore.Context.ClientPage.IsEvent) {
                _isEvent = false;
                // Use Sitecore's checklist
                var list = new Sitecore.Shell.Applications.ContentEditor.Checklist();
                this.Controls.Add(list);

                list.ID = GetID("list");
                list.Source = this.Source;
                list.ItemID = ItemID;
                list.FieldName = FieldName;
                list.TrackModified = TrackModified;
                list.Disabled = this.Disabled;
                list.Value = this.Value;

                // Use Sitecore's text control
                var text = new Sitecore.Shell.Applications.ContentEditor.Text();
                this.Controls.AddAt(0, text);
                text.ID = GetID("text");
                text.ReadOnly = true;
                text.Disabled = this.Disabled;

                // Label for the control
                this.Controls.Add(new LiteralControl(Sitecore.Resources.Images.GetSpacer(0x18, 16)));
            }
            else {
                // Recompose value from list. I wonder if the OnLoad method is hit because of postback with ajax?
                var list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
                if (list != null) {
                    var valueList = new ListString();
                    foreach (DataChecklistItem item in list.Items) {
                        if (item.Checked) {
                            valueList.Add(item.ItemID);
                        }
                    }
                    
                    if (this.Value != valueList.ToString()) {
                        this.TrackModified = list.TrackModified;
                        this.SetModified();
                    }
                    this.Value = valueList.ToString();
                }
            }
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e) {
            var list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
            var text = FindControl(GetID("text")) as Sitecore.Shell.Applications.ContentEditor.Text;
            FillTextBox(text, list);
            if (!_isEvent) {
                if (list != null) {
                    for (int i = 0; i < list.Items.Length; i++ ) {
                        list.Items[i].ServerProperties["Click"] = string.Format("{0}.ListItemClick", this.ID);
                    }
                }
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// Checkbox click event handler. Does not perform anything, since value refresh is handled in the OnLoad override
        /// </summary>
        public void ListItemClick() {
            var list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
            Sitecore.Context.ClientPage.ClientResponse.SetReturnValue(true);
        }

        /// <summary>
        /// Override for Sitecore's message handling system.
        /// </summary>
        /// <param name="message">Message to be handled</param>
        public override void HandleMessage(Sitecore.Web.UI.Sheer.Message message) {
            if (message["id"] == this.ID) {
                var list = FindControl(GetID("list")) as Sitecore.Shell.Applications.ContentEditor.Checklist;
                var text = FindControl(GetID("text")) as Sitecore.Shell.Applications.ContentEditor.Text;
                if (list != null) {
                    string messageText;
                    if ((messageText = message.Name) == null) { // Isn't this too JS?
                        return;
                    }
                    if (messageText != "checklist:checkall") {
                        if (messageText == "checklist:uncheckall") {
                            list.UncheckAll();
                        }
                        else if (messageText == "checklist:invert") {
                            list.Invert();
                        }
                    }
                    else {
                        list.CheckAll();
                    }
                }
            }
            
            base.HandleMessage(message);
        }
        #endregion Overrides

        #region Protected Scope
        protected virtual void FillTextBox(Sitecore.Shell.Applications.ContentEditor.Text text, Sitecore.Shell.Applications.ContentEditor.Checklist list) {
            if ((text != null) && (list != null) && (list.Items != null)) {
                text.Value = string.Empty;
                Hashtable textItems = new Hashtable();

                for (int i = 0; i < list.Items.Length; i++) {
                    if (list.Items[i].Checked) {
                        string itemTitle = list.Items[i].Header;
                        if (!textItems.ContainsKey(itemTitle)) {
                            textItems.Add(itemTitle, (int)1);
                            text.Value = text.Value + (text.Value == string.Empty ? string.Empty : Separator) + itemTitle;
                        }
                        else {
                            int sameItemsCount = (int)textItems[itemTitle];
                            if (sameItemsCount == 1) {
                                if (text.Value.IndexOf(Separator + itemTitle + Separator) > -1) {
                                    text.Value = text.Value.Replace(Separator + itemTitle + Separator, Separator + itemTitle + " (1)" + Separator);
                                }
                                else {
                                    if (text.Value.IndexOf(itemTitle + Separator) == 0) {
                                        text.Value = text.Value.Insert(itemTitle.Length, " (1)");
                                    }
                                    else {
                                        if ((text.Value.IndexOf(Separator + itemTitle) == (text.Value.Length - (Separator.Length + itemTitle.Length))) || (text.Value == itemTitle)) {
                                            text.Value += " (1)";
                                        }
                                    }
                                }
                                
                                text.Value.Replace(itemTitle, itemTitle + "(1)");
                            }

                            textItems[itemTitle] = ++sameItemsCount;
                            text.Value = string.Format("{0}{1}{2} ({3})", text.Value,  (text.Value == string.Empty ? string.Empty : Separator),  itemTitle,  sameItemsCount); 
                        }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Sets the current page context to modified, so the user is prompted to save changes on leaving
        /// TODO: Refactor method and property to 
        /// </summary>
        protected virtual void SetModified() {
            if (this.TrackModified) {
                Sitecore.Context.ClientPage.Modified = true;
            }
        }
        #endregion Protected Scope

    }

}