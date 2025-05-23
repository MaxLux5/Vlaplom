using Vlaplom.ViewModel.Components.Helpers.Enums;

namespace Vlaplom.ViewModel.Components.Helpers
{
    /// <summary>
    /// Базовый класс, представляющий собой модель заявки.
    /// </summary>
    public partial class RequestViewModel
    {
        public int Id { get; private set; }
        public MaterialViewModel Material { get; set; }
        public int RequiredQuantity { get; set; }
        public string WorkType { get; set; }
        public string TimeToComplete { get; set; }
        public ExecutorViewModel Executor { get; set; }
        public RequestStatus Status { get; set; }


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
