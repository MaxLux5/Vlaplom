using System;

namespace Vlaplom.ViewModel.Components.Helpers
{
    /// <summary>
    /// Базовый класс, представляющий собой модель материала.
    /// </summary>
    public class MaterialViewModel
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public string MeasurementUnit { get; set; }


        public MaterialViewModel(string id, string name, int stockQuantity, string measurementUnit)
        {
            Id = id;
            Name = name;
            StockQuantity = stockQuantity;
            MeasurementUnit = measurementUnit;
        }
    }
}
