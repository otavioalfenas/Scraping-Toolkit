using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping
{
    public class ComponentTypesBase 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
