using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraping.Web;
using static Scraping.Web.Enums;

namespace Test
{
    [TestClass]
    public class ExtentionsTest
    {
        [TestMethod]
        public void AllTags()
        {
            var ret = new HttpRequestFluent(true)
                .FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
                .Load();
            var byClassContain = ret.HtmlPage.GetByClassNameContains("Box mb-3 Box--");
            var byClassEquals = ret.HtmlPage.GetByClassNameEquals("Box mb-3 Box--condensed");
            var byId = ret.HtmlPage.GetById("readme");

            Assert.IsTrue(byClassContain != null && byClassEquals != null && byId != null);
        }

        [TestMethod]
        public void GetComponents()
        {
            var ret = new HttpRequestFluent(true)
                .FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
                .Load();
            var byClassEquals = ret.HtmlPage.GetByClassNameEquals("Box mb-3 Box--condensed");
            var images = byClassEquals.FirstOrDefault().GetAllComponents(TypeComponent.Image);
            var link = byClassEquals.FirstOrDefault().GetAllComponents(TypeComponent.LinkButton);
            Assert.IsTrue(images.Images.Count>0 && link.LinkButtons.Count>0);
        }
    }
}
