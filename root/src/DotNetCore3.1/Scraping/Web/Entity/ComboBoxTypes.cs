using System.Collections.Generic;

namespace Scraping.Web
{
    public class ComboBoxTypes : ComponentTypesBase
    {
        public ComboBoxTypes()
        {
            this.ComboBoxItems = new List<ComboBoxItemType>();
        }
        public List<ComboBoxItemType> ComboBoxItems { get; set; }
    }
}
