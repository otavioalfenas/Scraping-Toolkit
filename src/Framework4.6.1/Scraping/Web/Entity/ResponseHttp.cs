using System;
using System.Collections.Generic;
using System.Net;

namespace Scraping.Web
{
    public class ResponseHttp
    {
        public ResponseHttp()
        {
            this.Components = new ComponentList();
        }
        public HttpStatusCode StatusCode { get; set; }
        public String HtmlPage { get; set; }
        public CookieCollection CookiesAdded { get; set; } = new CookieCollection();
        public string UrlLocation { get; set; }
        public string Method { get; set; }
        public string Server { get; set; }
        public Dictionary<string, string> HeadersAdded = new Dictionary<string, string>();
        public ComponentList Components { get; set;   }
    }
}
