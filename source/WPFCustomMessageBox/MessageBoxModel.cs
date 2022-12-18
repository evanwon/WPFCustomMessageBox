using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Maximum window width
        /// </summary>
        public double MaxWidth
        {
            get => this.ViewModel.MaxWidth;
            set => this.ViewModel.MaxWidth = Math.Min(Math.Max(value, 0), 10000);
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
        /// Caption of the 'Yes' button
        /// </summary>
        public string YesButtonCaption { get; set; } = "Yes";

        /// <summary>
        /// Caption of the 'No' button
        /// </summary>
        public string NoButtonCaption { get; set; } = "No";

        /// <summary>
        /// Caption of the 'Cancel' button
        /// </summary>
        public string CancelButtonCaption { get; set; } = "Cancel";

        /// <summary>
        /// Caption of the 'OK' button
        /// </summary>
        public string OkButtonCaption { get; set; } = "OK";

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
                FirstButtonClick = new ButtonClickCommand(this.SetResult),
                SecondButtonClick = new ButtonClickCommand(this.SetResult),
                ThirdButtonClick = new ButtonClickCommand(this.SetResult)
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

            this.RequestClosingEvent = null;
            this.RequestClosingEvent += new Action(() => {
                msg?.Close(); msg = null;
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
                    this.ViewModel.FirstButtonCaption = this.OkButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.SecondButtonCaption = this.OkButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.FirstButtonClick.Parameter = MessageBoxResult.OK;
                    this.ViewModel.SecondButtonClick.Parameter = MessageBoxResult.Cancel;
                    this.ViewModel.FirstButtonDock = Dock.Left;
                    break;

                case MessageBoxButton.YesNoCancel:
                    this.ViewModel.FirstButtonCaption = this.YesButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.SecondButtonCaption = this.NoButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.ThirdButtonCaption = this.CancelButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.FirstButtonClick.Parameter = MessageBoxResult.Yes;
                    this.ViewModel.SecondButtonClick.Parameter = MessageBoxResult.No;
                    this.ViewModel.ThirdButtonClick.Parameter = MessageBoxResult.Cancel;
                    break;

                case MessageBoxButton.YesNo:
                    this.ViewModel.FirstButtonCaption = this.YesButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.SecondButtonCaption = this.NoButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.FirstButtonClick.Parameter = MessageBoxResult.Yes;
                    this.ViewModel.SecondButtonClick.Parameter = MessageBoxResult.No;
                    break;

                default: //MessageBoxButton.OK
                    this.ViewModel.FirstButtonCaption = this.OkButtonCaption.TryAddKeyboardAccellerator();
                    this.ViewModel.FirstButtonClick.Parameter = MessageBoxResult.OK;
                    this.ViewModel.FirstButtonDock = Dock.Left;
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
