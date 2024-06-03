using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace ArchitectureCreator
{
    public static class FileManager
    {
        public static List<Element> LoadElements()
        {
            string json = File.ReadAllText("elements.json");
            return JsonConvert.DeserializeObject<List<Element>>(json);
        }
        public static void AddImagePath(List<Element> elements)
        {
            string imageT;
            foreach (Element element in elements)
            {
                imageT = element.image;
                element.image = Path.Combine(Environment.CurrentDirectory, "Images", imageT);
            }
        }
    }
}
