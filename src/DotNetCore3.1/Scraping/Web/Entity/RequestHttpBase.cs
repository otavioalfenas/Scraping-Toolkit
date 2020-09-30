using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Scraping.Web.Enums;

namespace Scraping.Web
{
    public abstract class RequestHttpBase
    {
        public TypeComponent TypeComponent = TypeComponent.None;

        public readonly object SyncRoot = new object();
        public CookieCollection AllCookies { get; set; } = new CookieCollection();

        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        public Encoding EncodingPage { get; set; } = Encoding.GetEncoding("ISO-8859-1");

        public Int32 TimeoutRequest { get; set; } = 50000;

        public bool AutoRedirect { get; set; }

        public string UserAgent { get; set; } = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 1.0.3705)";

        public bool PreAuthenticate { get; set; } = true;

        public string ContentType { get; set; } = "application/x-www-form-urlencoded";

        public bool KeepAlive { get; set; } = true;

        public string AcceptEncoding { get; set; } = "gzip, deflate";

        public string Accept { get; set; } = "*/*";

        public string AcceptLanguage { get; set; } = "pt-br";

        public string UACPU { get; set; } = "x86";

        public string CacheControl { get; set; } = "no-cache";

        public string RequestedWith { get; set; } = null;

        public string Referer { get; set; }

        public int MaxRedirect { get; set; } = 0;

        public string Parameters { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// Classe de requisição http
        /// </summary>
        /// <param name="sslIgnore">Ignora procotocolo SLL para conexão</param>
        public RequestHttpBase(bool sslIgnore)
        {
            this.SSLConfig(sslIgnore);
            IncreaseConectionLimit();
        }

        /// <summary>
        /// Aumenta o tempo limite de conexao 
        /// </summary>
        protected void IncreaseConectionLimit()
        {
            lock (SyncRoot)
                ServicePointManager.DefaultConnectionLimit = 999;
        }

        /// <summary>
        /// Diminui o tempo limite de conexao
        /// </summary>
        protected void DecreaseConectionLimit()
        {
            lock (SyncRoot)
            {
                int num = ServicePointManager.DefaultConnectionLimit - 1;
                if (num < 2)
                    ServicePointManager.DefaultConnectionLimit = 2;
                else
                    ServicePointManager.DefaultConnectionLimit = num;
            }
        }

        protected bool BypassAllCertificateStuff(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        /// <summary>
        /// Adicionado um cookie a requisição
        /// </summary>
        /// <param name="cookie">cokkie a ser adicionado</param>
        public void AddCookie(Cookie cookie)
        {
            this.AllCookies.Add(cookie);
        }

        /// <summary>
        /// Adiciona os cookies do resposta na requisição
        /// </summary>
        /// <param name="httpRes">resposta</param>
        /// <param name="responseHttp">requisição</param>
        protected void AddInternalCookie(HttpWebResponse httpRes, ResponseHttp responseHttp)
        {
            if (httpRes.Headers["Set-Cookie"] == null)
                return;

            string str1 = httpRes.Headers["Set-Cookie"].ToString().Trim().Split(';')[0];
            string name = str1.Split('=')[0];
            string str2 = str1.Split('=')[1];
            string empty = string.Empty;

            string str3;
            if (httpRes.Headers["Set-Cookie"].ToString().Trim().Contains(";"))
                str3 = httpRes.Headers["Set-Cookie"].ToString().Trim().Split(';')[0].Split('=')[1];
            else
                str3 = httpRes.Headers["Set-Cookie"].ToString().Trim();

            string domain = httpRes.ResponseUri.Host;

            if (domain.StartsWith("www."))
                domain = domain.Replace("www.", "");

            if (this.AllCookies[name] == null)
            {
                var cookie = new Cookie(name, str3, empty, domain);
                this.AllCookies.Add(cookie);
                responseHttp.CookiesAdded.Add(cookie);
            }
        }

        /// <summary>
        /// Adiciona uma coleção de cookies na requisição
        /// </summary>
        /// <param name="collection">coleção de cookies</param>
        /// <param name="responseHttp">requisição</param>
        public void AddCookies(CookieCollection collection, ResponseHttp responseHttp)
        {
            if (collection == null)
                return;
            foreach (Cookie cookie in collection)
            {
                if (this.AllCookies[cookie.Name] != null && this.AllCookies[cookie.Name].Domain == cookie.Domain)
                    this.AllCookies[cookie.Name].Value = cookie.Value;
                else
                    this.AllCookies.Add(cookie);

                responseHttp.CookiesAdded.Add(cookie);
            }
        }

        /// <summary>
        /// remove um cookie
        /// </summary>
        /// <param name="nome">nome do cookie que sera removido</param>
        public void RemoveCookie(string nome)
        {
            if (this.AllCookies[nome] != null)
                this.AllCookies[nome].Expired = true;
        }

        /// <summary>
        /// seta as configuração para aceitar protocolo de segurança SSL
        /// </summary>
        public void StartSSLConfig()
        {
            if (ServicePointManager.ServerCertificateValidationCallback == null)
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(BypassAllCertificateStuff);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Protocolo de segunça SSL
        /// </summary>
        /// <param name="config">Parametro se seta a configuração SSL</param>
        protected void SSLConfig(bool ignoreSslError)
        {
            if (!ignoreSslError)
                return;
            StartSSLConfig();
        }

        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Adicionar header para a requisição
        /// </summary>
        /// <param name="header">Nome de header</param>
        /// <param name="Valor">Valor do header</param>
        public void AddHeader(string name, string value)
        {
            if (this.Headers == null)
                this.Headers = new Dictionary<string, string>();
            if (this.Headers.ContainsKey(name))
                this.Headers[name] = value;
            else
                this.Headers.Add(name, value);
        }

        /// <summary>
        /// Remover header da requisição
        /// </summary>
        /// <param name="header">Nome do header</param>
        public void RemoveHeader(string name)
        {
            if (this.Headers == null)
                this.Headers = new Dictionary<string, string>();
            if (this.Headers.ContainsKey(name))
                this.Headers.Remove(name);
        }

        ~RequestHttpBase()
        {
            DecreaseConectionLimit();
        }
    }
}
