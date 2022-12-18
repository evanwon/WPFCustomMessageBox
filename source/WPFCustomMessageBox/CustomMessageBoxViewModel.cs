using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFCustomMessageBox
{
    internal class CustomMessageBoxViewModel : INotifyPropertyChanged
    {
        public double MaxWidth { get; set; } = 470;

        public double MaxHeight { get; set; } = 900;

        public string Caption
        {
            get => this.caption;
            set
            {
                this.caption = value;
                this.OnPropertyChanged(nameof(this.Caption));
            }
        }
        private string caption = "Message";

        public ImageSource CustomImage { get; set; }

        public Visibility ImageVisibility => (this.CustomImage is null) ? Visibility.Collapsed : Visibility.Visible;

        public string Message
        {
            get => this.message;
            set
            {
                this.message = value;
                this.OnPropertyChanged(nameof(this.Message));
            }
        }
        private string message = string.Empty;

        public double ButtonMaxWidth { get; set; } = 160;

        public string FirstButtonCaption { get; set; } = string.Empty;

        public string SecondButtonCaption { get; set; } = string.Empty;

        public string ThirdButtonCaption { get; set; } = string.Empty;

        public Visibility SecondButtonVisibility => string.IsNullOrEmpty(this.SecondButtonCaption) ? Visibility.Collapsed : Visibility.Visible;

        public Visibility ThirdButtonVisibility => string.IsNullOrEmpty(this.ThirdButtonCaption) ? Visibility.Collapsed : Visibility.Visible;

        public ButtonClickCommand FirstButtonClick { get; set; }

        public ButtonClickCommand SecondButtonClick { get; set; }

        public ButtonClickCommand ThirdButtonClick { get; set; }

        public Dock FirstButtonDock { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
