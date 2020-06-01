using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping.Web
{
    public partial class InputCheckBoxTypes : ComponentTypesBase
    {
        public bool? IsChecked { get; set; }
        public String Text { get; set; }
    }
}
