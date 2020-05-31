using HtmlAgilityPack;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static Scraping.Web.Enums;

namespace Scraping.Web
{
    public static class Extentions
    {
        /// <summary>
        /// Return bytes[] of stream
        /// </summary>
        /// <param name="stream">object stream</param>
        /// <returns>byte[]</returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            byte[] buffer = new byte[1024];
            BufferedStream bufferedStream = new BufferedStream(stream);
            MemoryStream memoryStream = new MemoryStream();
            int count;
            while ((count = bufferedStream.Read(buffer, 0, 1024)) > 0)
                memoryStream.Write(buffer, 0, count);
            return memoryStream.GetBuffer();
        }

        /// <summary>
        /// Search value from combo by text showing
        /// </summary>
        /// <param name="doc">HtmlAgilityPack loaded with page</param>
        /// <param name="Id">Item Id</param>
        /// <param name="text">Text to search</param>
        /// <returns></returns>
        public static string GetSelectedValueByText(this HtmlDocument doc, string Id, string text)
        {
            var option = doc.GetElementbyId(Id).ChildNodes.FirstOrDefault(x => x.InnerText.ToLower().RemoveDiacritics() == text.ToLower().RemoveDiacritics());

            var result = option != null
                ? option.Attributes["value"].Value
                : "Value Not Found.";
            return result;

        }

        public static string CleanText(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return Regex.Replace(text.ToUpper().RemoveExtraWhitespace().RemoveDiacritics().Trim(), "[^A-Za-z0-9]", "");
        }

