using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SevenAstro2.Models
{
    class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T field, T value, string propertyName, params string[] otherChangedProperties)
        {
            if (!Object.Equals(field, value))
            {
                field = value;

                RaisePropertyChanged(propertyName, otherChangedProperties);

                return true;
            }

            return false;
        }

        protected bool Set<T>(ref T field, T value, string propertyName, bool compare, params string[] otherChangedProperties)
        {
            if (!compare)
            {
                field = value;

                RaisePropertyChanged(propertyName, otherChangedProperties);

                return true;
            }

            return Set(ref field, value, propertyName, otherChangedProperties);
        }
        //protected bool Set<T>(ref T field, T value)
        //{
        //    return this.Set(ref field, value, Here.CallerMemberName);
        //}

        protected void RaisePropertyChanged(string propertyName, params string[] otherChangedProperties)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                foreach (var pn in otherChangedProperties)
                    PropertyChanged(this, new PropertyChangedEventArgs(pn));
            }
        }
        //protected void RaisePropertyChanged()
        //{
        //    this.RaisePropertyChanged(Here.CallerMemberName);
        //}
    }

    class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion
        // Fields 
        #region Constructors
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute; _canExecute = canExecute;
        }
        #endregion
        // Constructors 
        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) { return _canExecute == null ? true : _canExecute(parameter); }
        public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
        public void Execute(object parameter) { _execute(parameter); }
        #endregion
        // ICommand Members 
    }
}
