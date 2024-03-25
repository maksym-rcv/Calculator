using Xunit;
using Moq;
using Calculator.Services;
using Calculator.DataAccess;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Logging;

namespace CalculatorTests
{
    public class CalculatorServiceTests
    {
        [Theory]
        [InlineData("2+3", 5)]
        [InlineData("5-3", 2)]
        [InlineData("2*3", 6)]
        [InlineData("6/3", 2)]
        public void CalculateExpression_ReturnsCorrectResult(string expression, decimal expectedResult)
        {
            var loggerMock = new Mock<ILogger<CalculatorService>>();

            var dbContextOptions = new DbContextOptionsBuilder<CalculatorDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new CalculatorDbContext(dbContextOptions);

            var calculatorRepository = new CalculatorRepository(dbContext);

            var calculatorService = new CalculatorService(calculatorRepository, loggerMock.Object);

            var result = calculatorService.CalculateExpression(expression, false);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("5/3", 1)] // Only an integer should be returned
        public void CalculateExpression_WithIntegerOnlyOption_ReturnsIntegerResult(string expression, decimal expectedResult)
        {
            var loggerMock = new Mock<ILogger<CalculatorService>>();

            var dbContextOptions = new DbContextOptionsBuilder<CalculatorDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new CalculatorDbContext(dbContextOptions);

            var calculatorRepository = new CalculatorRepository(dbContext);

            var calculatorService = new CalculatorService(calculatorRepository, loggerMock.Object);

            var result = calculatorService.CalculateExpression(expression, true);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CalculateExpression_WithInvalidOperation_ThrowsException()
        {
            var loggerMock = new Mock<ILogger<CalculatorService>>();

            var dbContextOptions = new DbContextOptionsBuilder<CalculatorDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new CalculatorDbContext(dbContextOptions);

            var calculatorRepository = new CalculatorRepository(dbContext);

            var calculatorService = new CalculatorService(calculatorRepository, loggerMock.Object);
            decimal number1 = 5;
            decimal number2 = 3;
            string unsupportedOperation = "%";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => calculatorService.CalculateExpression(number1 + unsupportedOperation + number2, false));
        }

    }
}