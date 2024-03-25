namespace Calculator.Services
{
    public interface ICalculatorService
    {
        decimal CalculateExpression(string expression, bool returnIntegerOnly);
        void AddToHistory(string expression, decimal result);
        List<string> GetOperationHistory();
        void SendError(Exception exception);
    }
}