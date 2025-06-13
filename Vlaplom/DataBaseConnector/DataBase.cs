using Dapper;
using MySqlConnector;
using System.Windows;
using System.Windows.Media.Media3D;
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

        public IEnumerable<RequestViewModel>? GetRequestCollection()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
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
                catch (Exception)
                {
                    MessageBox.Show("Не удалось загрузить данные.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
        public IEnumerable<ExecutorViewModel>? GetExecutorCollection()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    var queryString =
                    """
                    select id as Id,
                    executor_name as Name
                    from executors
                    """;

                    return connection.Query<ExecutorViewModel>(queryString);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось загрузить данные.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
        public IEnumerable<MaterialViewModel>? GetMaterialCollection()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
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
                catch (Exception)
                {
                    MessageBox.Show("Не удалось загрузить данные.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        /// <summary>
        /// Изменяет в базе данных исполнителя и статус заданной заявки.
        /// В случае ошибки при попытке изменения данных появится сообщение об ошибке.
        /// </summary>
        /// <param name="updatedRequest">Обновленная заявка.</param>
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
                    MessageBox.Show("Не удалось обновить заявку.", "Ошибка изменения!", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// <param name="newRequest">Новая заявка.</param>
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
        /// <param name="changeableMaterial">Изменяемый материал.</param>
        /// <param name="changingQuantity">Число, меняющее количество StockQuantity у материала.</param>
        /// <returns>Возвращает true, если изменение данных прошло успешно, иначе возвращает false.</returns>
        public bool ChangeMaterialStockQuantity(MaterialViewModel changeableMaterial, int changingQuantity)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    if (changeableMaterial.StockQuantity + changingQuantity < 0) return false;

                    var queryString =
                    $"""
                    update materials
                    set materials.stock_quantity = '{changeableMaterial.StockQuantity + changingQuantity}'
                    where materials.id = '{changeableMaterial.Id}'
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
        /// <summary>
        /// Проверяет есть ли пользователь в базе данных.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Возвращает true, если пользователь есть в базе данных, иначе возвращает false.</returns>
        public bool Login(string login, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    var queryString =
                    $"""
                    select count(*) from auths
                    where auths.login = '{login}'
                    and auths.password = '{password}'
                    """;

                    var result = connection.ExecuteScalar<int>(queryString);
                    if (result > 0) return true;
                    else return CallErrorMessage();
                }
                catch (Exception)
                {
                    return CallErrorMessage();
                }
            }

            bool CallErrorMessage()
            {
                MessageBox.Show("Такого пользователя не существует.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
