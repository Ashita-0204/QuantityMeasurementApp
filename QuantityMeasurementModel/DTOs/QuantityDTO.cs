namespace QuantityMeasurementModel.DTOs
{
    public class QuantityDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public QuantityDTO() { }
        //the params which will be sent as a req
        public QuantityDTO(double value, string unit, string category)
        {
            Value = value;
            Unit = unit;
            Category = category;
        }

        public override string ToString() => $"{Value} {Unit} [{Category}]";
    }

}