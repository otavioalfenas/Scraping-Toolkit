using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
