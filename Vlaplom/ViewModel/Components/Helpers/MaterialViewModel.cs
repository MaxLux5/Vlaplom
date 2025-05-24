using CommunityToolkit.Mvvm.ComponentModel;

namespace Vlaplom.ViewModel.Components.Helpers
{
    /// <summary>
    /// Базовый класс, представляющий собой модель материала.
    /// </summary>
    public partial class MaterialViewModel : ObservableObject
    {
        public string Id { get; private set; }
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private int _stockQuantity;
        [ObservableProperty]
        private string _measurementUnit;


        public MaterialViewModel(string id, string name, int stockQuantity, string measurementUnit)
        {
            Id = id;
            Name = name;
            StockQuantity = stockQuantity;
            MeasurementUnit = measurementUnit;
        }
    }
}
