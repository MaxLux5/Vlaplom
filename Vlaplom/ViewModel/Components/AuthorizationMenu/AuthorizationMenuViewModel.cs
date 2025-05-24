using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using Vlaplom.DataBaseConnector;

namespace Vlaplom.ViewModel.Components.AuthorizationMenu
{
    /// <summary>
    /// ViewModel компонента меню авторизации.
    /// </summary>
    public partial class AuthorizationMenuViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _login;
        [ObservableProperty]
        private string _password;

        /// <summary>
        /// Событие, вызывающееся при успешной авторизации.
        /// </summary>
        public event EventHandler LoginCompleted;


        [RelayCommand]
        private void TryLogin()
        {
            if (!DataBase.GetInstance().Login(Login, Password)) return;

            OnLoginCompleted(this, EventArgs.Empty);
        }
        private void OnLoginCompleted(object? sender, EventArgs e)
        {
            LoginCompleted?.Invoke(sender, e);
        }
    }
}
