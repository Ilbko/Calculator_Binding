using Calculator_Binding.Model;
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
        //ВьюМодель, содержащая в себе наблюдаемые поля и логика калькулятора
        private CalcViewModel ViewModel;
        private CalcModel Model;
        public MainWindow()
        {
            //Можно передавать вьюМодель в конструктор вместо передачи её в методы
            //Режим работы калькулятора
            if (MessageBox.Show("Включить кнопку \"Равно\"?", "Режим", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Model = new CalcModel(true);
            else
                this.Model = new CalcModel(false);

            InitializeComponent();
            this.DataContext = this.ViewModel = new CalcViewModel();
            //this.Model = new CalcModel();
        }

        //Работа калькулятора по нажатию на его кнопки
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Model.calculateMouse(ViewModel, sender as Button);
        }

        //Работа калькулятора по нажатию на кнопки клавиатуры
        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            this.Model.calculateKey(ViewModel, e);
        }

        //Реализация стирания примера с помощью бэкспейса
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Model.Erase(ViewModel, e);
        }
    }
}
