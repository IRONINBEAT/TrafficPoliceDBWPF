using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class Owner : ViewModelBase
    {
        private int _id;
        private string _surname;
        private string _name;
        private string _patronymic;
        private string _legalRelationTitle;
        private bool _legalRelation;
        private string _organizationName;
        private string _phoneNumber;
        private string _passportSeries;
        private string _passportNumber;
        private DateTime _passportDateOfRelease;
        private string _passportReleaseOrganization;
        private string _postalCode;
        private string _city;
        private int _streetId;
        private string _streetTitle;
        private string _houseNumber;
        private string _appartmentNumber;

        [DisplayName("ID")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        [DisplayName("Фамилия")]
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        [DisplayName("Имя")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [DisplayName("Отчество")]
        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        [DisplayName("Тип владельца")]
        public string LegalRelationTitle
        {
            get => LegalRelation == false ? "Физическое лицо" : "Юридическое лицо";
            set
            {
                _legalRelationTitle = value;
                OnPropertyChanged(nameof(LegalRelationTitle));
            }
        }

        public bool LegalRelation
        {
            get => _legalRelation;
            set
            {
                _legalRelation = value;
                OnPropertyChanged(nameof(LegalRelation));
            }
        }

        [DisplayName("Наименование организации")]
        public string OrganizationName
        {
            get => _organizationName;
            set
            {
                _organizationName = value;
                OnPropertyChanged(nameof(OrganizationName));
            }
        }

        [DisplayName("Телефон")]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        [DisplayName("Паспортные данные")]
        public string PassportDetails => LegalRelation == false
            ? $"{PassportSeries} {PassportNumber}, выдан {PassportDateOfRelease} {PassportReleaseOrganization}"
            : "—";

        [DisplayName("Серия паспорта")]
        public string PassportSeries
        {
            get => _passportSeries;
            set
            {
                _passportSeries = value;
                OnPropertyChanged(nameof(PassportSeries));
            }
        }

        [DisplayName("Номер паспорта")]
        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                _passportNumber = value;
                OnPropertyChanged(nameof(PassportNumber));
            }
        }

        [DisplayName("Дата выдачи паспорта")]
        public DateTime PassportDateOfRelease
        {
            get => _passportDateOfRelease;
            set
            {
                _passportDateOfRelease = value;
                OnPropertyChanged(nameof(PassportDateOfRelease));
            }
        }

        [DisplayName("Кем выдан")]
        public string PassportReleaseOrganization
        {
            get => _passportReleaseOrganization;
            set
            {
                _passportReleaseOrganization = value;
                OnPropertyChanged(nameof(PassportReleaseOrganization));
            }
        }

        [DisplayName("Почтовый индекс")]
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                _postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        [DisplayName("Город")]
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public int StreetId
        {
            get => _streetId;
            set
            {
                _streetId = value;
                OnPropertyChanged(nameof(StreetId));
            }
        }

        [DisplayName("Улица")]
        public string StreetTitle
        {
            get => _streetTitle;
            set
            {
                _streetTitle = value;
                OnPropertyChanged(nameof(StreetTitle));
            }
        }

        [DisplayName("Номер дома")]
        public string HouseNumber
        {
            get => _houseNumber;
            set
            {
                _houseNumber = value;
                OnPropertyChanged(nameof(HouseNumber));
            }
        }

        [DisplayName("Квартира/офис")]
        public string AppartmentNumber
        {
            get => _appartmentNumber;
            set
            {
                _appartmentNumber = value;
                OnPropertyChanged(nameof(AppartmentNumber));
            }
        }
    }
}
