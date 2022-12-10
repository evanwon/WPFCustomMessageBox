using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace WPFCustomMessageBox
{
    internal class MessageBoxData
    {
        #region Properties

        public Window Owner { get; set; }

        public string Message { get; set; } = "";

        public string Caption { get; set; } = "Message";

        public MessageBoxButton Buttons { get; set; } = MessageBoxButton.OK;

        public MessageBoxImage Image { get; set; } = MessageBoxImage.None;

        public ImageSource Icon { get; set; }

        public string YesButtonCaption { get; set; }

        public string NoButtonCaption { get; set; }

        public string CancelButtonCaption { get; set; }

        public string OkButtonCaption { get; set; }

        public MessageBoxResult Result { get; set; } = MessageBoxResult.None;

        private static Dictionary<MessageBoxImage, Icon> IconLookup { get; } = new Dictionary<MessageBoxImage, Icon>()
        {
            { MessageBoxImage.None, null },
            { MessageBoxImage.Error, SystemIcons.Hand },                // Hand, Stop and Error share the same value '16'
            { MessageBoxImage.Question, SystemIcons.Question },
            { MessageBoxImage.Warning, SystemIcons.Warning },           // Exclamation and Warning share the same value '48'
            { MessageBoxImage.Information, SystemIcons.Information }    // Information and Asterisk share the same value '64'
        };

        #endregion

        #region Methods

        /// <summary>
        /// Displays a message box that is defined by the properties of this class.
        /// In case the current thread is not a STA thread already,
        /// a new STA thread is being created and the MessageBox is being displayed from there.
        /// </summary>
        /// <returns></returns>
        public MessageBoxResult ShowMessageBox()
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                this.ShowMessageBoxSTA();
            }
            else
            {
                var thread = new Thread(this.ShowMessageBoxSTA);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }

            return this.Result;
        }

        private void ShowMessageBoxSTA()
        {
            this.Icon = this.Icon ?? MessageBoxData.IconLookup[this.Image].ToImageSource();

            var msg = new CustomMessageBoxWindow(this.Message, this.Caption, this.Buttons, this.Icon);

            msg.YesButtonText = this.YesButtonCaption ?? msg.YesButtonText;
            msg.NoButtonText = this.NoButtonCaption ?? msg.NoButtonText;
            msg.CancelButtonText = this.CancelButtonCaption ?? msg.CancelButtonText;
            msg.OkButtonText = this.OkButtonCaption ?? msg.OkButtonText;
            msg.Owner = this.Owner ?? msg.Owner;

            msg.ShowDialog();

            this.Result = msg.Result;
        }

        #endregion
    }
}
