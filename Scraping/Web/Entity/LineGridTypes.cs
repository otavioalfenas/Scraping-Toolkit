using System.Collections.Generic;

namespace Scraping.Web
{
    public class LineGridTypes : ComponentTypesBase
    {
        public LineGridTypes()
        {
            this.Columns = new List<ColumnGridTypes>();
        }
        public int? LineNumber { get; set; }
        public List<ColumnGridTypes> Columns { get; set; }
    }
}
