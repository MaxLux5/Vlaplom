using Vlaplom.Services.Abstractions;
using Vlaplom.View.Dialogs.MaterialMenu;
using Vlaplom.ViewModel.Dialogs.MaterialMenu;

namespace Vlaplom.Services
{
    public class DialogService : IDialogService
    {
        private static DialogService _instance;


        private DialogService() { }


        public static DialogService GetInstance()
        {
            return _instance ??= new DialogService();
        }

        public void ShowDialog()
        {
            var dialog = new OrderMaterialDialog();
            dialog.DataContext = new OrderMaterialDialogViewModel(dialog);
            dialog.OpenDialog();
        }
    }
}
