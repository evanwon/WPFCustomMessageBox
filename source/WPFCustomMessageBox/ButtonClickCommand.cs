using System;
using System.Windows;
using System.Windows.Input;

namespace WPFCustomMessageBox
{
    internal class ButtonClickCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<MessageBoxResult> Action { get; set; }

        public MessageBoxResult Parameter { get; set; } = MessageBoxResult.None;

        public ButtonClickCommand()
        {

        }

        public ButtonClickCommand(Action<MessageBoxResult> action)
        {
            this.Action = action;
        }

        public bool CanExecute(object parameter)
            => (this.Action != null);

        public void Execute(object parameter)
            => this.Action.Invoke(this.Parameter);
    }
}
