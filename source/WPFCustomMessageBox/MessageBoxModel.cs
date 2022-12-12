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
        /// Owner window
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        /// Message to be displayed
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// Caption of the message box
        /// </summary>
        public string Caption { get; set; } = "Message";

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
        public string YesButtonCaption { get; set; } = "_Yes";

        /// <summary>
        /// Caption of the 'No' button
        /// </summary>
        public string NoButtonCaption { get; set; } = "_No";

        /// <summary>
        /// Caption of the 'Cancel' button
        /// </summary>
        public string CancelButtonCaption { get; set; } = "_Cancel";

        /// <summary>
        /// Caption of the 'OK' button
        /// </summary>
        public string OkButtonCaption { get; set; } = "_OK";

        /// <summary>
        /// Result of the MessageBox
        /// </summary>
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

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
        /// This method is blocking until user input.
        /// </summary>
        /// <returns></returns>
        public MessageBoxResult ShowDialog()
        {
            this.Show().Wait();

            return this.Result;
        }

        /// <summary>
        /// Displays a message box that is defined by the properties of this class.
        /// In case the current thread is not a STA thread already,
        /// a new STA thread is being created and the MessageBox is being displayed from there.
        /// This method is not blocking.
        /// </summary>
        /// <returns>Task that will complete once the user closes the message box</returns>
        public Task Show()
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                return Task.Factory.StartNew(() => this.ShowMessageBoxSTA());
            }

            var thread = new Thread(this.ShowMessageBoxSTA);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return Task.Factory.StartNew(() => thread.Join());
        }

        private void ShowMessageBoxSTA()
        {
            if (this.CustomImage is null)
            {
                this.CustomImage = MessageBoxModel.IconLookup[this.Image].ToImageSource();
            }

            var msg = new CustomMessageBoxView();
            msg.DataContext = msg;
            msg.ShowDialog();
        }
            
        #endregion
    }
}
