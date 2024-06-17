using Newtonsoft.Json;
using System.IO;

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
        public static List<CanvasElement> SorterElements(List<CanvasElement> canvas_ns)
        {
            List<CanvasElement> elements = new List<CanvasElement>();
            foreach (CanvasElement element in canvas_ns)
            {
                if (element.etype.category == "Chair")
                {
                    elements.Add(new CanvasElement(new Chair(element.etype), element.position, element.angle));
                }
                else if (element.etype.category == "Desk")
                {
                    elements.Add(new CanvasElement(new Desk(element.etype), element.position, element.angle));
                }
                else if (element.etype.category == "Door")
                {
                    elements.Add(new CanvasElement(new Door(element.etype), element.position, element.angle));
                }
                else if (element.etype.category == "Window")
                {
                    elements.Add(new CanvasElement(new WindowO(element.etype), element.position, element.angle));
                }
                else if (element.etype.category == "Dresser")
                {
                    elements.Add(new CanvasElement(new Dresser(element.etype), element.position, element.angle));
                }
                else if (element.etype.category == "Wardrobe")
                {
                    elements.Add(new CanvasElement(new Wardrobe(element.etype), element.position, element.angle));
                }
                else if (element.etype.category == "Bed")
                {
                    elements.Add(new CanvasElement(new Bed(element.etype), element.position, element.angle));
                }
            }
            return elements;
        }
    }
}
