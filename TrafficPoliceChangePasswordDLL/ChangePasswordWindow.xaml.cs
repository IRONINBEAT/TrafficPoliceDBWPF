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

namespace TrafficPoliceChangePasswordDLL
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly Timer _timer;
        public ChangePasswordWindow(string username)
        {
            _timer = new Timer(100);
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
            InitializeComponent();

            DataContext = new ChangePasswordWindowViewModel(username);




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

        private void PasswordBoxOld_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).OldPassword = ((PasswordBox)sender).Password; }

        }

        private void PasswordBoxNewAgain_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).NewPasswordAgain = ((PasswordBox)sender).Password; }

        }

        private void PasswordBoxNew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).NewPassword = ((PasswordBox)sender).Password; }

        }
    }
}
