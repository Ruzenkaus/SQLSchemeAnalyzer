using hz.Classes;
using hz.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace hz.Controllers
{
    public class HomeController : Controller
    {
        private SQLSchemeAnalyzer _analyzer;

        public HomeController(SQLSchemeAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AnalyzeSQL(string scheme)
        {
            _analyzer._Scheme = scheme;
            var (columnsOuter, columnsInner) = _analyzer.SplittingScheme(scheme);
            var analyzed = new SchemeModel(columnsOuter, columnsInner);

            TempData["SchemeModel"] = JsonConvert.SerializeObject(analyzed);

            return View(nameof(Index), analyzed);
        }

        [HttpPost]
        public ActionResult ProvideMethods(string scheme)
        {
            List<string> methods_q = new List<string> { "SELECT", "HAVING", "COUNT", "JOIN" };
            var (columnsOuter, columnsInner) = _analyzer.SplittingScheme(scheme);
            var Props_Tochoose = new List<string>();
            Props_Tochoose.AddRange(columnsOuter.Keys);
            Props_Tochoose.AddRange(columnsInner.Keys);
            var model = new SchemeModel(columnsOuter, columnsInner)
            {
                Methods = methods_q,
                Properties = Props_Tochoose
            };

            TempData["SchemeModel"] = JsonConvert.SerializeObject(model);

            return View(nameof(Index), model);
        }

        [HttpPost]
        public ActionResult GenerateQuery(string selectedMethod, string selectedProperty, string condition)
        {
            var tempSchemeJson = TempData["SchemeModel"] as string;
            var tempScheme = JsonConvert.DeserializeObject<SchemeModel>(tempSchemeJson);

            var queryTemplate = GenerateQueryTemplate(selectedMethod, selectedProperty, condition);
            var model = new SchemeModel(tempScheme.ColumnsOuter, tempScheme.ColumnsInner)
            {
                Methods = tempScheme.Methods,
                Properties = tempScheme.Properties,
                SelectedMethod = selectedMethod,
                SelectedProperty = selectedProperty,
                QueryTemplate = queryTemplate,
                Condition = condition
            };

            TempData["SchemeModel"] = JsonConvert.SerializeObject(model);

            return View(nameof(Index), model);
        }

        private string GenerateQueryTemplate(string method, string property, string condition)
        {
            string queryTemplate;
            switch (method)
            {
                case "SELECT":
                    queryTemplate = $"{method} {property} FROM YourTable";
                    break;
                case "HAVING":
                    queryTemplate = $"SELECT {property} FROM YourTable HAVING {condition}";
                    break;
                case "COUNT":
                    queryTemplate = $"SELECT COUNT({property}) FROM YourTable";
                    break;
                case "JOIN":
                    queryTemplate = $"SELECT {property} FROM YourTable JOIN AnotherTable ON YourTable.{property} = AnotherTable.{property}";
                    break;
                default:
                    queryTemplate = "No valid method selected";
                    break;
            }
            return queryTemplate;
        }
    }
}
