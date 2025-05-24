using CommunityToolkit.Mvvm.ComponentModel;

namespace Vlaplom.ViewModel.Components.Helpers
{
    /// <summary>
    /// Базовый класс, представляющий собой модель исполнителя.
    /// </summary>
    public partial class ExecutorViewModel : ObservableObject
    {
        public int Id { get; private set; }
        [ObservableProperty]
        private string _name;

        public ExecutorViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
