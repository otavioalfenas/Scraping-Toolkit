using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping.Web
{
    public class ComponentList
    {
        public ComponentList()
        {
            this.InputTexts = new List<InputTextTypes>();
            this.InputCheckBoxes = new List<InputCheckBoxTypes>();
            this.ComboBoxes = new List<ComboBoxTypes>();
            this.LinkButtons = new List<LinkTypes>();
            this.Grids = new List<GridTypes>();
            this.InputHidden = new List<InputTextTypes>();
            this.Images = new List<ImageTypes>();
        }
        public List<InputTextTypes> InputTexts { get; set; }
        public List<InputTextTypes> InputHidden { get; set; }
        public List<InputCheckBoxTypes> InputCheckBoxes { get; set; }
        public List<ComboBoxTypes> ComboBoxes { get; set; }
        public List<LinkTypes> LinkButtons { get; set; }
        public List<GridTypes> Grids { get; set; }
        public List<ImageTypes  > Images { get; set; }
    }
}
