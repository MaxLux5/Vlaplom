using System;

namespace Vlaplom.ViewModel.Components.Helpers
{
    /// <summary>
    /// Базовый класс, представляющий собой модель исполнителя.
    /// </summary>
    public class ExecutorViewModel
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        public ExecutorViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
