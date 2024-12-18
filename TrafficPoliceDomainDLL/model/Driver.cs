using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class Driver : ViewModelBase
    {

        private int _id;
        [DisplayName("ID")]
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string _surname;
        [DisplayName("Фамилия")]
        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        private string _name;
        [DisplayName("Имя")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _patronymic;
        [DisplayName("Отчество")]
        public string Patronymic
        {
            get => _patronymic;
            set
            {
                if (_patronymic != value)
                {
                    _patronymic = value;
                    OnPropertyChanged(nameof(Patronymic));
                }
            }
        }

        // Автосгенерированное свойство для ФИО
        private string _driverFullName;
        [DisplayName("ФИО")]
        public string DriverFullName
        {
            get => $"{Surname} {Name} {Patronymic}";
            set
            {
                _driverFullName = value;
                OnPropertyChanged(nameof(DriverFullName));
            }
        }

        private string _phoneNumber;
        [DisplayName("Телефон, +7")]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        private string _passportSeries;
        [DisplayName("Серия паспорта")]
        public string PassportSeries
        {
            get => _passportSeries;
            set
            {
                if (_passportSeries != value)
                {
                    _passportSeries = value;
                    OnPropertyChanged(nameof(PassportSeries));
                    OnPropertyChanged(nameof(PassportDetails)); // Обновляем составное свойство
                }
            }
        }

        private string _passportNumber;
        [DisplayName("Номер паспорта")]
        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                if (_passportNumber != value)
                {
                    _passportNumber = value;
                    OnPropertyChanged(nameof(PassportNumber));
                    OnPropertyChanged(nameof(PassportDetails)); // Обновляем составное свойство
                }
            }
        }

        private DateTime _passportDateOfRelease;
        [DisplayName("Дата выдачи паспорта")]
        public DateTime PassportDateOfRelease
        {
            get => _passportDateOfRelease;
            set
            {
                if (_passportDateOfRelease != value)
                {
                    _passportDateOfRelease = value;
                    OnPropertyChanged(nameof(PassportDateOfRelease));
                    OnPropertyChanged(nameof(PassportDetails)); // Обновляем составное свойство
                }
            }
        }

        private string _passportReleaseOrganization;
        [DisplayName("Кем выдан")]
        public string PassportReleaseOrganization
        {
            get => _passportReleaseOrganization;
            set
            {
                if (_passportReleaseOrganization != value)
                {
                    _passportReleaseOrganization = value;
                    OnPropertyChanged(nameof(PassportReleaseOrganization));
                    OnPropertyChanged(nameof(PassportDetails)); // Обновляем составное свойство
                }
            }
        }

        private string _licenseNumber;
        [DisplayName("Номер ВУ")]
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                if (_licenseNumber != value)
                {
                    _licenseNumber = value;
                    OnPropertyChanged(nameof(LicenseNumber));
                }
            }
        }

        private DateTime _licenseDateOfRelease;
        [DisplayName("Дата выдачи ВУ")]
        public DateTime LicenseDateOfRelease
        {
            get => _licenseDateOfRelease;
            set
            {
                if (_licenseDateOfRelease != value)
                {
                    _licenseDateOfRelease = value;
                    OnPropertyChanged(nameof(LicenseDateOfRelease));
                }
            }
        }

        // Составное свойство, зависящее от нескольких других
        [DisplayName("Паспортные данные")]
        public string PassportDetails => $"{PassportSeries} {PassportNumber}, выдан {PassportDateOfRelease:dd.MM.yyyy} {PassportReleaseOrganization}";


    }
}
