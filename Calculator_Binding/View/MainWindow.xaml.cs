using Calculator_Binding.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator_Binding
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ВьюМодель, содержащая в себе наблюдаемые поля
        private CalcViewModel ViewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.ViewModel = new CalcViewModel();
        }
    }
}
