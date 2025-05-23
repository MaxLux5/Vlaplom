using CommunityToolkit.Mvvm.ComponentModel;
using Vlaplom.Services.Abstractions;
using Vlaplom.ViewModel.Components.MaterialMenu;
using Vlaplom.ViewModel.Components.RequestMenu;

namespace Vlaplom.ViewModel.Screens
{
    /// <summary>
    /// ViewModel скрина меню.
    /// </summary>
    public partial class MenuScreenViewModel : ObservableObject
    {
        [ObservableProperty]
        private RequestMenuComponentViewModel _requestMenuViewModel;
        [ObservableProperty]
        private MaterialMenuComponentViewModel _materialMenuViewModel;


        public MenuScreenViewModel(IDialogService dialogService)
        {
            RequestMenuViewModel = new RequestMenuComponentViewModel();
            MaterialMenuViewModel = new MaterialMenuComponentViewModel(dialogService);
        }
    }
}
