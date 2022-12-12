using System.Windows;
using System.Windows.Controls;

namespace WPFCustomMessageBox
{
    /// <summary>
    /// Interaction logic for ModalDialog.xaml
    /// </summary>
    internal partial class CustomMessageBoxView : Window
    {
        internal CustomMessageBoxView()
        {
            this.InitializeComponent();
        }

        internal void ButtonClick(object sender, RoutedEventArgs e)
        {
            switch ((e.Source as Button)?.Name)
            {
                case "FirstButton":
                    break;
                case "SecondButton":
                    break;
                case "ThirdButton":
                    break;
                default:
                    break;
            }

            this.Close();
        }
    }
}
