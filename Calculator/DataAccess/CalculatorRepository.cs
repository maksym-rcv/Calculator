using Calculator.Controllers;
using Calculator.Models;
using System.Collections.Generic;
using System.Linq;


namespace Calculator.DataAccess
{
    public class CalculatorRepository : ICalculatorRepository
    {
        private readonly CalculatorDbContext _dbContext;

        public CalculatorRepository(CalculatorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveOperation(CalculatorOperation operation)
        {
            _dbContext.CalculatorOperations.Add(operation);
            _dbContext.SaveChanges();
        }

        public IEnumerable<CalculatorOperation> GetLastOperations(int count)
        {
            return _dbContext.CalculatorOperations
                .OrderByDescending(o => o.Id)
                .Take(count)
                .ToList();
        }
    }
}
