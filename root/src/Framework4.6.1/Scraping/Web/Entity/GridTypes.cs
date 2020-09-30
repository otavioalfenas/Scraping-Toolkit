using System.Collections.Generic;

namespace Scraping.Web
{
    public class GridTypes : ComponentTypesBase
    {
        public GridTypes()
        {
            this.Lines = new List<LineGridTypes>();
            this.Head = new HeadGridTypes();
        }
        public HeadGridTypes Head { get; set; }
        public List<LineGridTypes> Lines { get; set; }

        public int LinesTotal
        {
            get { return Lines != null ? Lines.Count : 0; }
        }
    }
}
