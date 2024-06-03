using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArchitectureCreator
{
    public class Element
    {
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string category { get; set; }

        public Element(string nameT, string descriptionT, string imageT, string categoryT)
        {
            name = nameT;
            description = descriptionT;
            image = imageT;
            category = categoryT;
        }
    }

}
