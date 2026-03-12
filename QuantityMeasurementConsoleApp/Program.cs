using QuantityMeasurementApp.Models;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository;

#nullable disable

namespace QuantityMeasurementApp
{
    class Program
    {
        public static void Main()
        {
            IMenu menu = new Menu();
            menu.ShowMenu();
        }
    }
}