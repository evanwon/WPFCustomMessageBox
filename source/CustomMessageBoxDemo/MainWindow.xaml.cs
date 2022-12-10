using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WPFCustomMessageBox;

namespace CustomMessageBoxDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void button_StandardMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\n");
        }

        private void button_StandardMessageNew_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show("Hello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\nHello World!\nHello World\n");
        }

        private void button_MessageWithCaption_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello World!", "Hello World the title.");
        }

        private void button_MessageWithCaptionNew_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show("Hello world!", "Hello World the title.");
        }

        private void button_MessageWithCaptionAndButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello World!", "Hello World the title.", MessageBoxButton.OKCancel);
        }

        private void button_MessageWithCaptionAndButtonNew_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show("Hello World!", "Hello World the title.", MessageBoxButton.OKCancel);
        }

        private void button_MessageWithCaptionButtonImage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Are you sure you want to eject the nuclear fuel rods?",
                "Confirm Fuel Ejection",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Exclamation);
        }

        private void button_MessageWithCaptionButtonImageNew_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show("This is a message.", "This is a caption", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
        }

        private void button_MessageWithCaptionButtonCustomImageNew_Click(object sender, RoutedEventArgs e)
        {
            var winLogo = Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Shield.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            CustomMessageBox.Show("This is a message with a windows shield beside it.", "This is a caption", MessageBoxButton.YesNo, winLogo);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNoCancel(
                "You have unsaved changes.",
                "Unsaved Changes!",
                "Evan Wondrasek",
                "Don't Save",
                "Cancel",
                MessageBoxImage.Exclamation);

            Console.WriteLine(result);
        }
    }
}
