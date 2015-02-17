using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace Acura.Com.Web.Formatters {

    /// <summary>
    /// Encapsulates the result of a JSON net.
    /// </summary>
    public class JsonNetResult : JsonResult {

        public JsonSerializerSettings Settings { get; set; }
        
        public Formatting Formatting { get; set; }

        public JsonNetResult() {
            this.Settings = new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            this.Formatting = Formatting.None;
            this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)) {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null) {
                response.ContentEncoding = ContentEncoding;
            }

            if (this.Data != null) {
                var serializer = JsonSerializer.Create(Settings);
                var writer = new JsonTextWriter(response.Output) {
                    Formatting = this.Formatting
                };

                serializer.Serialize(writer, this.Data);
                writer.Flush();
            }
        }
    }
}