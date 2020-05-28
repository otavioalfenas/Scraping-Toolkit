using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping
{
    public class LineGridTypes:ComponentTypesBase
    {
        public LineGridTypes()
        {
            this.Columns = new List<ColumnGridTypes>();
        }
        public int? LineNumber { get; set; }
        public List<ColumnGridTypes> Columns { get; set; }
    }
}
