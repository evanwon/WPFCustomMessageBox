using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace WPFCustomMessageBox
{
    /// <summary>
    /// Interaction logic for ModalDialog.xaml
    /// </summary>
    internal partial class CustomMessageBoxWindow : Window
    {
        #region Properties

        internal string Caption
        {
            get
            {
                return this.Title;
            }
            set
            {
                this.Title = value;
            }
        }

        internal string Message
        {
            get
            {
                return this.TextBlock_Message.Text;
            }
            set
            {
                this.TextBlock_Message.Text = value;
            }
        }

        internal string OkButtonText
        {
            get
            {
                return this.Label_Ok.Content.ToString();
            }
            set
            {
                this.Label_Ok.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string CancelButtonText
        {
            get
            {
                return this.Label_Cancel.Content.ToString();
            }
            set
            {
                this.Label_Cancel.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string YesButtonText
        {
            get
            {
                return this.Label_Yes.Content.ToString();
            }
            set
            {
                this.Label_Yes.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string NoButtonText
        {
            get
            {
                return this.Label_No.Content.ToString();
            }
            set
            {
                this.Label_No.Content = value.TryAddKeyboardAccellerator();
            }
        }

        public MessageBoxResult Result { get; set; }

        #endregion

        #region Constructor

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button, ImageSource icon)
        {
            this.InitializeComponent();

            this.Message = message;
            this.Caption = caption;

            if (icon != null) // Display image/icon
            {
                this.Image_MessageBox.Source = icon;
                this.Image_MessageBox.Visibility = Visibility.Visible;
            }

            this.DisplayButtons(button);
        }

        #endregion

        #region Methods

        private void DisplayButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    // Hide all but OK, Cancel
                    this.Button_OK.Visibility = Visibility.Visible;
                    this.Button_OK.Focus();
                    this.Button_Cancel.Visibility = Visibility.Visible;

                    this.Button_Yes.Visibility = Visibility.Collapsed;
                    this.Button_No.Visibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.YesNo:
                    // Hide all but Yes, No
                    this.Button_Yes.Visibility = Visibility.Visible;
                    this.Button_Yes.Focus();
                    this.Button_No.Visibility = Visibility.Visible;

                    this.Button_OK.Visibility = Visibility.Collapsed;
                    this.Button_Cancel.Visibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.YesNoCancel:
                    // Hide only OK
                    this.Button_Yes.Visibility = Visibility.Visible;
                    this.Button_Yes.Focus();
                    this.Button_No.Visibility = Visibility.Visible;
                    this.Button_Cancel.Visibility = Visibility.Visible;

                    this.Button_OK.Visibility = Visibility.Collapsed;
                    break;

                default:
                    // Hide all but OK
                    this.Button_OK.Visibility = Visibility.Visible;
                    this.Button_OK.Focus();

                    this.Button_Yes.Visibility = Visibility.Collapsed;
                    this.Button_No.Visibility = Visibility.Collapsed;
                    this.Button_Cancel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.OK;
            this.Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Cancel;
            this.Close();
        }

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Yes;
            this.Close();
        }

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.No;
            this.Close();
        }

        #endregion
    }
}
