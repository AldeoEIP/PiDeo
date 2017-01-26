using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Storage;
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

        private const int LedPin = 17;
        private GpioPin _pin;
        private bool _isPiConnected;
        ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo (e);
        }

        public MainPage() {
            this.InitializeComponent ();
            CoreWindow.GetForCurrentThread ().KeyDown += MainPage_KeyDown;
            Message.Text = string.Empty;

            StartScenario ();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) {
            Login ();
        }

        private async void Login() {
            
            var composite = (ApplicationDataCompositeValue) _localSettings.Values["credentials"];

            if (composite == null || composite["login"] == null || composite["password"] == null)
                return;

            var login = composite["login"].ToString ();
            var password = composite["password"].ToString ();
            try {
                var connected = false;//await client.LoginAsync (login, password);
            }
            catch (Exception ex) {
                Debug.WriteLine (ex.Message);
                Answer.Text = "Je n'arrive pas à contacter le nid ! Nous retenterons plus tard.";
                return;
            }
            Answer.Text = "Ravi de te voir.";
            ToggleConnection ();
        }

        private async void Connect_OnClick(object sender, RoutedEventArgs e) {
            var login = LoginBox.Text;
            var password = PasswordBox.Password;
            //var client = new Client ();

            bool connected;
            try {
                connected = false; //await client.LoginAsync (login, password);
            }
            catch (Exception ex) {
                Debug.WriteLine (ex.Message);
                Answer.Text = "Je n'arrive pas à contacter le nid ! Nous retenterons plus tard.";
                return;
            }

            if (!connected) {
                Answer.Text = "Oups je ne te reconnais pas! Recommence ou inscris toi sur ton smartphone.";
                return;
            }
            _localSettings.Values["credentials"] = new Windows.Storage.ApplicationDataCompositeValue {
                ["password"] = password,
                ["login"] = login
            };

            Answer.Text = "Bonjour !";
            ToggleConnection ();
        }

        private void StartScenario() {
            var gpio = GpioController.GetDefault ();

            if (gpio == null)
                return;
            _isPiConnected = true;

            _pin = gpio.OpenPin (LedPin);
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
            if (message.Trim ().StartsWith ("Bye", StringComparison.OrdinalIgnoreCase)) {
                _localSettings.Values.Remove ("credentials");
                ToggleConnection ();
            }
            if (message.Trim ().StartsWith ("Vol", StringComparison.OrdinalIgnoreCase)) {
                if (!_isPiConnected)
                    return "Je ne sens pas mes ailes ! Mon avatar est il connecté ?";
                FlapDragon ();
                return "Ça fait du bien de se dégourdir.";
            }

            try {
                var answer = "yo";//await client.GetAnswerAsync (message);
                return answer;
            }
            catch (Exception ex) {
                Debug.WriteLine (ex.Message);
                return "Désolé, je n'ai pas accès au savoir draconique.";
            }
        }

        private async void FlapDragon() {
            _pin.Write (GpioPinValue.High);
            await Task.Delay (TimeSpan.FromSeconds (2));
            _pin.Write (GpioPinValue.Low);
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

        private void ToggleConnection() {
            CredentialsBox.Visibility = 1 - CredentialsBox.Visibility;
            QuestionBox.Visibility = 1 - QuestionBox.Visibility;
        }
    }
}