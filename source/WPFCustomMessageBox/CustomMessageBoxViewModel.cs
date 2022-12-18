using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFCustomMessageBox
{
    internal class CustomMessageBoxViewModel
    {
        public double MaxWidth { get; set; } = 470;

        public double MaxHeight { get; set; } = 900;

        public string Caption { get; set; } = string.Empty;

        public ImageSource CustomImage { get; set; }

        public Visibility ImageVisibility => (this.CustomImage is null) ? Visibility.Collapsed : Visibility.Visible;

        public string Message { get; set; } = string.Empty;

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
    }
}
