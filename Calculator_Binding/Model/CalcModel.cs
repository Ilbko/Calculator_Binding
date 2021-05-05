using Calculator_Binding.ViewModel;
using System;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator_Binding.Model
{
    public class CalcModel
    {
        //private bool usingEquals;
        //Массив чисел и массив операций для работы с калькулятором
        private char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        //Сюда не входит минус, ибо он может стоять в уравнении неограниченное количество раз подряд без каких-либо негативных последствий (см. ниже)
        private char[] operations = new char[] { '+', '/', '*', '=' };

        //Делегаты для режима работы (с кнопкой "равно" или без) вместо того, чтобы каждый раз проверять режим в самом методе и запускать нужный код
        public delegate void CalculateMouse(CalcViewModel ViewModel, Button button);
        public CalculateMouse calculateMouse;
        public delegate void CalculateKey(CalcViewModel ViewModel, TextCompositionEventArgs e);
        public CalculateKey calculateKey;

        //Конструктор, подписывающий нужные методы на делегаты в зависимости от режима работы калькулятора.
        public CalcModel(bool mode)
        {
            //Если кнопка "Равно" будет использоваться
            if (mode)
            {
                calculateMouse = CalculateMouseEquals;
                calculateKey = CalculateKeyEquals;
            }
            else
            {
                calculateMouse = CalculateMouseWEquals;
                calculateKey = CalculateKeyWEquals;
            }
        }
        #region Методы для работы с мышкой
        private void CalculateMouseEquals(CalcViewModel ViewModel, Button button)
        {
            //Режим работы, зависящий от кнопки "Равно"

            //Если была нажата кнопка "Равно"
            if (button.Name == "ButtonEquals")
            {
                try
                {
                    //Неизвестно исключение, выкидываемое Compute
                    ViewModel.ResultProperty = new DataTable().Compute(ViewModel.HistoryProperty, null).ToString();
                }
                catch (Exception) { }
            }
            //Если была нажата кнопка "СЕ"
            else if (button.Name == "ButtonCE")
            {
                //то очищается число
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.TrimEnd(numbers);
            }
            //Если была нажата кнопка "С"
            else if (button.Name == "ButtonC")
            {
                //то очищается число и знак перед ним (если он есть)
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.TrimEnd(numbers);
                if (ViewModel.HistoryProperty.Length != 0)
                    ViewModel.HistoryProperty = ViewModel.HistoryProperty.Remove(ViewModel.HistoryProperty.Length - 1);
            }
            //Если была нажата кнопка "<-"
            else if (button.Name == "ButtonErase" && ViewModel.HistoryProperty.Length != 0)
            {
                //то стирается последний символ, если он есть
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.Remove(ViewModel.HistoryProperty.Length - 1);
            }
            //Если текст кнопки совпал с какой-либо операцией со списка
            else if (operations.Any(x => (button.Content as string)[0] == x))
            {
                //Если выбранная операция - плюс или если последний символ уравнения - число, то число вводится в уравнение
                //нужно во избежание чего-то на примере такого - "6-/9"
                if ((button.Content as string)[0] == '+' || numbers.Any(x => ViewModel.HistoryProperty[ViewModel.HistoryProperty.Length - 1] == x))
                    ViewModel.HistoryProperty += button.Content;
            }
            //Если текст кнопки - число или если выбрана операция разницы 
            else if ((button.Content as string).All(char.IsDigit)
                  || (button.Content as string) == "-")
            {
                ViewModel.HistoryProperty += button.Content;
            }
        }

        //Принцип тот же, только результат уравнения просчитывается после каждого нажатия любой кнопки
        private void CalculateMouseWEquals(CalcViewModel ViewModel, Button button)
        {
            //Режим работы, не зависящий от кнопки "Равно" (считает сразу)

            if (button.Name == "ButtonCE")
            {
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.TrimEnd(numbers);
            }
            else if (button.Name == "ButtonC")
            {
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.TrimEnd(numbers);
                if (ViewModel.HistoryProperty.Length != 0)
                    ViewModel.HistoryProperty = ViewModel.HistoryProperty.Remove(ViewModel.HistoryProperty.Length - 1);
            }
            else if (ViewModel.HistoryProperty.Length != 0 && button.Name == "ButtonErase")
            {
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.Remove(ViewModel.HistoryProperty.Length - 1);
            }
            else if (operations.Any(x => (button.Content as string)[0] == x))
            {
                if ((button.Content as string)[0] == '+' || numbers.Any(x => ViewModel.HistoryProperty[ViewModel.HistoryProperty.Length - 1] == x))
                    ViewModel.HistoryProperty += button.Content;
            }
            else if ((button.Content as string).All(char.IsDigit)
                  || (button.Content as string) == "-")
            {
                ViewModel.HistoryProperty += button.Content;
            }

            try
            {
                //Неизвестно исключение, выкидываемое Compute
                ViewModel.ResultProperty = new DataTable().Compute(ViewModel.HistoryProperty, null).ToString();
            }
            catch (Exception) { }
        }
        #endregion

        //Работают так же, как и методы мышки.
        #region Методы для работы с клавиатурой
        private void CalculateKeyEquals(CalcViewModel ViewModel, TextCompositionEventArgs e)
        {
            //Режим работы, зависящий от кнопки "Равно"

            if (e.Text[0] == '=')
            {
                try
                {
                    //Неизвестно исключение, выкидываемое Compute
                    ViewModel.ResultProperty = new DataTable().Compute(ViewModel.HistoryProperty, null).ToString();
                }
                catch (Exception) { }
            }
            //else if (e.Text == "C" || e.Text == "c" || e.Text == "С" || e.Text == "с")
            //Английский и русский раскладки
            else if (e.Text.ToLower() == "c" || e.Text.ToLower() == "с")
            {
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.TrimEnd(numbers);
                if (ViewModel.HistoryProperty.Length != 0)
                    ViewModel.HistoryProperty = ViewModel.HistoryProperty.Remove(ViewModel.HistoryProperty.Length - 1);
            }
            else if (operations.Any(x => e.Text[0] == x))
            {
                if (e.Text[0] == '+' || numbers.Any(x => ViewModel.HistoryProperty[ViewModel.HistoryProperty.Length - 1] == x))
                    ViewModel.HistoryProperty += e.Text;
            }
            else if (char.IsDigit(e.Text[0]) || e.Text == "-" || e.Text == "-")
            {
                ViewModel.HistoryProperty += e.Text;
            }
        }

        private void CalculateKeyWEquals(CalcViewModel ViewModel, TextCompositionEventArgs e)
        {
            //Режим работы, не зависящий от кнопки "Равно" (считает сразу)

            if (e.Text.ToLower() == "c" || e.Text.ToLower() == "с")
            {
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.TrimEnd(numbers);
                if (ViewModel.HistoryProperty.Length != 0)
                    ViewModel.HistoryProperty = ViewModel.HistoryProperty.Remove(ViewModel.HistoryProperty.Length - 1);
            }
            else if (operations.Any(x => e.Text[0] == x))
            {
                if (e.Text[0] == '+' || numbers.Any(x => ViewModel.HistoryProperty[ViewModel.HistoryProperty.Length - 1] == x))
                    ViewModel.HistoryProperty += e.Text[0];
            }
            else if (char.IsDigit(e.Text[0]) || e.Text == "-")
            {
                ViewModel.HistoryProperty += e.Text;
            }

            try
            {
                //Неизвестно исключение, выкидываемое Compute
                ViewModel.ResultProperty = new DataTable().Compute(ViewModel.HistoryProperty, null).ToString();
            }
            catch (Exception) { }
        }

        //Метод для работы бэкспейса
        public void Erase(CalcViewModel ViewModel, KeyEventArgs e)
        {
            if (ViewModel.HistoryProperty.Length > 0 && e.Key == Key.Back)
                ViewModel.HistoryProperty = ViewModel.HistoryProperty.Remove(ViewModel.HistoryProperty.Length - 1);
        }
        #endregion
    }
}
