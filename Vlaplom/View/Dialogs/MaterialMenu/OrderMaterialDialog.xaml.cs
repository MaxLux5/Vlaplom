using System.Windows;
using Vlaplom.Services.Abstractions;

namespace Vlaplom.View.Dialogs.MaterialMenu
{
    /// <summary>
    /// Диалоговое окно для заказа материала.
    /// </summary>
    public partial class OrderMaterialDialog : Window, IDialog
    {
        public OrderMaterialDialog()
        {
            InitializeComponent();
        }

        public void CloseDialog()
        {
            Close();
        }
        public void OpenDialog()
        {
            ShowDialog();
        }
    }
}