        public static string RemoveExtraWhitespace(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return Regex.Replace(str, "\\s+", " ");
        }

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            string str = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < str.Length; ++index)
            {
                char ch = str[index];
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }

        public static string RemoveScriptsCss(this string html)
        {
            html = Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "onload=\".*?\"", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "onclick=\".*?\"", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<link rel=\"stylesheet\".*?.css\" />", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return html;
        }

        public static HtmlNodeCollection GetById(this string html, string id)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return GetById(doc, id);
        }

        public static HtmlNodeCollection GetByClassNameEquals(this string html, string className)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return GetByClassNameEquals(doc, className);
        }

        public static HtmlNodeCollection GetByClassNameContains(this string html, string className)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return GetByClassNameContains(doc, className);
        }

        public static HtmlNodeCollection GetById(this HtmlDocument html, string id)
        {
            return html.DocumentNode.SelectNodes($"//*[@id='{id}']");
        }

        public static HtmlNodeCollection GetByClassNameEquals(this HtmlDocument html, string className)
        {
            return html.DocumentNode.SelectNodes($"//*[@class='{className}']");
        }

        public static HtmlNodeCollection GetByClassNameContains(this HtmlDocument html, string className)
        {
            return html.DocumentNode.SelectNodes("//*[contains(@class, '" + className + "')]");
        }

        public static List<ImageTypes> ParseImage(this HtmlDocument doc)
        {
            List<ImageTypes> ret = null;
            var images = doc.DocumentNode.SelectNodes("//img");

            if (images != null)
            {
                ret = new List<ImageTypes>();
                foreach (var image in images)
                {
                    var classes = image.Attributes["class"]?.Value;
                    var id = image.Attributes["id"]?.Value;
                    var src = image.Attributes["src"]?.Value;
                    var alt = image.Attributes["Alt"]?.Value;
                    ret.Add(new ImageTypes()
                    {
                        Class = classes,
                        Id = id,
                        Alt = alt,
                        Src = src,
                    });
                }
            }
            return ret;
        }

        public static List<InputTextTypes> ParseInputText(this HtmlDocument doc)
        {
            List<InputTextTypes> ret = null;
            var inputs = doc.DocumentNode.SelectNodes("//input[@type='text']");

            if (inputs != null)
            {
                ret = new List<InputTextTypes>();
                foreach (var input in inputs)
                {
                    var classInput = input.Attributes["class"]?.Value;
                    var id = input.Attributes["id"]?.Value;
                    var name = input.Attributes["name"]?.Value;
                    var text = input.Attributes["value"]?.Value;
                    var maxLength = input.Attributes["maxLength"]?.Value;
                    var isEnabled = input.Attributes["disabled"] != null;

                    ret.Add(new InputTextTypes()
                    {
                        Id = id,
                        Class = classInput,
                        Name = name,
                        Text = text,
                        MaxLength = maxLength,
                        Label = "",
                        IsEnabled = true
                    });
                }
            }
            return ret;
        }

        public static List<InputTextTypes> ParseInputHiddenText(this HtmlDocument doc)
        {
            List<InputTextTypes> ret = null;
            var inputs = doc.DocumentNode.SelectNodes("//input[@type='hidden']");

            if (inputs != null)
            {
                ret = new List<InputTextTypes>();
                foreach (var input in inputs)
                {
                    var classInput = input.Attributes["class"]?.Value;
                    var id = input.Attributes["id"]?.Value;
                    var name = input.Attributes["name"]?.Value;
                    var text = input.Attributes["value"]?.Value;
                    var maxLength = input.Attributes["maxLength"]?.Value;
                    var isEnabled = input.Attributes["disabled"] != null;

                    ret.Add(new InputTextTypes()
                    {
                        Id = id,
                        Class = classInput,
                        Name = name,
                        Text = text,
                        MaxLength = maxLength,
                        Label = "",
                        IsEnabled = true
                    });
                }
            }
            return ret;
        }

        public static List<InputCheckBoxTypes> ParseCheckbox(this HtmlDocument doc)
        {
            List<InputCheckBoxTypes> ret = null;
            var checkBoxs = doc.DocumentNode.SelectNodes("//input[@type='checkbox']");

            if (checkBoxs != null)
            {
                ret = new List<InputCheckBoxTypes>();
                foreach (var checkbox in checkBoxs)
                {
                    var classCheckBox = checkbox.Attributes["class"]?.Value;
                    var id = checkbox.Attributes["id"]?.Value;
                    var name = checkbox.Attributes["name"]?.Value;
                    var isChecked = checkbox.Attributes["checked"] != null;
                    var isEnabled = checkbox.Attributes["disabled"] != null;
                    var text = checkbox.NextSibling?.InnerText?.Trim();
                    ret.Add(new InputCheckBoxTypes()
                    {
                        Class = classCheckBox,
                        Id = id,
                        Name = name,
                        IsChecked = isChecked,
                        IsEnabled = isEnabled,
                        Text = text
                    });
                }
            }
            return ret;
        }

        public static List<GridTypes> ParseGrid(this HtmlDocument doc)
        {
            List<GridTypes> ret = null;
            var grids = doc.DocumentNode.SelectNodes("//table");

            if (grids != null)
            {
                ret = new List<GridTypes>();
                foreach (var grid in grids)
                {
                    HtmlDocument htmlTable = new HtmlDocument();
                    htmlTable.LoadHtml(grid.OuterHtml);
                    var classe = grid.Attributes["class"]?.Value;
                    var id = grid.Attributes["id"]?.Value;
                    var name = grid.Attributes["name"]?.Value;
                    var linhas = htmlTable.DocumentNode.SelectNodes("//tbody/tr");
                    var heads = htmlTable.DocumentNode.SelectNodes("//thead/tr/th|td");

                    GridTypes gridTypes = new GridTypes();
                    gridTypes.Id = id;
                    gridTypes.Class = classe;
                    gridTypes.Name = name;
                    //cabecalho
                    if (heads != null)
                    {
                        for (int i = 0; i < heads.Count; i++)
                        {
                            var headHtml = heads[i];
                            gridTypes.Head.ColumnsHead.Add(new ColumnGridTypes() { Text = headHtml.InnerText.Trim(), Id = headHtml.Id, Position = i });
                        }
                    }

                    if (linhas != null && linhas.Count > 0)
                    {
                        for (int i = 0; i < linhas.Count; i++)
                        {
                            HtmlDocument htmlColumns = new HtmlDocument();
                            htmlColumns.LoadHtml(linhas[i].OuterHtml);
                            var classeLinha = linhas[i].Attributes["class"]?.Value;
                            var colunas = htmlColumns.DocumentNode.SelectNodes("//td");
                            LineGridTypes lines = new LineGridTypes();
                            lines.LineNumber = i;
                            lines.Class = classeLinha;
                            for (int x = 0; x < colunas.Count; x++)
                            {
                                var text = colunas[x].InnerText.Trim();
                                lines.Columns.Add(new ColumnGridTypes() { Text = text });
                            }
                            gridTypes.Lines.Add(lines);
                        }
                    }
                    ret.Add(gridTypes);
                }
            }
            return ret;
        }

        public static List<LinkTypes> ParseLink(this HtmlDocument doc)
        {
            List<LinkTypes> ret = null;
            var links = doc.DocumentNode.SelectNodes("//a");

            if (links != null)
            {
                ret = new List<LinkTypes>();
                foreach (var link in links)
                {
                    var classe = link.Attributes["class"]?.Value;
                    var id = link.Attributes["id"]?.Value;
                    var name = link.Attributes["name"]?.Value;
                    var href = link.Attributes["href"]?.Value;
                    var text = link.InnerHtml.Trim();

                    ret.Add(new LinkTypes()
                    {
                        Id = id,
                        Class = classe,
                        Name = name,
                        Href = href,
                        Text = text,
                    });
                }
            }
            return ret;
        }

        public static List<ComboBoxTypes> ParseCombo(this HtmlDocument doc)
        {
            List<ComboBoxTypes> ret = null;
            var combos = doc.DocumentNode.SelectNodes("//select");

            if (combos != null)
            {
                ret = new List<ComboBoxTypes>();
                foreach (var combo in combos)
                {
                    //var label = combo.ParentNode.PreviousSibling?.PreviousSibling?.InnerText;
                    var classCombo = combo.Attributes["class"]?.Value;
                    var id = combo.Attributes["id"]?.Value;
                    var name = combo.Attributes["name"]?.Value;
                    var itens = combo.Descendants()?.Where(d => d.Name == "option").Select(d => new { Valor = d.Attributes["value"]?.Value, Texto = d.InnerText, Selected = (d.Attributes["selected"] == null ? false : true) }).ToList();
                    ret.Add(new ComboBoxTypes()
                    {
                        Id = id,
                        Class = classCombo,
                        Name = name
                    });

                    foreach (var item in itens)
                    {
                        ret.LastOrDefault().ComboBoxItems.Add(new ComboBoxItemType() { Option = item.Texto, Value = item.Valor, IsSelected = item.Selected });
                    }
                }
            }
            return ret;
        }

        public static ComponentList GetAllComponents(this HtmlNode html, TypeComponent component)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.OuterHtml);
            return GetAllComponents(doc, component);
        }

        public static HtmlNodeCollection GetTags(this HtmlNode html, string tag)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.OuterHtml);
            return doc.DocumentNode.SelectNodes($"//{tag}");
        }

        public static ComponentList GetAllComponents(this HtmlDocument html, TypeComponent component)
        {
            ComponentList components = new ComponentList();
            switch (component)
            {
               
                case TypeComponent.InputText:
                    components.InputTexts = html.ParseInputText();
                    return components;
                case TypeComponent.InputCheckbox:
                    components.InputCheckBoxes = html.ParseCheckbox();
                    return components;
                case TypeComponent.InputHidden:
                    components.InputHidden = html.ParseInputHiddenText();
                    return components;
                case TypeComponent.ComboBox:
                    components.ComboBoxes = html.ParseCombo();
                    return components;
                case TypeComponent.DataGrid:
                    components.Grids = html.ParseGrid();
                    return components;
                case TypeComponent.LinkButton:
                    components.LinkButtons = html.ParseLink();
                    return components;
                case TypeComponent.Image:
                    components.Images = html.ParseImage();
                    return components;
                default:
                    return null;
            }
        }
    }
}
