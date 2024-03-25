using Calculator.Models;

namespace Calculator.DataAccess
{
    public interface ICalculatorRepository
    {
        void SaveOperation(CalculatorOperation operation);
        IEnumerable<CalculatorOperation> GetLastOperations(int count);
    }
}
