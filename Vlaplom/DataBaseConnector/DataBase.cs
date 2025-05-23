using Dapper;
using MySqlConnector;
using System.Net;
using System.Windows;
using Vlaplom.ViewModel.Components.Helpers;

namespace Vlaplom.DataBaseConnector
{
    /// <summary>
    /// Singleton-класс для обращения в базу данных.
    /// </summary>
    public class DataBase
    {
        private static DataBase _instance;
        private const string _connectionString = "Server=localhost; Username=root; Password=24681357; Database=Warehouse";


        private DataBase() { }


        public static DataBase GetInstance()
        {
            return _instance ??= new DataBase();
        }

        public IEnumerable<RequestViewModel> GetRequestCollection()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                var queryString =
                    """
                    select requests.id as Id,
                    materials.id as MaterialId,
                    materials.material_name as MaterialName,
                    materials.stock_quantity as MaterialStockQuantity,
                    materials.measurement_unit as materialMeasurementUnit,
                    requests.required_quantity as RequiredQuantity,
                    requests.work_type as WorkType,
                    requests.time_to_complete as TimeToComplete,
                    executors.id as ExecutorId,
                    executors.executor_name as ExecutorName,
                    requests.request_status as Status
                    from requests
                    left join materials
                    ON requests.material_id = materials.id
                    left join executors
                    ON requests.executor_id = executors.id
                    """;

                return connection.Query<RequestViewModel>(queryString);
            }
        }
        public IEnumerable<ExecutorViewModel> GetExecutorCollection()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                var queryString =
                    """
                    select id as Id,
                    executor_name as Name
                    from executors
                    """;

                return connection.Query<ExecutorViewModel>(queryString);
            }
        }
        public IEnumerable<MaterialViewModel> GetMaterialCollection()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                var queryString =
                    """
                    select id as Id,
                    material_name as Name,
                    stock_quantity as StockQuantity,
                    measurement_unit as MeasurementUnit
                    from materials
                    """;

                return connection.Query<MaterialViewModel>(queryString);
            }
        }
        /// <summary>
        /// Изменяет в базе данных исполнителя и статус заданной заявки.
        /// В случае ошибки при попытке изменения данных появится сообщение об ошибке.
        /// </summary>
        /// <param name="updatedRequest"></param>
        /// <returns>Возвращает true, если изменение данных прошло успешно, иначе возвращает false.</returns>
        public bool ChangeRequest(RequestViewModel updatedRequest)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    var queryString =
                    $"""
                    update requests
                    set requests.executor_id = '{updatedRequest?.Executor.Id}',
                    requests.request_status = '{(int)updatedRequest?.Status}'
                    where requests.id = '{updatedRequest?.Id}'
                    """;

                    connection.Execute(queryString);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось обновить заявку.", "Ошибка сохранения!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return true;
            }
        }
        /// <summary>
        /// Добавляет в базу данных заданную заявку без Id, Status'а и Executor.Id. Но в базе данных
        /// поля Id и Status устанавливаются самостоятельно по умолчанию, а поле Executor.Id остается пустым.
        /// В случае ошибки при попытке добавления данных появится сообщение об ошибке.
        /// </summary>
        /// <param name="newRequest"></param>
        /// <returns>Возвращает true, если добавление данных прошло успешно, иначе возвращает false.</returns>
        public bool AddNewRequest(RequestViewModel newRequest)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    var queryString =
                    $"""
                    insert into requests(required_quantity, work_type,
                    time_to_complete, material_id)
                    values('{newRequest.RequiredQuantity}', '{newRequest.WorkType}',
                    '{newRequest.TimeToComplete}', '{newRequest.Material.Id}')
                    """;

                    connection.Query<RequestViewModel>(queryString);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось добавить новую заявку.", "Ошибка сохранения!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return true;
            }
        }
        /// <summary>
        /// Изменяет в базе данных количество запасов заданного материала.
        /// В случае ошибки при попытке изменения данных появится сообщение об ошибке.
        /// </summary>
        /// <param name="changedMaterial"></param>
        /// <param name="ChangeableQuantity"></param>
        /// <returns>Возвращает true, если изменение данных прошло успешно, иначе возвращает false.</returns>
        public bool ChangeMaterialStockQuantity(MaterialViewModel changedMaterial, int ChangeableQuantity)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    if (changedMaterial.StockQuantity + ChangeableQuantity < 0) return false;

                    var queryString =
                    $"""
                    update materials
                    set materials.stock_quantity = '{changedMaterial.StockQuantity + ChangeableQuantity}'
                    where materials.id = '{changedMaterial.Id}'
                    """;

                    connection.Execute(queryString);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось обновить данные материала.", "Ошибка сохранения!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return true;
            }
        }
    }
}
