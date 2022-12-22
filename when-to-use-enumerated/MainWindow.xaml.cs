using IVSoftware;
using System;
using System.IO;
using System.Windows;
using Window = IVSoftware.Window;
using Console = IVSoftware.Console;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace wpf_window_ex
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Bindings();
        }
    }

    internal class Bindings : INotifyPropertyChanged
    {
        public Bindings()
        {
            AdvancePriorityCommand = new Command(OnAdvancePriority);
        }
        int _pLevel = 1;
        public int PLevel
        {
            get => _pLevel;
            set
            {
                if (!Equals(_pLevel, value))
                {
                    _pLevel = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand AdvancePriorityCommand { get; private set; }
        private void OnAdvancePriority(object o)
        {
            // Advance to 2, then to three, then wrap around to 1
            if(PLevel == 3)
            {
                PLevel = 1;
            }
            else
            {
                PLevel++;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    // Returns a formatted version of level, which can be an int or an enum
    public class PriorityLevelToFormatted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => $"Level {(int)value}";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException("Unused");
    }
    public class PriorityLevelToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (int)value == 1 ? Visibility.Hidden : Visibility.Visible;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException("Unused");
    }
    internal class Command : ICommand
    {
        public Command(Action<object> command, Func<object, bool> canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
        }

        private readonly Func<object, bool> _canExecute = null;
        public bool CanExecute(object parameter = null) =>
            _canExecute == null ? true : _canExecute(parameter);

        private readonly Action<object> _command = null;
        public void Execute(object parameter = null) => _command(parameter);
        public event EventHandler CanExecuteChanged;
    }
}
