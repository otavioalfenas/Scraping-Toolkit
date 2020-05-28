using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping
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
