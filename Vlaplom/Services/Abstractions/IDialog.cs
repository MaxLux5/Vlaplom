using System;

namespace Vlaplom.Services.Abstractions
{
    /// <summary>
    /// Диалоговое окошко.
    /// </summary>
    public interface IDialog
    {
        void OpenDialog();
        void CloseDialog();
    }
}
