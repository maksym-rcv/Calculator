namespace Calculator.Models
{
    public class CalculatorOperation
    {
        public int Id { get; set; }
        public decimal Number1 { get; set; }
        public decimal Number2 { get; set; }
        public string? Operation { get; set; }
        public decimal Result { get; set; }
    }
}
