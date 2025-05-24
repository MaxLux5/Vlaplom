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
            var collection = DataBase.GetInstance().GetRequestCollection();
            if (collection is null) return;

            Requests.Clear();
            foreach (var item in collection)
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
            var asd = SelectedRequest;
            // Костыльная попытка найти индекс полученного пользователя.
            var index = Requests.IndexOf(Requests.FirstOrDefault(x => x.Id == request.Id));

            // Индекс может быть -1, если IndexOf не нашел указанного пользователя.
            if (index >= 0)
            {
                // Присвоение Requests[index] = request может не обновить интерфейс. ObservableCollection<T> отслеживает только
                // структурные изменения — например, добавление или удаление элементов. При замене элемента по индексу он
                // проверяет только ссылку, но не содержимое объекта. Поэтому если вы меняете свойства существующего
                // объекта (который не реализует INotifyPropertyChanged), UI не обновится. 
                //
                // Если же вы присвоите новый объект, то внешне может показаться, что интерфейс отреагировал — WPF может
                // пересоздать элемент из-за смены ссылки. Но это иллюзия: ObservableCollection<T> на самом деле не
                // отправляет уведомление о замене.
                //
                // var request = Requests[index];
                // request.Name = "Новое имя"; // Изменяем свойство (Пример)
                // Requests[index] = request;  // Это НЕ вызовет уведомления, так как объект тот же
                
                Requests.RemoveAt(index);
                Requests.Insert(index, request);
                SelectedRequest = request;
            }
        }
    }
}