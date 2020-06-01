using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Scraping.Web
{
    public class HttpRequest : RequestHttpBase
    {
        public event EventHandler<RequestHttpEventArgs> OnLoad = null;
        public ResponseHttp responseHttp = null;

        public HttpRequest(bool sslIgnore) : base(sslIgnore)
        {
            if (!sslIgnore)
                return;

            if(ServicePointManager.ServerCertificateValidationCallback==null)
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ByPassAllCertificateStuff);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
        private static bool ByPassAllCertificateStuff(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        internal async Task<ResponseHttp> LoadPageAsync(string url)
        {
            responseHttp = new ResponseHttp();
            this.Url = url;
       
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                    httpWebRequest.UserAgent = UserAgent;
                    httpWebRequest.PreAuthenticate = PreAuthenticate;
                    httpWebRequest.Accept = Accept;
                    httpWebRequest.Timeout = TimeoutRequest;
                    httpWebRequest.KeepAlive = KeepAlive;

                    if (this.Parameters != null && this.Parameters.Length > 0)
                        httpWebRequest.Method = "POST";
                    else
                        httpWebRequest.Method = "GET";

                    httpWebRequest.Headers["Accept-Encoding"] = AcceptEncoding;
                    httpWebRequest.Headers["Accept-Language"] = AcceptLanguage;
                    httpWebRequest.Headers["UA-CPU"] = UACPU;
                    httpWebRequest.Headers["Cache-Control"] = CacheControl;

                    if (!string.IsNullOrEmpty(RequestedWith))
                        httpWebRequest.Headers["x-requested-with"] = RequestedWith;

                    httpWebRequest.ContentLength = 0L;
                    httpWebRequest.AllowAutoRedirect = AutoRedirect;

                    if (Headers != null)
                    {
                        foreach (KeyValuePair<string, string> header in Headers)
                            httpWebRequest.Headers.Add(header.Key, header.Value);
                    }

                    if (Referer != null)
                        httpWebRequest.Referer = Referer;
                    httpWebRequest.CookieContainer = new CookieContainer();

                    if (AllCookies.Count > 0)
                    {
                        foreach (Cookie todosCookie in AllCookies)
                            httpWebRequest.CookieContainer.Add(todosCookie);
                    }

                    if (Parameters != null && Parameters.Trim().Length > 0)
                    {
                        byte[] bytes = EncodingPage.GetBytes(Parameters);
                        httpWebRequest.Method = "POST";
                        httpWebRequest.ContentType = ContentType;
                        httpWebRequest.ContentLength = bytes.Length;
                        using (Stream requestStream = httpWebRequest.GetRequestStream())
                        {
                            requestStream.Write(bytes, 0, bytes.Length);
                            requestStream.Close();
                        }
                    }

                    string html;
                    string responseHeader;
                    using (HttpWebResponse response =(HttpWebResponse)await httpWebRequest.GetResponseAsync())
                    {
                        responseHttp.StatusCode = response.StatusCode;

                        if (response.ContentType.StartsWith("image"))
                        {
                            using (Stream responseStream = response.GetResponseStream())
                                html = string.Format("data:{0};base64,{1}", response.ContentType, Convert.ToBase64String(responseStream.ReadAllBytes()));
                        }
                        else
                            html = GetResponseHtml(response);

                        AddCookies(response.Cookies, responseHttp);
                        AddInternalCookie(response, responseHttp);
                        responseHeader = response.GetResponseHeader("Location");
                        responseHttp.UrlLocation = responseHeader;
                        responseHttp.Method = response.Method;
                        responseHttp.Server = response.Server;
                        responseHttp.HeadersAdded = this.Headers;
                    }

                    if (MaxRedirect > 0)
                    {
                        if (!string.IsNullOrEmpty(responseHeader) && Uri.IsWellFormedUriString(responseHeader, UriKind.Absolute))
                        {
                            MaxRedirect--;
                            this.Referer = url;
                            return await LoadPageAsync(responseHeader);
                        }
                    }
                    responseHttp.HtmlPage = html;

                    LoadTypes(html, responseHttp);

                    OnLoad?.Invoke(this, new RequestHttpEventArgs(html, responseHttp));

                    return responseHttp;
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseHttp.HtmlPage = reader.ReadToEnd();
                        }
                    }
                    responseHttp.StatusCode = HttpStatusCode.InternalServerError;
                }
                //catch (Exception ex)
                //{
                //    return null;
                //}

                return responseHttp;
        }

        private string GetResponseHtml(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                Stream stream = null;
                try
                {
                    stream = string.IsNullOrEmpty(response.ContentEncoding) || !response.ContentEncoding.ToLower().Contains("gzip") ? (string.IsNullOrEmpty(response.ContentEncoding) || !response.ContentEncoding.ToLower().Contains("deflate") ? responseStream : new DeflateStream(responseStream, CompressionMode.Decompress)) : new GZipStream(responseStream, CompressionMode.Decompress);
                    using (StreamReader streamReader = new StreamReader(stream, DefaultEncoding))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
        }

        public async Task<ResponseHttp> LoadPageAsync(string url, NameValueCollection parameter)
        {
            return await this.LoadPageAsync(url, parameter, string.Empty);
        }

        public async Task<ResponseHttp> LoadPageAsync(string url, NameValueCollection parameter, string referer)
        {
            if (parameter != null)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (string s in ((IEnumerable<string>)parameter.AllKeys).Where<string>(d => d != null))
                    stringBuilder.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(this.DefaultEncoding.GetBytes(s)), HttpUtility.UrlEncode(this.DefaultEncoding.GetBytes(parameter[s] == null ? string.Empty : parameter[s])));

                string str = stringBuilder.ToString();
                string param = str.Remove(str.Length - 1);
                this.Parameters = param;
            }
            this.Referer = referer;
            return await this.LoadPageAsync(url);
        }

        private void LoadTypes(String html, ResponseHttp responseHttp)
        {
            if (TypeComponent == Enums.TypeComponent.None)
                return;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(html);

            if ((TypeComponent & Enums.TypeComponent.ComboBox) == Enums.TypeComponent.ComboBox)
                responseHttp.Components.ComboBoxes = doc.ParseCombo();

            if ((TypeComponent & Enums.TypeComponent.LinkButton) == Enums.TypeComponent.LinkButton)
                responseHttp.Components.LinkButtons = doc.ParseLink();

            if ((TypeComponent & Enums.TypeComponent.DataGrid) == Enums.TypeComponent.DataGrid)
                responseHttp.Components.Grids = doc.ParseGrid();

            if ((TypeComponent & Enums.TypeComponent.InputCheckbox) == Enums.TypeComponent.InputCheckbox)
                responseHttp.Components.InputCheckBoxes = doc.ParseCheckbox();

            if ((TypeComponent & Enums.TypeComponent.Image) == Enums.TypeComponent.Image)
                responseHttp.Components.Images = doc.ParseImage();

            if ((TypeComponent & Enums.TypeComponent.InputText) == Enums.TypeComponent.InputText)
                responseHttp.Components.InputTexts = doc.ParseInputText();

            if ((TypeComponent & Enums.TypeComponent.InputHidden) == Enums.TypeComponent.InputHidden)
                responseHttp.Components.InputHidden = doc.ParseInputHiddenText();
        }
    }
}
