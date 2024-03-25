using Microsoft.Extensions.Logging;
using Calculator.Controllers;
using Calculator.DataAccess;
using Calculator.Models;

namespace Calculator.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ICalculatorRepository _calculatorRepository;
        private readonly ILogger<CalculatorService> _logger;

        public CalculatorService(ICalculatorRepository calculationRepository, ILogger<CalculatorService> logger)
        {
            _calculatorRepository = calculationRepository;
            _logger = logger;
        }

        public decimal CalculateExpression(string expression, bool returnIntegerOnly)
        {
            try
            {
                string[] elements = expression.Split(new char[] { '+', '-', '*', '/' }, StringSplitOptions.RemoveEmptyEntries);
                decimal number1 = decimal.Parse(elements[0]);
                decimal number2 = decimal.Parse(elements[1]);

                if (elements.Length != 2)
                {
                    throw new ArgumentException("Expression must contain exactly two operands and one operator");
                }

                string operation = expression.Replace(number1.ToString(), "").Replace(number2.ToString(), "").Trim();

                decimal result;

                switch (operation)
                {
                    case "+":
                        result = number1 + number2;
                        break;
                    case "-":
                        result = number1 - number2;
                        break;
                    case "*":
                        result = number1 * number2;
                        break;
                    case "/":
                        if (number2 == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by zero");
                        }
                        result = number1 / number2;
                        break;
                    default:
                        throw new ArgumentException("Unsupported operation");
                }

                if (returnIntegerOnly)
                {
                    result = Math.Truncate(result);
                }

                return result;
            }
            catch (Exception ex)
            {
                SendError(ex);
                throw new InvalidOperationException("An error occurred while evaluating the expression", ex);
            }
        }


        public void AddToHistory(string expression, decimal result)
        {
            try
            {
                string[] elements = expression.Split(new char[] { '+', '-', '*', '/' });
                decimal number1 = decimal.Parse(elements[0]);
                decimal number2 = decimal.Parse(elements[1]);
                string operation = expression.Replace(number1.ToString(), "").Replace(number2.ToString(), "").Trim();

                // Save record  to DB
                _calculatorRepository.SaveOperation(new CalculatorOperation()
                {
                    Number1 = number1,
                    Number2 = number2,
                    Operation = operation,
                    Result = result
                });
            }
            catch (Exception ex)
            {
                SendError(ex);
                throw; // Throwing the exception above for processing in the controller
            }
        }

        public List<string> GetOperationHistory()
        {
            try
            {
                // Use GetLastOperations from repository to get history of operations from DB
                var operations = _calculatorRepository.GetLastOperations(10);
                List<string> operationHistory = new List<string>();
                foreach (var operation in operations)
                {
                    operationHistory.Add($"{operation.Number1} {operation.Operation} {operation.Number2} = {operation.Result}");
                }
                return operationHistory;
            }
            catch (Exception ex)
            {
                SendError(ex);
                return new List<string>(); // return empty operations list
            }
        }

        public void SendError(Exception exception)
        {
            // Add error message to logs to file
            _logger.LogError(exception, "An error occurred: {ErrorMessage}", exception.Message);
        }
    }
}