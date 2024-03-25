using Calculator.Models;
using Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICalculatorService _calculatorService;

        public HomeController(ICalculatorService calculatorService, ILogger<HomeController> logger)
        {
            _calculatorService = calculatorService;
            _logger = logger;
        }

        public ActionResult Index()
        {
            var operationHistory = _calculatorService.GetOperationHistory();
            var viewModel = new CalculatorViewModel
            {
                OperationHistory = operationHistory
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Calculate([FromBody] CalculationRequest request)
        {
            string expression = request.Expression;
            try
            {
                // Evaluate operation
                decimal result = _calculatorService.CalculateExpression(expression, false);

                // Add operation to history
                _calculatorService.AddToHistory(expression, result);

                // return JSON result
                return Ok(result);
            }
            catch (Exception ex)
            {
                _calculatorService.SendError(ex);
                return StatusCode(500, ex.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}