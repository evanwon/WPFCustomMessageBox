using System;
using System.Windows;
using System.Windows.Input;

namespace WPFCustomMessageBox
{
    internal class ButtonClickCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<MessageBoxResult> Action { get; set; }

        public MessageBoxResult Result { get; set; } = MessageBoxResult.None;

        public ButtonClickCommand()
        {

        }

        public ButtonClickCommand(Action<MessageBoxResult> action, MessageBoxResult result)
        {
            this.Action = action;
            this.Result = result;
        }

        public bool CanExecute(object parameter)
            => (this.Action != null);

        public void Execute(object parameter)
            => this.Action.Invoke(this.Result);
    }
}
