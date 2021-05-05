using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Calculator_Binding.ViewModel
{
    public class CalcViewModel : INotifyPropertyChanged
    {
        public CalcViewModel() { }

        private string History = string.Empty;
        public string HistoryProperty
        {
            get { return History; }
            set { History = value; OnPropertyChanged("HistoryProperty"); }
        }

        private string Result = string.Empty;
        public string ResultProperty
        {
            get { return Result; }
            set { Result = value; OnPropertyChanged("ResultProperty"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
