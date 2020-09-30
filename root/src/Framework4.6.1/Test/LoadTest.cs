using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraping.Web;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class LoadTest
    {
        [TestMethod]
        public void LoagPage()
        {
            var ret = new HttpRequestFluent(true)
                .FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
                .Load();

            Assert.IsTrue(ret.HtmlPage != string.Empty);
        }

        [TestMethod]
        public void LoadComponents()
        {
            var ret = new HttpRequestFluent(true)
                .FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
                .TryGetComponents(Enums.TypeComponent.LinkButton| Enums.TypeComponent.InputHidden)
                .Load();

            Assert.IsTrue(ret.Components.LinkButtons.Count > 0 && ret.Components.InputHidden.Count > 0);
        }

        [TestMethod]
        public void LoadGetByClass()
        {
            var ret = new HttpRequestFluent(true)
                .FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
                .TryGetComponents(Enums.TypeComponent.LinkButton | Enums.TypeComponent.InputHidden)
                .Load();

            var itens = ret.HtmlPage.GetByClassNameEquals("edge-item-fix");
            var itens2 = ret.HtmlPage.GetByClassNameContains("edge-it");

            Assert.IsTrue(itens.Count > 0 && itens2.Count>0);
        }

        [TestMethod]
        public void LoadWithAutoRedirect()
        {
            var ret = new HttpRequestFluent(true)
                .FromUrl("https://github.com/otavioalfenas/RespireFundo/graphs/traffic")
                .TryGetComponents(Enums.TypeComponent.LinkButton | Enums.TypeComponent.InputHidden)
                .WithAutoRedirect(true)
                .Load();

            Assert.IsTrue(ret.HtmlPage.Contains("Password"));
        }

        [TestMethod]
        public void LoadWithoutAutoRedirect()
        {
            var ret = new HttpRequestFluent(true)
                .FromUrl("https://github.com/otavioalfenas/RespireFundo/graphs/traffic")
                .TryGetComponents(Enums.TypeComponent.LinkButton | Enums.TypeComponent.InputHidden)
                .WithAutoRedirect(false)
                .Load();

            Assert.IsTrue(!ret.HtmlPage.Contains("Password"));
        }

        [TestMethod]
        public async Task LoagPageNonFluent()
        {
            var http = new HttpRequest(true);
            var ret = await http.LoadPageAsync("https://github.com/otavioalfenas/Scraping-Toolkit", null);

            Assert.IsTrue(ret.HtmlPage != string.Empty);
        }
    }
}
