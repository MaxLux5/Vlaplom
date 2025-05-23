using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Vlaplom.DataBaseConnector;
using Vlaplom.EventArguments;
using Vlaplom.ViewModel.Components.Helpers;
using Vlaplom.ViewModel.Components.RequestMenu.Helpers;

namespace Vlaplom.ViewModel.Components.RequestMenu
{
    /// <summary>
    /// ViewModel компонента таблицы заявок.
    /// </summary>
    public partial class RequestTableComponentViewModel : ObservableObject
    {
        [ObservableProperty]
        private RequestViewModel _selectedRequest;


        public ObservableCollection<RequestViewModel> Requests { get; set; } = new ObservableCollection<RequestViewModel>();

        public event EventHandler<RequestEventArgs> SelectedRequestChanged;

        [RelayCommand]
        private void LoadData()
        {
            Requests.Clear();
            foreach (var item in DataBase.GetInstance().GetRequestCollection())
            {
                Requests.Add(item);
            }
        }
        [RelayCommand]
        private void GetRequest()
        {
            var newRequest = RandomRequestGenerator.GetInstance().GetRandomRequest();
            if (!DataBase.GetInstance().AddNewRequest(newRequest)) return;

            LoadData();
        }
        partial void OnSelectedRequestChanged(RequestViewModel value)
            => SelectedRequestChanged?.Invoke(this, new RequestEventArgs(value));


        /// <summary>
        /// Вносит изменения в таблицу.
        /// </summary>
        public void ChangeSelectedRequest(RequestViewModel request)
        {
            // Костыльная попытка найти индекс полученного пользователя.
            var index = Requests.IndexOf(Requests.FirstOrDefault(x => x.Id == request.Id));

            // Индекс может быть -1, если IndexOf не нашел указанного пользователя.
            if (index >= 0)
            {
                Requests[index] = request;
                SelectedRequest = request;
            }
        }
    }
}
