using System;
using System.IO;
using System.Text.RegularExpressions;

namespace traductor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Comenzando la traducción...");

            string pathAspProject = @"../../../../../asp-project";

            string pathResultado = @"../../../../../resultado_traductor";
            string pathCss = Path.Combine(pathResultado, "css");
            string pathAssets = Path.Combine(pathResultado, "assets");

            Console.WriteLine("Creando directorios...");
            if (!File.Exists(pathResultado))
            {
                Directory.CreateDirectory(pathResultado);
                Directory.CreateDirectory(pathCss);
                Directory.CreateDirectory(pathAssets);
                Console.WriteLine("La carpeta resultado_traductor ha sido creada exitosamente!");
            }

            Console.WriteLine("Copiando ficheros...");
            File.Copy($"{pathAspProject}/wwwroot/css/site.css", $"{pathCss}/site.css", true);
            File.Copy($"{pathAspProject}/wwwroot/assets/cheems.png", $"{pathAssets}/cheems.png", true);

            Console.WriteLine("Creando ficheros...");
            FileStream fs;
            fs = File.Create($"{pathResultado}/index.html");
            fs.Close();
            fs = File.Create($"{pathResultado}/tecnologias.html");
            fs.Close();
            fs = File.Create($"{pathResultado}/enlaces.html");
            fs.Close();

            Console.WriteLine("Recuperando HTML...");
            string layoutHtml = File.ReadAllText($"{pathAspProject}/Views/Shared/_Layout.cshtml");
            string indexHtml = File.ReadAllText($"{pathAspProject}/Views/Home/Index.cshtml");
            string enlacesHtml = File.ReadAllText($"{pathAspProject}/Views/Home/Enlaces.cshtml");
            string tecnologiasHtml = File.ReadAllText($"{pathAspProject}/Views/Home/Tecnologías.cshtml");

            /*
            public class Example
        {
            public static void Main()
            {
                string pattern = "(Mr\\.? |Mrs\\.? |Miss |Ms\\.? )";
                string[] names = { "Mr. Henry Hunt", "Ms. Sara Samuels",
                         "Abraham Adams", "Ms. Nicole Norris" };
                foreach (string name in names)
                    Console.WriteLine(Regex.Replace(name, pattern, String.Empty));
            }
        }
        // The example displays the following output:
        //    Henry Hunt
        //    Sara Samuels
        //    Abraham Adams
        //    Nicole Norris

            */


        Console.WriteLine("Traduciendo HTML...");

            string pRenderBody = @"@RenderBody\(\)";
            string pLinkCss = "~/css/site.css";
            string pCheems = "~/assets/cheems.png";
            string pAspController = $"asp-controller=\"Home\"";
            string pAspActionI = $"asp-action=\"Index\"";
            string pAspActionT = $"asp-action=\"Tecnologías\"";
            string pAspActionE = $"asp-action=\"Enlaces\"";

            Console.WriteLine(pAspController);

            string newLayoutHtml;
            string newIndexHtml;
            string newEnlacesHtml;
            string newTecnologíasHtml;

            newLayoutHtml = Regex.Replace(layoutHtml, pLinkCss, "/css/site.css");
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspController, String.Empty);
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspActionI, "href=\"./index.html\"");
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspActionT, "href=\"./tecnologias.html\"");
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspActionE, "href=\"./enlaces.html\"");

            newIndexHtml = Regex.Replace(newLayoutHtml, pRenderBody, indexHtml);
            newIndexHtml = Regex.Replace(newIndexHtml, pCheems, "/assets/cheems.png");

            newEnlacesHtml = Regex.Replace(newLayoutHtml, pRenderBody, enlacesHtml);
            newTecnologíasHtml = Regex.Replace(newLayoutHtml, pRenderBody, tecnologiasHtml);

            Console.WriteLine("Generando nuevo HTML...");
            File.WriteAllText($"{pathResultado}/index.html", newIndexHtml);
            File.WriteAllText($"{pathResultado}/enlaces.html", newEnlacesHtml);
            File.WriteAllText($"{pathResultado}/tecnologias.html", newTecnologíasHtml);

            Console.WriteLine("La traducción se ha completado exitosamente!");
            string i = Console.ReadLine();
        }
    }
}
