using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrafficPoliceDomainDLL.repository;
using TrafficPoliceDomainDLL;

namespace TrafficPoliceDBWPF
{
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        private string _userName;
        private string _password;
        private readonly Window _currentWindow; // Ссылка на текущее окно



        public string UserName
        {
            set
            {
                _userName = value;
                OnPropertyChanged(UserName);
            }
            get
            {
                return _userName;
            }
        }

        public string Password
        {
            set
            {
                _password = value;
                OnPropertyChanged(Password);
            }
            get
            {
                return _password;
            }
        }

        public ICommand Enter { get; set; }
        public AuthorizationWindowViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;

            Enter = new RelayCommand(o => Authenticate());
        }

        public AuthorizationWindowViewModel()
        {

        }

        private void Authenticate()
        {
            var userRepo = new UserRepository();

            var isOk = userRepo.AuthenticateUser(UserName, Password);

            if (isOk)
            {
                var anotherWindow = new MainWindow(UserName); // Экземпляр другого окна
                anotherWindow.Show(); // Открываем новое окно
                _currentWindow.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
