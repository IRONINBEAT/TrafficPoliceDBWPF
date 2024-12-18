using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrafficPoliceDomainDLL;
using TrafficPoliceDomainDLL.repository;

namespace TrafficPoliceAddUserDLL
{
    public class AddUserWindowViewModel
    {
        public string UserName { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordAgain { get; set; }

        public ICommand AddUserCommand { get; }

        public AddUserWindowViewModel()
        {
            AddUserCommand = new RelayCommand(o => AddUser());
        }

        public void AddUser()
        {
            if (NewPassword == NewPasswordAgain)
            {
                var userRepo = new UserRepository();
                if (userRepo.AddUser(UserName, NewPassword))
                {
                    MessageBox.Show("Пользователь успешно добавлен!");
                }
            }
            else
            {
                MessageBox.Show("Не удалось создать пользователя", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }

        }
    }
}
