using PhoneBookWPF.ViewModels;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace PhoneBookWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindowViewModel viewModel;
        protected override void OnStartup(StartupEventArgs e)
        {
            var cultureInfo = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);

            MainWindow window = new MainWindow();

            viewModel = new();

            window.DataContext = viewModel;

            window.Show();

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            //await viewModel.Context.DisposeAsync();

            viewModel.Dispose();

            base.OnExit(e);
        }
    }

}
