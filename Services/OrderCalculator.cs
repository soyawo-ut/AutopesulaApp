using AutopesulaApp.Models;

namespace AutopesulaApp.Services
{
    public static class OrderCalculator
    {
        public static decimal CalculatePrice(
            VehicleType vehicleType,
            WashProgram washProgram)
        {
            decimal price = washProgram switch
            {
                WashProgram.Basic => 10m,
                WashProgram.Standard => 15m,
                WashProgram.Premium => 22m,
                _ => 0m
            };

            if (vehicleType == VehicleType.SUV)
            {
                price += 3m;
            }
            else if (vehicleType == VehicleType.Van)
            {
                price += 5m;
            }

            return price;
        }

        public static int CalculateDuration(
            VehicleType vehicleType,
            WashProgram washProgram)
        {
            int minutes = washProgram switch
            {
                WashProgram.Basic => 15,
                WashProgram.Standard => 25,
                WashProgram.Premium => 40,
                _ => 0
            };

            if (vehicleType == VehicleType.SUV)
            {
                minutes += 5;
            }
            else if (vehicleType == VehicleType.Van)
            {
                minutes += 10;
            }

            return minutes;
        }
    }
}