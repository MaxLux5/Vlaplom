using System.Windows;
using Vlaplom.Services;
using Vlaplom.View.Windows;
using Vlaplom.ViewModel.Windows;

namespace Vlaplom;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        MainWindow mainWindow = new MainWindow();
        mainWindow.DataContext = new MainWindowViewModel(DialogService.GetInstance());
        mainWindow.Show();
        
    }
}