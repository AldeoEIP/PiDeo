using System;
using Windows.Devices.Gpio;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PiDeo {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        //static GpioController _gpio;

        private const int LedPin = 17;
        private GpioPin _pin;
        private bool _isPiConnected;

        public MainPage() {
            this.InitializeComponent ();
            CoreWindow.GetForCurrentThread ().KeyDown += MainPage_KeyDown;
            Message.Text = string.Empty;

            StartScenario ();
        }

        private void StartScenario() {
            var gpio = GpioController.GetDefault ();

            // Set up our GPIO pin for setting values.
            // If this next line crashes with a NullReferenceException,
            // then the problem is that there is no GPIO controller on the device.
            if (gpio == null)
                return;
            _isPiConnected = true;

            _pin = gpio.OpenPin (LedPin);

            // _pin.Write (GpioPinValue.Low); // we dont need this
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
                if (!_isPiConnected)
                    return "Je ne sens pas mes ailes ! Mon avatar est il connecté ?";
                FlapDragon ();
                return "Ça fait du bien de se dégourdir.";
            }
            return "Je n'ai pas accès au avoir draconique.";
        }

        private void FlapDragon() {
            //_pin.Write (_flag ? GpioPinValue.High : GpioPinValue.Low);
            //_flag = !_flag;
            _pin.Write (1 - _pin.Read ());
        }

        //private async void OnMessage(ChatMessage msg) {
        //    await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync (CoreDispatcherPriority.Normal, () =>
        //    {
        //        //ChatVM.Messages.Add (msg);
        //    });
        //}

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            StopScenario ();
        }

        private void StopScenario() {
            _pin?.Dispose ();
        }
    }
}