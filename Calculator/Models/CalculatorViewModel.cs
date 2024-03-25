namespace Calculator.Models
{
    public class CalculatorViewModel
    {
        public decimal? Number1 { get; set; }
        public decimal? Number2 { get; set; }
        public string Operation { get; set; }
        public decimal? Result { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> OperationHistory { get; set; }

        public CalculatorViewModel()
        {
            OperationHistory = new List<string>();
        }
    }
}
