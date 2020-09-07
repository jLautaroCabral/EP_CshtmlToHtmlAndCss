using System;
using System.IO;
using System.Text.RegularExpressions;

namespace traductor
{
    class Program
    {
        static void Main(string[] args)
        {

            // ------------------------------------------------------------------
            // ------------------------------------------------------------------


            // Se establecen las rutas a utilizar
            Console.WriteLine("Comenzando la traducción...");
            string pathAspProject = @"../../../../../asp-project";
            string pathResultado = @"../../../../../resultado_traductor";

            string pathCss = Path.Combine(pathResultado, "css");
            string pathAssets = Path.Combine(pathResultado, "assets");


            // ------------------------------------------------------------------
            // ------------------------------------------------------------------


            // Se crean directorios utilizando algunas de las anteriores rutas
            Console.WriteLine("Creando directorios...");
            if (!File.Exists(pathResultado))
            {
                Directory.CreateDirectory(pathResultado);
                Directory.CreateDirectory(pathCss);
                Directory.CreateDirectory(pathAssets);
                Console.WriteLine("La carpeta resultado_traductor ha sido creada exitosamente!");
            }


            // ------------------------------------------------------------------
            // ------------------------------------------------------------------

            // Se copian los archivos CSS y assets a la ruta resultado
            Console.WriteLine("Copiando ficheros...");
            File.Copy($"{pathAspProject}/wwwroot/css/site.css", $"{pathCss}/site.css", true);
            File.Copy($"{pathAspProject}/wwwroot/assets/cheems.png", $"{pathAssets}/cheems.png", true);

            // Se cran los html en la ruta resultado
            Console.WriteLine("Creando ficheros...");
            FileStream fs;
            fs = File.Create($"{pathResultado}/index.html");
            fs.Close();
            fs = File.Create($"{pathResultado}/tecnologias.html");
            fs.Close();
            fs = File.Create($"{pathResultado}/enlaces.html");
            fs.Close();


            // ------------------------------------------------------------------
            // ------------------------------------------------------------------

            // Se leen los archivos HTML y se guarda su contenido en variables
            Console.WriteLine("Recuperando HTML...");
            string layoutHtml = File.ReadAllText($"{pathAspProject}/Views/Shared/_Layout.cshtml");
            string indexHtml = File.ReadAllText($"{pathAspProject}/Views/Home/Index.cshtml");
            string enlacesHtml = File.ReadAllText($"{pathAspProject}/Views/Home/Enlaces.cshtml");
            string tecnologiasHtml = File.ReadAllText($"{pathAspProject}/Views/Home/Tecnologías.cshtml");


            // ------------------------------------------------------------------
            // ------------------------------------------------------------------

            // Utilizando Expresiones Regulares se crean nuevos strings juntando el HTML del Layout y el HTML de las otras vistas,
            // de la misma manera se reemplazan las directivas de ASP por HTML puro. Luego se guardan en variables strings
            // Las variables de esta sección que empiezan con "p", son los patrones que se buscan al usar RegEx
            Console.WriteLine("Traduciendo HTML...");
            string pRenderBody = @"@RenderBody\(\)";

            // Esta salsa se produjo al tratar de "escapar" carácteres de strings y al mismo tiempo tratar de "escapar" carácteres de RegEx
            string pRenderScripts = @"@RenderSection\(" + $"\"Scripts\"" + @", required: false\)"; 

            string pLinkCss = "~/css/site.css";
            string pCheems = "~/assets/cheems.png";
            string pAspController = $"asp-controller=\"Home\"";
            string pAspActionI = $"asp-action=\"Index\"";
            string pAspActionT = $"asp-action=\"Tecnologías\"";
            string pAspActionE = $"asp-action=\"Enlaces\"";

            string newLayoutHtml;
            string newIndexHtml;
            string newEnlacesHtml;
            string newTecnologíasHtml;

            newLayoutHtml = Regex.Replace(layoutHtml, pLinkCss, "css/site.css");
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspController, String.Empty);
            newLayoutHtml = Regex.Replace(newLayoutHtml, pRenderScripts, String.Empty); 
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspActionI, "href=\"./index.html\"");
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspActionT, "href=\"./tecnologias.html\"");
            newLayoutHtml = Regex.Replace(newLayoutHtml, pAspActionE, "href=\"./enlaces.html\"");

            newIndexHtml = Regex.Replace(newLayoutHtml, pRenderBody, indexHtml);
            newIndexHtml = Regex.Replace(newIndexHtml, pCheems, "assets/cheems.png");

            newEnlacesHtml = Regex.Replace(newLayoutHtml, pRenderBody, enlacesHtml);
            newTecnologíasHtml = Regex.Replace(newLayoutHtml, pRenderBody, tecnologiasHtml);

            // ------------------------------------------------------------------
            // ------------------------------------------------------------------
             
            // Se escribe en los HTML creados anteriormente el contenido de las variables con el HTML ya procesado
            Console.WriteLine("Generando nuevo HTML...");
            File.WriteAllText($"{pathResultado}/index.html", newIndexHtml);
            File.WriteAllText($"{pathResultado}/enlaces.html", newEnlacesHtml);
            File.WriteAllText($"{pathResultado}/tecnologias.html", newTecnologíasHtml);

            Console.WriteLine("La traducción se ha completado exitosamente!");
            Console.WriteLine("============================================");
            Console.WriteLine("");
            Console.WriteLine("Para volver a ejecutar la traduccion tienes que borrar la carpeta generada \"resultado_traductor\" ");
            var i = Console.ReadKey();
        }
    }
}
