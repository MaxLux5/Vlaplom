using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Vlaplom.DataBaseConnector;
using Vlaplom.Services.Abstractions;
using Vlaplom.ViewModel.Components.Helpers;

namespace Vlaplom.ViewModel.Components.MaterialMenu
{
    /// <summary>
    /// ViewModel компонента меню материалов.
    /// </summary>
    public partial class MaterialMenuComponentViewModel
    {
        private IDialogService _dialogService;


        public ObservableCollection<MaterialViewModel> Materials { get; private set; } = new ObservableCollection<MaterialViewModel>();


        public MaterialMenuComponentViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
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
            _dialogService.ShowDialog();
            LoadData();
        }
    }
}
