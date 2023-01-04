using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace WPFCustomMessageBox
{
    internal class CustomMessageBoxViewModel : INotifyPropertyChanged
    {
        #region Constants

        private static double ButtonMinWidth => 88;

        private static double ButtonMaxWidth => 160;

        #endregion

        #region Properties that can be updated while the message box is open

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

        public string CancelButtonCaption
        {
            get => this.cancelButtonCaption;
            set
            {
                this.cancelButtonCaption = value;
                this.OnPropertyChanged(nameof(this.CancelButtonCaption));
            }
        }
        private string cancelButtonCaption = "_Cancel";

        public string NoButtonCaption
        {
            get => this.noButtonCaption;
            set
            {
                this.noButtonCaption = value;
                this.OnPropertyChanged(nameof(this.NoButtonCaption));
            }
        }
        private string noButtonCaption = "_No";

        public string YesButtonCaption
        {
            get => this.yesButtonCaption;
            set
            {
                this.yesButtonCaption = value;
                this.OnPropertyChanged(nameof(this.YesButtonCaption));
            }
        }
        private string yesButtonCaption = "_Yes";

        public string OkButtonCaption
        {
            get => this.okButtonCaption;
            set
            {
                this.okButtonCaption = value;
                this.OnPropertyChanged(nameof(this.OkButtonCaption));
            }
        }
        private string okButtonCaption = "_OK";

        #endregion

        #region Fixed properties

        public double MaxWidth { get; set; } = 470;

        public double MinWidth { get; set; } = 154;

        public double MaxHeight { get; set; } = 600;

        public double MinHeight { get; set; } = 155;

        public ImageSource CustomImage { get; set; }

        public Visibility ImageVisibility => (this.CustomImage is null) ? Visibility.Collapsed : Visibility.Visible;

        public double CancelButtonMinWidth { get; set; } = ButtonMinWidth;

        public double CancelButtonMaxWidth { get; set; } = ButtonMaxWidth;

        public double NoButtonMinWidth { get; set; } = ButtonMinWidth;

        public double NoButtonMaxWidth { get; set; } = ButtonMaxWidth;

        public double YesButtonMinWidth { get; set; } = ButtonMinWidth;

        public double YesButtonMaxWidth { get; set; } = ButtonMaxWidth;

        public double OkButtonMinWidth { get; set; } = ButtonMinWidth;

        public double OkButtonMaxWidth { get; set; } = ButtonMaxWidth;

        public Visibility CancelButtonVisibility { get; set; } = Visibility.Collapsed;

        public Visibility NoButtonVisibility { get; set; } = Visibility.Collapsed;

        public Visibility YesButtonVisibility { get; set; } = Visibility.Collapsed;

        public Visibility OkButtonVisibility { get; set; } = Visibility.Collapsed;

        public ButtonClickCommand CancelButtonClick { get; set; }

        public ButtonClickCommand NoButtonClick { get; set; }

        public ButtonClickCommand YesButtonClick { get; set; }

        public ButtonClickCommand OkButtonClick { get; set; }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
