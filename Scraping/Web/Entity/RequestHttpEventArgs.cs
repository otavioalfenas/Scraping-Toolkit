using System;

namespace Scraping.Web
{
    public class RequestHttpEventArgs : EventArgs
    {
        public string HtmlPage { get; set; }
        public ResponseHttp ResponseHttp { get; set; }
        public RequestHttpEventArgs(string page, ResponseHttp responseHttp)
        {
            this.HtmlPage = page;
            this.ResponseHttp = responseHttp;
        }
    }
}
