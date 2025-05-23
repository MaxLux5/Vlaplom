using Vlaplom.DataBaseConnector;
using Vlaplom.ViewModel.Components.Helpers;
using Vlaplom.ViewModel.Components.Helpers.Enums;

namespace Vlaplom.ViewModel.Components.RequestMenu.Helpers
{
    /// <summary>
    /// Singleton-класс для генерации случайных заявок из компонентов.
    /// </summary>
    public class RandomRequestGenerator
    {
        private static RandomRequestGenerator _instance;
        private List<MaterialViewModel> materials = new List<MaterialViewModel>(DataBase.GetInstance().GetMaterialCollection());
        // Поле WorkTypes сделано словарем и сразу инициализировано для, возможно, последующей доработки, где это поле
        // в дальнейшем могло бы получать из базы данных динамический список <Dictionary<int, string>.
        private Dictionary<int, string> WorkTypes = new Dictionary<int, string>()
        {
            {0, "Выгрузка на склад"},
            {1, "Выгрузка со склада"},
            {2, "Доставка в цех тестирования"},
            {3, "Доставка в цех литья"},
            {4, "Доставка в цех сборки"}
        };


        private RandomRequestGenerator() { }


        public static RandomRequestGenerator GetInstance()
        {
            return _instance ??= new RandomRequestGenerator();
        }

        public RequestViewModel GetRandomRequest()
        {
            Random random = new Random();

            var material = materials[random.Next(0, materials.Count)];
            // Случаное заполнение переменной requiredQuantity ниже в дальнейшем может быть переработано.
            int requiredQuantity;
            switch (material.MeasurementUnit)
            {
                case "шт":
                    requiredQuantity = random.Next(5, 26) * 10;
                    break;

                case "м²":
                    requiredQuantity = random.Next(10, 81) * 10;
                    break;

                case "кг":
                    requiredQuantity = random.Next(1, 11) * 10;
                    break;

                default:
                    requiredQuantity = 10;
                    break;
            }
            string workType = WorkTypes[random.Next(0, WorkTypes.Count)];
            // Случаное заполнение переменной timeToComplete ниже в дальнейшем может быть переработано.
            string timeToComplete = $"{random.Next(1, 6)}ч";

            // В поля id, executor и status устанавливаются неправильные значения, так как при создании заявки требуется их указать,
            // но в дальнейшем они использоваться не будут у этой заявки.
            return new RequestViewModel(0, material.Id, material.Name, material.StockQuantity, material.MeasurementUnit,
                requiredQuantity, workType, timeToComplete, -1, null, RequestStatus.Received);
        }
    }
}
