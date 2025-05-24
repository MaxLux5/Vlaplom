using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
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
            var collection = DataBase.GetInstance().GetExecutorCollection();
            if (collection is null) return;

            Executors.Clear();
            foreach (var item in collection)
            {
                Executors.Add(item);
            }
        }
        [RelayCommand]
        private void SaveRequest(TimeSpan timeSpan)
        {
            if(SelectedRequest is null)
            {
                MessageBox.Show("Выберите заявку.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Если Executor.Id <= 0 или null, то испольнитель не назначен.
            if (SelectedRequest.Executor is null || SelectedRequest.Executor.Id <= 0)
            {
                MessageBox.Show("Исполнитель не выбран. Нельзя начать выполнять заявку.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedRequest.Status != RequestStatus.InProgress)
                MessageBox.Show($"Если заявке не назначить статус \"В процессе\", то она не будет выполняться.", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Information);


            if (SelectedRequest.Status == RequestStatus.InProgress) CompleteRequest(timeSpan);
            else
            {
                DataBase.GetInstance().ChangeRequest(SelectedRequest);

                ChangeSaved?.Invoke(this, new RequestEventArgs(SelectedRequest));
            }
        }
        private async void CompleteRequest(TimeSpan timeSpan)
        {
            // Для корректной работы метода необходима временная копия SelectedRequest, чтобы, при изменении
            // этого свойства пользователем, использовалось изначальное значение.
            var tempSelectedRequest = new RequestViewModel(SelectedRequest, SelectedRequest.Executor);

            // Сразу устанаваливаются новые исполнитель и статус у заявки.
            if (!DataBase.GetInstance().ChangeRequest(tempSelectedRequest)) return;
            ChangeSaved?.Invoke(this, new RequestEventArgs(tempSelectedRequest));

            await Task.Delay(timeSpan);

            // При выполнение заявки, количество материала со склада должно вычитаться,
            // поэтому SelectedRequest.RequiredQuantity должен быть отрицательным.
            if (!DataBase.GetInstance().ChangeMaterialStockQuantity(SelectedRequest.Material, -SelectedRequest.RequiredQuantity))
            {
                // Устанавливается статус заявки "Блокирована".
                tempSelectedRequest.Status = RequestStatus.Blocked;
                if (!DataBase.GetInstance().ChangeRequest(tempSelectedRequest)) return;
                ChangeSaved?.Invoke(this, new RequestEventArgs(tempSelectedRequest));

                return;
            }

            // Устанавливается статус заявки "Выполнена".
            tempSelectedRequest.Status = RequestStatus.Done;
            if (!DataBase.GetInstance().ChangeRequest(tempSelectedRequest)) return;
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