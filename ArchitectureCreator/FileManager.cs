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
            List<Element> elements_ns;
            List<Element> elements = new List<Element>();
            string json = File.ReadAllText("elements.json");
            string imageT;

            elements_ns = JsonConvert.DeserializeObject<List<Element>>(json);
            foreach (Element element in elements_ns)
            {
                imageT = element.image;
                element.image = Path.Combine(Environment.CurrentDirectory, "Images", imageT);
                if (element.category == "Chair")
                {
                    elements.Add(new Chair(element.category, element.name, element.description, element.image, element.width, element.height));
                }
                else if (element.category == "Desk")
                {
                    elements.Add(new Desk(element.category, element.name, element.description, element.image, element.width, element.height));
                }
                else if (element.category == "Door")
                {
                    elements.Add(new Door(element.category, element.name, element.description, element.image, element.width, element.height));
                }
                else if (element.category == "Window")
                {
                    elements.Add(new WindowO(element.category, element.name, element.description, element.image, element.width, element.height));
                }
                else if (element.category == "Dresser")
                {
                    elements.Add(new Dresser(element.category, element.name, element.description, element.image, element.width, element.height));
                }
                else if (element.category == "Wardrobe")
                {
                    elements.Add(new Wardrobe(element.category, element.name, element.description, element.image, element.width, element.height));
                }
                else if (element.category == "Bed")
                {
                    elements.Add(new Bed(element.category, element.name, element.description, element.image, element.width, element.height));
                }
            }
            return elements;
        }
    }
}
