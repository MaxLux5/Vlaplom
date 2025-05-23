using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
using Vlaplom.Services.Abstractions;
using Vlaplom.View.Screens;
using Vlaplom.ViewModel.Screens;

namespace Vlaplom.ViewModel.Windows
{
    /// <summary>
    /// ViewModel главного окна.
    /// </summary>
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserControl _mainScreen;


        public MainWindowViewModel(IDialogService dialogService)
        {
            // TODO: Исправить нарушение MVVM.
            MainScreen = new MenuScreen();
            MainScreen.DataContext = new MenuScreenViewModel(dialogService);
        }
    }
}
