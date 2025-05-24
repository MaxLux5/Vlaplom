using CommunityToolkit.Mvvm.ComponentModel;
using Vlaplom.ViewModel.Components.AuthorizationMenu;

namespace Vlaplom.ViewModel.Screens
{
    /// <summary>
    /// ViewModel скрина меню авторизации.
    /// </summary>
    public partial class AuthorizationScreenViewModel : ObservableObject
    {
        [ObservableProperty]
        private AuthorizationMenuViewModel _authorizationViewModel;

        public AuthorizationScreenViewModel()
        {
            AuthorizationViewModel = new AuthorizationMenuViewModel();
            AuthorizationViewModel.LoginCompleted += OnLoginCompleted;
        }

        /// <summary>
        /// Промежуточное событие, вызывающееся при успешной авторизации.
        /// </summary>
        public event EventHandler LoginCompleted;
        private void OnLoginCompleted(object? sender, EventArgs e)
        {
            LoginCompleted?.Invoke(sender, e);
        }
    }
}
