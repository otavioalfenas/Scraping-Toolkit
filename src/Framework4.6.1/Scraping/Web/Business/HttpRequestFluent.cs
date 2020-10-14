using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Scraping.Web.Enums;

namespace Scraping.Web
{
    public class HttpRequestFluent
    {
        private HttpRequest request = new HttpRequest(false);
        
        public event EventHandler<RequestHttpEventArgs> OnLoad
        {
            add { request.OnLoad += value; }
            remove { request.OnLoad -= value; }
        }

        public HttpRequestFluent(bool sslIgnore)
        {
            request = new HttpRequest(sslIgnore);
        }

        public HttpRequestFluent WithUserAgent(string userAgent)
        {
            request.UserAgent = userAgent;
            return this;
        }

        public HttpRequestFluent WithPreAuthenticate(bool preAuthenticate)
        {
            request.PreAuthenticate = preAuthenticate;
            return this;
        }

        public HttpRequestFluent WithAccept(string accept)
        {
            request.Accept = accept;
            return this;
        }

        public HttpRequestFluent FromUrl(string url)
        {
            request.Url = url;
            return this;
        }

        public HttpRequestFluent WithTimeoutRequest(int timeoutRequest)
        {
            request.TimeoutRequest = timeoutRequest;
            return this;
        }

        public HttpRequestFluent KeepAlive(bool keepAlive)
        {
            request.KeepAlive = keepAlive;
            return this;
        }

        public HttpRequestFluent WithAcceptEncoding(string acceptEncoding)
        {
            request.AcceptEncoding = acceptEncoding;
            return this;
        }

        public HttpRequestFluent WithAcceptLanguage(string acceptLanguage)
        {
            request.AcceptLanguage = acceptLanguage;
            return this;
        }

        public HttpRequestFluent WithRequestedWith(string requestedWith)
        {
            request.RequestedWith = requestedWith;
            return this;
        }

        public HttpRequestFluent WithAutoRedirect(bool autoRedirect)
        {
            request.AutoRedirect = autoRedirect;
            return this;
        }

        public HttpRequestFluent WithContentType(string contentType)
        {
            request.ContentType = contentType;
            return this;
        }

        public HttpRequestFluent TryGetComponents(TypeComponent TypeComponent)
        {
            request.TypeComponent = TypeComponent;
            return this;
        }

        public HttpRequestFluent WithParameters(NameValueCollection parameter)
        {
            if (parameter != null)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (string s in ((IEnumerable<string>)parameter.AllKeys).Where<string>(d => d != null))
                    stringBuilder.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(request.DefaultEncoding.GetBytes(s)), HttpUtility.UrlEncode(request.DefaultEncoding.GetBytes(parameter[s] == null ? string.Empty : parameter[s])));

                string str = stringBuilder.ToString();
                string param = str.Remove(str.Length - 1);

                request.Parameters = param;
            }
            return this;
        }

        public HttpRequestFluent WithParameters(string parameter)
        {
            if (parameter != null)
                request.Parameters = parameter;

            return this;
        }

        public HttpRequestFluent AddHeader(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CustomException("The name is required to add the header.");

            request.AddHeader(name, value);

            return this;
        }

        public HttpRequestFluent AddHeader(Dictionary<String, String> nameValues)
        {
            foreach (var item in nameValues)
            {
                request.AddHeader(item.Key, item.Value);
            }

            return this;
        }

        public HttpRequestFluent RemoveHeader(string name)
        {
            request.RemoveHeader(name);

            return this;
        }

        public HttpRequestFluent RemoveHeader(List<string> names)
        {
            foreach (var item in names)
            {
                request.RemoveHeader(item);
            }

            return this;
        }

        public HttpRequestFluent WithMaxRedirect(int maxRedirect)
        {
            request.MaxRedirect = maxRedirect;

            return this;
        }

        public HttpRequestFluent WithReferer(string referer)
        {
            request.Referer = referer;
            return this;
        }

        public HttpRequestFluent WithCookies(CookieCollection cookies)
        {
            request.AllCookies = cookies;
            return this;
        }

        public HttpRequestFluent AddCookie(Cookie cookie)
        {
            request.AllCookies.Add(cookie);
            return this;
        }

        public HttpRequestFluent AddCookie(string name, string value)
        {
            request.AllCookies.Add(new Cookie
            {
                Name = name,
                Value = value
            });
            return this;
        }

        public ResponseHttp Load()
        {
            lock(this.request.SyncRoot)
            {
                return request.LoadPage(request.Url);
            }
        }

        public async Task<ResponseHttp> LoadAsync()
        {
            if (string.IsNullOrWhiteSpace(request.Url))
                throw new CustomException("The Url field is blank or invalid.");
            return await request.LoadPageAsync(request.Url);
        }
    }
}
