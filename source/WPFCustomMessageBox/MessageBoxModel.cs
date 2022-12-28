using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFCustomMessageBox
{
    /// <summary>
    /// Model that can be used to configure and the show message boxes
    /// </summary>
    public class MessageBoxModel
    {
        #region Properties

        /// <summary>
        /// Maximum window height
        /// </summary>
        public double MaxHeight
        {
            get => this.ViewModel.MaxHeight;
            set => this.ViewModel.MaxHeight = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// Window height
        /// </summary>
        public double Height
        {
            get => (this.ViewModel.MinHeight == this.ViewModel.MaxHeight) ? this.ViewModel.MinHeight : double.NaN;
            set
            {
                this.MaxHeight = value;
                this.ViewModel.MinHeight = this.MaxHeight;
            }
        }

        /// <summary>
        /// Maximum window width
        /// </summary>
        public double MaxWidth
        {
            get => this.ViewModel.MaxWidth;
            set => this.ViewModel.MaxWidth = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// Window width
        /// </summary>
        public double Width
        {
            get => (this.ViewModel.MinWidth == this.ViewModel.MaxWidth) ? this.ViewModel.MinWidth : double.NaN;
            set
            {
                this.MaxWidth = value;
                this.ViewModel.MinWidth = this.MaxWidth;
            }
        }

        /// <summary>
        /// Minimum button width
        /// </summary>
        public double MinButtonWidth
        {
            get => this.ViewModel.ButtonMinWidth;
            set => this.ViewModel.ButtonMinWidth = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// Maximum button width
        /// </summary>
        public double MaxButtonWidth
        {
            get => this.ViewModel.ButtonMaxWidth;
            set => this.ViewModel.ButtonMaxWidth = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// Cancel button width (double.NaN for 'Auto')
        /// </summary>
        public double CancelButtonWidth
        {
            get => this.ViewModel.CancelButtonWidth;
            set => this.ViewModel.CancelButtonWidth = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// No button width (double.NaN for 'Auto')
        /// </summary>
        public double NoButtonWidth
        {
            get => this.ViewModel.NoButtonWidth;
            set => this.ViewModel.NoButtonWidth = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// Yes button width (double.NaN for 'Auto')
        /// </summary>
        public double YesButtonWidth
        {
            get => this.ViewModel.YesButtonWidth;
            set => this.ViewModel.YesButtonWidth = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// OK button width (double.NaN for 'Auto')
        /// </summary>
        public double OkButtonWidth
        {
            get => this.ViewModel.OkButtonWidth;
            set => this.ViewModel.OkButtonWidth = Math.Min(Math.Max(value, 0), 10000);
        }

        /// <summary>
        /// Owner window
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        /// Message to be displayed
        /// </summary>
        public string Message
        {
            get => this.ViewModel.Message;
            set => this.ViewModel.Message = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Caption of the message box
        /// </summary>
        public string Caption
        {
            get => this.ViewModel.Caption;
            set => this.ViewModel.Caption = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Buttons to show
        /// </summary>
        public MessageBoxButton Buttons { get; set; } = MessageBoxButton.OK;

        /// <summary>
        /// Image that should be shown
        /// </summary>
        public MessageBoxImage Image { get; set; } = MessageBoxImage.None;

        /// <summary>
        /// Custom image that should be shown.
        /// Must be 32x32 pixels in size.
        /// This property overwrites the @Image property if it is not null.
        /// </summary>
        public ImageSource CustomImage { get; set; }

        /// <summary>
        /// Caption of the 'Cancel' button
        /// </summary>
        public string CancelButtonCaption
        {
            get => this.ViewModel.CancelButtonCaption.TryRemoveKeyboardAccellerator();
            set => this.ViewModel.CancelButtonCaption = value.TryAddKeyboardAccellerator();
        }

        /// <summary>
        /// Caption of the 'No' button
        /// </summary>
        public string NoButtonCaption
        {
            get => this.ViewModel.NoButtonCaption.TryRemoveKeyboardAccellerator();
            set => this.ViewModel.NoButtonCaption = value.TryAddKeyboardAccellerator();
        }

        /// <summary>
        /// Caption of the 'Yes' button
        /// </summary>
        public string YesButtonCaption
        {
            get => this.ViewModel.YesButtonCaption.TryRemoveKeyboardAccellerator();
            set => this.ViewModel.YesButtonCaption = value.TryAddKeyboardAccellerator();
        }

        /// <summary>
        /// Caption of the 'OK' button
        /// </summary>
        public string OkButtonCaption
        {
            get => this.ViewModel.OkButtonCaption.TryRemoveKeyboardAccellerator();
            set => this.ViewModel.OkButtonCaption = value.TryAddKeyboardAccellerator();
        }

        /// <summary>
        /// Result of the MessageBox
        /// </summary>
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        private event Action RequestClosingEvent;

        private ManualResetEvent WindowClosedEvent { get; } = new ManualResetEvent(true);

        private CustomMessageBoxViewModel ViewModel { get; }

        private static Dictionary<MessageBoxImage, Icon> IconLookup { get; } = new Dictionary<MessageBoxImage, Icon>()
        {
            { MessageBoxImage.None, null },
            { MessageBoxImage.Error, SystemIcons.Hand },                // Hand, Stop and Error share the same value '16'
            { MessageBoxImage.Question, SystemIcons.Question },
            { MessageBoxImage.Warning, SystemIcons.Warning },           // Exclamation and Warning share the same value '48'
            { MessageBoxImage.Information, SystemIcons.Information }    // Information and Asterisk share the same value '64'
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageBoxModel()
        {
            this.ViewModel = new CustomMessageBoxViewModel()
            {
                CancelButtonClick = new ButtonClickCommand(this.SetResult, MessageBoxResult.Cancel),
                NoButtonClick = new ButtonClickCommand(this.SetResult, MessageBoxResult.No),
                YesButtonClick = new ButtonClickCommand(this.SetResult, MessageBoxResult.Yes),
                OkButtonClick = new ButtonClickCommand(this.SetResult, MessageBoxResult.OK)
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays a message box that is defined by the properties of this class.
        /// In case the current thread is not a STA thread already,
        /// a new STA thread is being created and the MessageBox is being displayed from there.
        /// This method is blocking until user input.
        /// </summary>
        /// <returns></returns>
        public MessageBoxResult ShowDialog()
        {
            this.ConfigureViewModel();

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                this.ShowDialogSTA();
            }
            else
            {
                var thread = new Thread(this.ShowDialogSTA);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }

            return this.Result;
        }

        /// <summary>
        /// Displays a message box that is defined by the properties of this class.
        /// In case the current thread is not a STA thread already,
        /// a new STA thread is being created and the MessageBox is being displayed from there.
        /// This method is not blocking.
        /// </summary>
        /// <returns>Task that will complete once the user closes the message box</returns>
        public Task<MessageBoxResult> Show()
            => Task.Factory.StartNew<MessageBoxResult>(() => this.ShowDialog());

        /// <summary>
        /// Closes the current message box if it is still open
        /// </summary>
        public void Close()
        {
            this.RequestClosingEvent.Invoke();
            this.WindowClosedEvent.WaitOne();
        }

        private void ShowDialogSTA()
        {
            var msg = new CustomMessageBoxView();
            msg.Owner = this.Owner;
            msg.DataContext = this.ViewModel;
            msg.Closed += this.MessageBoxClosed;
            
            // Handle focus (using non MVVM approach here because MVVM would be just far too complicated in this case)
            if(this.ViewModel.OkButtonVisibility == Visibility.Visible)
            {
                msg.OkButton.Focus();
            }
            else if(this.ViewModel.YesButtonVisibility == Visibility.Visible)
            {
                msg.YesButton.Focus();
            }

            this.RequestClosingEvent = null;
            this.RequestClosingEvent += new Action(() =>
            {
                msg?.Close();
                msg = null;
            });

            msg.ShowDialog();
        }

        private void ConfigureViewModel()
        {
            // Set image
            this.ViewModel.CustomImage = this.CustomImage ?? MessageBoxModel.IconLookup[this.Image]?.ToImageSource();

            // Set buttons
            switch (this.Buttons)
            {
                case MessageBoxButton.OKCancel:
                    this.ViewModel.OkButtonVisibility = Visibility.Visible;
                    this.ViewModel.CancelButtonVisibility = Visibility.Visible;
                    break;

                case MessageBoxButton.YesNoCancel:
                    this.ViewModel.CancelButtonVisibility = Visibility.Visible;
                    this.ViewModel.NoButtonVisibility = Visibility.Visible;
                    this.ViewModel.YesButtonVisibility = Visibility.Visible;
                    break;

                case MessageBoxButton.YesNo:
                    this.ViewModel.NoButtonVisibility = Visibility.Visible;
                    this.ViewModel.YesButtonVisibility = Visibility.Visible;
                    break;

                default: //MessageBoxButton.OK
                    this.ViewModel.OkButtonVisibility = Visibility.Visible;
                    break;
            }
        }

        private void SetResult(MessageBoxResult result)
        {
            this.Result = result;
            this.Close();
        }

        private void MessageBoxClosed(object sender, EventArgs e)
        {
            this.WindowClosedEvent.Set();
        }

        #endregion
    }
}
