using CommunityToolkit.Mvvm.ComponentModel;
using Vlaplom.ViewModel.Components.Helpers.Enums;

namespace Vlaplom.ViewModel.Components.Helpers
{
    /// <summary>
    /// Базовый класс, представляющий собой модель заявки.
    /// </summary>
    public partial class RequestViewModel : ObservableObject
    {
        public int Id { get; private set; }
        [ObservableProperty]
        private MaterialViewModel _material;
        [ObservableProperty]
        private int _requiredQuantity;
        [ObservableProperty]
        private string _workType;
        [ObservableProperty]
        private string _timeToComplete;
        [ObservableProperty]
        private ExecutorViewModel _executor;
        [ObservableProperty]
        private RequestStatus _status;


        public RequestViewModel(int id, string materialId, string materialName, int materialStockQuantity,
            string materialMeasurementUnit, int requiredQuantity, string workType,
            string timeToComplete, int executorId, string executorName, RequestStatus status)
        {
            Id = id;
            Material = new MaterialViewModel(materialId, materialName, materialStockQuantity, materialMeasurementUnit);
            RequiredQuantity = requiredQuantity;
            WorkType = workType;
            TimeToComplete = timeToComplete;
            Executor = new ExecutorViewModel(executorId, executorName);
            Status = status;
        }
        public RequestViewModel(RequestViewModel request, ExecutorViewModel executor)
        {
            Id = request.Id;
            Material = request.Material;
            RequiredQuantity = request.RequiredQuantity;
            WorkType = request.WorkType;
            TimeToComplete = request.TimeToComplete;
            Executor = executor;
            Status = request.Status;
        }
    }
}
