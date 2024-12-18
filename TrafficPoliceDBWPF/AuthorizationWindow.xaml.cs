using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;


namespace TrafficPoliceDBWPF
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private readonly Timer _timer;
        public AuthorizationWindow()
        {
            _timer = new Timer(100);
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
            InitializeComponent();

            DataContext = new AuthorizationWindowViewModel(this);




            UpdateCapsLockStatus();
            UpdateInputLanguage();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {

            Dispatcher.Invoke(() =>
            {
                UpdateCapsLockStatus();
                UpdateInputLanguage();
            });
        }

        private void UpdateCapsLockStatus()
        {

            if (Keyboard.IsKeyToggled(Key.CapsLock))
            {
                CapsLockLabel.Content = "CapsLock нажата";
            }
            else
            {
                CapsLockLabel.Content = "";
            }
        }

        private void UpdateInputLanguage()
        {
            // Получение текущей раскладки и преобразование в читабельный формат
            var culture = InputLanguageManager.Current.CurrentInputLanguage;

            // Определение языка ввода
            string language;
            switch (culture.TwoLetterISOLanguageName)
            {
                case "ru":
                    language = "Язык ввода: Русский";
                    break;
                case "en":
                    language = "Язык ввода: Английский";
                    break;
                default:
                    language = "Язык ввода: Другой";
                    break;
            }

            // Обновление метки языка ввода
            InputLanguageLabel.Content = language;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }

        }
    }
}
