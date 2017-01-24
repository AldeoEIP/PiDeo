using Windows.Devices.Gpio;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PiDeo {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        static GpioController _gpio;

        private const int LedPin = 17;
        private readonly GpioPin _pin;
        private bool _flag;

        public MainPage() {
            this.InitializeComponent ();
            CoreWindow.GetForCurrentThread ().KeyDown += MainPage_KeyDown;

            _gpio = GpioController.GetDefault ();
            Message.Text = "";
            _pin = _gpio.OpenPin (LedPin);
            _pin.Write (GpioPinValue.Low);
            _pin.SetDriveMode (GpioPinDriveMode.Output);
        }

        private void MainPage_KeyDown(CoreWindow sender, KeyEventArgs e) {
            if (e.VirtualKey == VirtualKey.Enter)
                ButtonBase_OnClick (sender, null);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            Answer.Text = Ask (Message.Text);
        }

        private string Ask(string message) {
            if (string.IsNullOrWhiteSpace (message))
                return "Il fait beau aujourd'hui.";
            if (message.Trim ().Equals ("Vol")) {
                if (_gpio == null)
                    return "Je ne sens pas mes ailes ! Mon avatar est il connecté ?";
                FlapDragon ();
                return "Ça fait du bien de se dégourdir.";
            }
            return "Je n'ai pas accès au avoir draconique.";
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