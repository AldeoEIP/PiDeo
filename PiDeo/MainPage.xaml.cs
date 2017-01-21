using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Chat;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PiDeo {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        static GpioController _gpio;

        private const int LedPin = 17;
        private GpioPin _pin;
        private bool _flag;

        public MainPage() {
            this.InitializeComponent ();
            CoreWindow.GetForCurrentThread ().KeyDown += MainPage_KeyDown;

            _gpio = GpioController.GetDefault ();
        }

        private void MainPage_KeyDown(CoreWindow sender, KeyEventArgs e) {
            if (e.VirtualKey == VirtualKey.Enter)
                ButtonBase_OnClick (sender, null);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            var message = Message.Text;
            if (string.IsNullOrWhiteSpace (message))
                return;
            if (message.Trim ().Equals ("Vol")) {
                if (_gpio != null)
                    TakeOff ();
                else
                    message = "Je ne sens pas mes ailes ! Mon avatar est il connecté ?";
            }

            Answer.Text = "Je n'ai pas accès au avoir draconique.";
        }

        private void TakeOff() {
            _pin = _gpio.OpenPin (LedPin);
            _pin.Write (GpioPinValue.Low);
            _pin.SetDriveMode (GpioPinDriveMode.Output);
            FlapDragon ();
        }

        private void FlapDragon() {
            _pin.Write (_flag ? GpioPinValue.High : GpioPinValue.Low);
            _flag = !_flag;
        }

        //private async void OnMessage(ChatMessage msg) {
        //    await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync (CoreDispatcherPriority.Normal, () =>
        //    {
        //        //ChatVM.Messages.Add (msg);
        //    });
        //}
    }
}
