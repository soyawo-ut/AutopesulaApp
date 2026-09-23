namespace AutopesulaApp.Models
{
    public class CarWashOrder
    {
        public string RegistrationNumber { get; set; } = string.Empty;

        public VehicleType VehicleType { get; set; }

        public WashProgram WashProgram { get; set; }

        public decimal Price { get; set; }

        public int DurationMinutes { get; set; }
    }
}