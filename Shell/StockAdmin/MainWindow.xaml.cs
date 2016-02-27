using System.Windows;
using StockAdmin.ViewModel;
using StockAdmin.Views;

namespace StockAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Customers c = new Customers();
            c.Show();
        }

        private void ButtonDemoBusquedaYModificacion_Click(object sender, RoutedEventArgs e)
        {
            Demo2View c = new Demo2View();
            c.Show();
        }

        private void ButtonBatchProcessing_Click(object sender, RoutedEventArgs e)
        {
            DemoBatchProcess d = new DemoBatchProcess();
            d.Show();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemAutor_Click(object sender, RoutedEventArgs e)
        {
            new Author().Show();
        }

        
    }
}