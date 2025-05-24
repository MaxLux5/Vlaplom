using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
using Vlaplom.Services;
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
        private UserControl _currentScreen;

        private IDialogService _dialogService;


        public MainWindowViewModel(IDialogService dialogService)
        {
            // TODO: Исправить нарушение MVVM.
            _dialogService = dialogService;

            CurrentScreen = new AuthorizationScreen();
            var authViewModel = new AuthorizationScreenViewModel();
            CurrentScreen.DataContext = authViewModel;
            authViewModel.LoginCompleted += Login;
        }

        
        /// <summary>
        /// Смена на страницу с меню.
        /// </summary>
        private void Login(object? sender, EventArgs e)
        {
            CurrentScreen = new MenuScreen();
            CurrentScreen.DataContext = new MenuScreenViewModel(_dialogService);
        }
    }
}
