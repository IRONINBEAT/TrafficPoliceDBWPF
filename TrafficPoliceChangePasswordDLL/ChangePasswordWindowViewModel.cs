using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrafficPoliceDomainDLL;
using TrafficPoliceDomainDLL.repository;

namespace TrafficPoliceChangePasswordDLL
{
    public class ChangePasswordWindowViewModel
    {
        private string _username;
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordAgain { get; set; }

        public ICommand SaveChanges { get; }

        public ChangePasswordWindowViewModel(string username)
        {
            _username = username;

            SaveChanges = new RelayCommand(o => ChangePassword());
        }


        private void ChangePassword()
        {
            var userRepo = new UserRepository();

            if (NewPassword == NewPasswordAgain)
            {
                if (userRepo.ChangePassword(_username, OldPassword, NewPassword))
                {
                    MessageBox.Show("Пароль успешно изменен.");
                }
                else
                {
                    MessageBox.Show("Не удалось изменить пароль.");
                }
            }
            else
            {
                MessageBox.Show("Новые пароли не совпадают!");
            }

        }

    }
}
