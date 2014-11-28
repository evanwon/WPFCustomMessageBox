using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WPFCustomMessageBox
{
    /// <summary>
    /// Interaction logic for ModalDialog.xaml
    /// </summary>
    internal partial class CustomMessageBoxWindow : Window
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);

        internal string Caption
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }

        internal string Message
        {
            get
            {
                return TextBlock_Message.Text;
            }
            set
            {
                TextBlock_Message.Text = value;
            }
        }

        internal string OkButtonText
        {
            get
            {
                return Label_Ok.Content.ToString();
            }
            set
            {
                Label_Ok.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string CancelButtonText
        {
            get
            {
                return Label_Cancel.Content.ToString();
            }
            set
            {
                Label_Cancel.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string YesButtonText
        {
            get
            {
                return Label_Yes.Content.ToString();
            }
            set
            {
                Label_Yes.Content = value.TryAddKeyboardAccellerator();
            }
        }

        internal string NoButtonText
        {
            get
            {
                return Label_No.Content.ToString();
            }
            set
            {
                Label_No.Content = value.TryAddKeyboardAccellerator();
            }
        }

        public MessageBoxResult Result { get; set; }

        private CustomMessageBoxWindow()
        {
            InitializeComponent();
            SetButtonText();
        }

        internal CustomMessageBoxWindow(string message) : this()
        {
            Message = message;
            Image_MessageBox.Visibility = System.Windows.Visibility.Collapsed;
            DisplayButtons(MessageBoxButton.OK);
        }

        internal CustomMessageBoxWindow(string message, string caption) : this(message)
        {
            Caption = caption;
            DisplayButtons(MessageBoxButton.OK);
        }

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button) : this(message, caption)
        {
            DisplayButtons(button);
        }

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxImage image) : this(message, caption)
        {
            DisplayImage(image);
            DisplayButtons(MessageBoxButton.OK);
        }

        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button, MessageBoxImage image) :
            this(message, caption, image)
        {
            DisplayButtons(button);
            DisplayImage(image);
        }

        private void DisplayButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    // Hide all but OK, Cancel
                    Button_OK.Visibility = System.Windows.Visibility.Visible;
                    Button_OK.IsDefault = true;
                    Button_OK.Focus();
                    Button_Cancel.Visibility = System.Windows.Visibility.Visible;

                    Button_Yes.Visibility = System.Windows.Visibility.Collapsed;
                    Button_No.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNo:
                    // Hide all but Yes, No
                    Button_Yes.Visibility = System.Windows.Visibility.Visible;
                    Button_Yes.Focus();
                    Button_Yes.IsDefault = true;
                    Button_No.Visibility = System.Windows.Visibility.Visible;
                    Button_No.IsCancel = true;

                    Button_OK.Visibility = System.Windows.Visibility.Collapsed;
                    Button_Cancel.Visibility = System.Windows.Visibility.Collapsed;
                    Button_Cancel.IsCancel = false;
                    break;
                case MessageBoxButton.YesNoCancel:
                    // Hide only OK
                    Button_Yes.Visibility = System.Windows.Visibility.Visible;
                    Button_Yes.Focus();
                    Button_Yes.IsDefault = true;
                    Button_No.Visibility = System.Windows.Visibility.Visible;
                    Button_Cancel.Visibility = System.Windows.Visibility.Visible;

                    Button_OK.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                default:
                    // Hide all but OK
                    Button_OK.Visibility = System.Windows.Visibility.Visible;
                    Button_OK.IsDefault = true;
                    Button_OK.Focus();

                    Button_Yes.Visibility = System.Windows.Visibility.Collapsed;
                    Button_No.Visibility = System.Windows.Visibility.Collapsed;
                    Button_Cancel.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        private void DisplayImage(MessageBoxImage image)
        {
            Icon icon;

            switch (image)
            {
                case MessageBoxImage.Exclamation:       // Enumeration value 48 - also covers "Warning"
                    icon = SystemIcons.Exclamation;
                    break;
                case MessageBoxImage.Error:             // Enumeration value 16, also covers "Hand" and "Stop"
                    icon = SystemIcons.Hand;
                    break;
                case MessageBoxImage.Information:       // Enumeration value 64 - also covers "Asterisk"
                    icon = SystemIcons.Information;
                    break;
                case MessageBoxImage.Question:
                    icon = SystemIcons.Question;
                    break;
                default:
                    icon = SystemIcons.Information;
                    break;
            }

            Image_MessageBox.Source = icon.ToImageSource();
            Image_MessageBox.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Sets the initial button text based on Windows strings
        /// </summary>
        private void SetButtonText()
        {
            var okText = GetUserString(800).TrimStart('&');
            var cancelText = GetUserString(801).TrimStart('&');
            var yesText = GetUserString(805).TrimStart('&');
            var noText = GetUserString(806).TrimStart('&');

            if (!string.IsNullOrWhiteSpace(okText))
            {
                Button_OK.Content = "_" + okText;
            }
            if (!string.IsNullOrWhiteSpace(cancelText))
            {
                Button_Cancel.Content = "_" + cancelText;
            }
            if (!string.IsNullOrWhiteSpace(yesText))
            {
                Button_Yes.Content = "_" + yesText;
            }
            if (!string.IsNullOrWhiteSpace(noText))
            {
                Button_No.Content = "_" + noText;
            }
        }

        /// <summary>
        /// Gets a user string from user32.dll
        /// </summary>
        /// <param name="stringId">the id of the string</param>
        /// <returns>the string or string.empty</returns>
        /// <remarks>see http://www.tech-archive.net/Archive/Development/microsoft.public.win32.programmer.kernel/2010-02/msg00129.html 
        /// for list of IDs. Since this is a common technique, MS probably won't change it.</remarks>
        private static string GetUserString(uint stringId)
        {
            var libraryHandle = GetModuleHandle("user32.dll");
            if (libraryHandle == IntPtr.Zero)
            {
                return string.Empty;
            }
            var sb = new StringBuilder(1024);
            var size = LoadString(libraryHandle, stringId, sb, 1024);
            return size > 0 ? sb.ToString() : string.Empty;
        }


        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }        
    }
}
