using System.Collections.Generic;

namespace Scraping.Web
{
    public class HeadGridTypes : ComponentTypesBase
    {
        public HeadGridTypes()
        {
            this.ColumnsHead = new List<ColumnGridTypes>();
        }
        public List<ColumnGridTypes> ColumnsHead { get; set; }
    }
}
