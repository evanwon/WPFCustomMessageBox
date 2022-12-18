using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WPFCustomMessageBox;
using System.Threading.Tasks;

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
            var result = MessageBox.Show(string.Join("\n", Enumerable.Repeat("Hello World!", 12)));
            Debug.WriteLine(result.ToString());
        }

        private void button_StandardMessageNew_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.Show(string.Join("\n", Enumerable.Repeat("Hello World!", 12)));
            Debug.WriteLine(result.ToString());
        }

        private void button_MessageWithCaption_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Hello World!", "Hello World the title.");
            Debug.WriteLine(result.ToString());
        }

        private void button_MessageWithCaptionNew_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.Show("Hello world!", "Hello World the title.");
            Debug.WriteLine(result.ToString());
        }

        private void button_MessageWithCaptionAndButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Hello World!", "Hello World the title.", MessageBoxButton.OKCancel);
            Debug.WriteLine(result.ToString());
        }

        private void button_MessageWithCaptionAndButtonNew_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.Show("Hello World!", "Hello World the title.", MessageBoxButton.OKCancel);
            Debug.WriteLine(result.ToString());
        }

        private void button_MessageWithCaptionButtonImage_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to eject the nuclear fuel rods?",
                "Confirm Fuel Ejection",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Exclamation);
            Debug.WriteLine(result.ToString());
        }

        private void button_MessageWithCaptionButtonImageNew_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.Show("This is a message.", "This is a caption", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            Debug.WriteLine(result.ToString());
        }

        private void button_MessageWithCaptionButtonCustomImageNew_Click(object sender, RoutedEventArgs e)
        {
            var shield = Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Shield.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            var msgBox = new MessageBoxModel()
            {
                Message = "This is a message with a windows shield beside it.",
                Caption = "This is a caption",
                Buttons = MessageBoxButton.YesNo,
                CustomImage = shield
            };
            var result = msgBox.ShowDialog();

            Debug.WriteLine(result.ToString());
        }

        private void button_SelfUpdatingMessage_Click(object sender, RoutedEventArgs e)
        {
            var stopwatch = Stopwatch.StartNew();
            var msgBox = new MessageBoxModel()
            {
                Message = $"This message box is open since {(int)stopwatch.Elapsed.TotalSeconds}s",
                Caption = $"Open since {(int)stopwatch.Elapsed.TotalSeconds}s",
                Buttons = MessageBoxButton.OK
            };
            var task = msgBox.Show();

            while (task.Status == TaskStatus.Running)
            {
                msgBox.Message = $"This message box is open since {(int)stopwatch.Elapsed.TotalSeconds}s";
                msgBox.Caption = $"Open since {(int)stopwatch.Elapsed.TotalSeconds}s";
                Task.Delay(100).Wait();
            }

            Debug.WriteLine(task.Result.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.ShowYesNoCancel(
                "You have unsaved changes.",
                "Unsaved Changes!",
                "Evan Wondrasek",
                "Don't Save",
                "Cancel",
                MessageBoxImage.Exclamation);

            Debug.WriteLine(result.ToString());
        }
    }
}
