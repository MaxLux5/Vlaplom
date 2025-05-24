using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Vlaplom.DataBaseConnector;
using Vlaplom.Services.Abstractions;
using Vlaplom.ViewModel.Components.Helpers;

namespace Vlaplom.ViewModel.Dialogs.MaterialMenu
{
    /// <summary>
    /// ViewModel компонента меню материалов.
    /// </summary>
    public partial class OrderMaterialDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private MaterialViewModel _selectedMaterial;
        [ObservableProperty]
        private int _minValue;
        [ObservableProperty]
        private int _maxValue;
        [ObservableProperty]
        private int _value;

        private IDialog _dialog;


        public ObservableCollection<MaterialViewModel> Materials { get; private set; } = new ObservableCollection<MaterialViewModel>();


        public OrderMaterialDialogViewModel(IDialog dialog)
        {
            _dialog = dialog;
        }


        [RelayCommand]
        private void LoadData()
        {
            var collection = DataBase.GetInstance().GetMaterialCollection();
            if (collection is null) return;

            Materials.Clear();
            foreach (var item in collection)
            {
                Materials.Add(item);
            }
        }
        [RelayCommand]
        private void OrderMaterial()
        {
            if(SelectedMaterial is null)
            {
                MessageBox.Show("Не выбран материал.", "Ошибка заказа!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Value < MinValue || Value > MaxValue)
            {
                MessageBox.Show("Указано неверное количество.", "Ошибка заказа!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // Временная копия нужна вместо SelectedMaterial для того, чтобы при неудачном
            // обновлении базы данных не осталось прошлой ошибочной замены SelectedMaterial.
            //var copy = new MaterialViewModel(SelectedMaterial.Id, SelectedMaterial.Name,
            //    SelectedMaterial.StockQuantity + Value, SelectedMaterial.MeasurementUnit);
            if (!DataBase.GetInstance().ChangeMaterialStockQuantity(SelectedMaterial, Value)) return;

            _dialog.CloseDialog();
        }
        partial void OnSelectedMaterialChanged(MaterialViewModel value)
        {
            switch (value.MeasurementUnit)
            {
                case "шт":
                    MinValue = 500;
                    MaxValue = 1000;
                    break;

                case "м²":
                    MinValue = 1000;
                    MaxValue = 3000;
                    break;

                case "кг":
                    MinValue = 100;
                    MaxValue = 700;
                    break;
            }
            Value = MinValue;
        }
    }
}
