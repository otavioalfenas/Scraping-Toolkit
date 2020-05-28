using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraping
{
    public class ComboBoxTypes: ComponentTypesBase
    {
        public ComboBoxTypes()
        {
            this.ComboBoxItems = new List<ComboBoxItemType>();
        }
        public List<ComboBoxItemType> ComboBoxItems { get; set; }
    }
}
