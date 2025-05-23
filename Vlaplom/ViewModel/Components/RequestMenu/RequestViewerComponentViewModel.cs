using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Vlaplom.DataBaseConnector;
using Vlaplom.EventArguments;
using Vlaplom.ViewModel.Components.Helpers;
using Vlaplom.ViewModel.Components.Helpers.Enums;

namespace Vlaplom.ViewModel.Components.RequestMenu
{
    /// <summary>
    /// ViewModel компонента обозревателя заявки.
    /// </summary>
    public partial class RequestViewerComponentViewModel : ObservableObject
    {
        [ObservableProperty]
        private RequestViewModel _selectedRequest;


        public ObservableCollection<ExecutorViewModel> Executors { get; set; } = new ObservableCollection<ExecutorViewModel>();

        public event EventHandler<RequestEventArgs> ChangeSaved;

        [RelayCommand]
        private void LoadData()
        {
            Executors.Clear();
            foreach (var item in DataBase.GetInstance().GetExecutorCollection())
            {
                Executors.Add(item);
            }
        }
        [RelayCommand]
        private void SaveRequest(TimeSpan timeSpan)
        {
            OnSave(timeSpan);
        }
        private async void OnSave(TimeSpan timeSpan)
        {
            // Для корректной работы метода необходима временная копия SelectedRequest, чтобы, при изменении
            // этого свойства пользователем, использовалось изначальное значение.
            var tempSelectedRequest = new RequestViewModel(SelectedRequest, SelectedRequest.Executor);

            if (!DataBase.GetInstance().ChangeRequest(tempSelectedRequest)) return;
            
            await Task.Delay(timeSpan);

            if (!DataBase.GetInstance().ChangeMaterialStockQuantity(SelectedRequest.Material, SelectedRequest.RequiredQuantity))
            {
                tempSelectedRequest.Status = RequestStatus.Blocked;
                DataBase.GetInstance().ChangeRequest(tempSelectedRequest);

                return;
            }

            tempSelectedRequest.Status = RequestStatus.Done;
            ChangeSaved?.Invoke(this, new RequestEventArgs(tempSelectedRequest));
        }


        // Для корректной работы метода необходимо именно копирование данных, а не передача по ссылке, иначе,
        // при изменении ComboBox'а, данные сразу поменяются в таблице без использования кнопки сохранения.
        public void SetNewSelectedRequest(RequestViewModel request)
        {
            // Необходимо для того, чтобы в случае, когда выделение в таблице слетело, в обозревателе ничего не было выбрано.
            if (request is null)
            {
                SelectedRequest = null;
                return;
            }
            // Необходимо в случае, когда исполнитель еще не назначен.
            if (request.Executor is not null && request.Executor?.Id <= 0)
            {
                // TODO: Исправить баг, при котором в comboBox'е остается выбранный исполнитель, даже если в новой выбранной заявке он отсутствует.
                SelectedRequest = new RequestViewModel(request, request.Executor);
                return;
            }

            // Необходимо использовать элемент конкретно из коллекции "Executors", иначе
            // экземпляр исполнителя будет считаться другим объектом, хотя он может быть идентичен
            // элементу коллекции "Executors".
            var Executor = Executors[request.Executor.Id - 1];
            SelectedRequest = new RequestViewModel(request, Executor);
        }
    }
}
