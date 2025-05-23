using CommunityToolkit.Mvvm.ComponentModel;
using Vlaplom.EventArguments;

namespace Vlaplom.ViewModel.Components.RequestMenu
{
    /// <summary>
    /// ViewModel компонента меню заявок.
    /// </summary>
    public partial class RequestMenuComponentViewModel : ObservableObject
    {
        [ObservableProperty]
        private RequestTableComponentViewModel _requestTableViewModel;
        [ObservableProperty]
        private RequestViewerComponentViewModel _requestViewerViewModel;

        public RequestMenuComponentViewModel()
        {
            RequestTableViewModel = new RequestTableComponentViewModel();
            RequestTableViewModel.SelectedRequestChanged += SelectedRequestChanged;

            RequestViewerViewModel = new RequestViewerComponentViewModel();
            RequestViewerViewModel.ChangeSaved += ChangeSaved;
        }


        // Простая но рабочая связь между VM таблицы и VM обозревателя.
        private void SelectedRequestChanged(object? sender, RequestEventArgs e)
        {
            RequestViewerViewModel.SetNewSelectedRequest(e.Request);
        }
        private void ChangeSaved(object? sender, RequestEventArgs e)
        {
            RequestTableViewModel.ChangeSelectedRequest(e.Request);
        }
    }
}