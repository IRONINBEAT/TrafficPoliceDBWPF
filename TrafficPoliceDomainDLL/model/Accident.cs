using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class Accident : ViewModelBase
    {
        private DateTime _date;
        private int _streetId;
        private string _streetTitle;
        private string _nearHouseNumber;
        private string _severity;
        private int _inspectorId;
        private string _inspectorName;
        private string _inspectorSurname;
        private string _inspectorPatronymic;
        private string _inspectorFullName;
        private string _description;

        public int Id { get; set; }

        [DisplayName("Дата")]
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
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

        [DisplayName("Номер дома поблизости")]
        public string NearHouseNumber
        {
            get => _nearHouseNumber;
            set
            {
                _nearHouseNumber = value;
                OnPropertyChanged(nameof(NearHouseNumber));
            }
        }

        [DisplayName("Степень тяжести")]
        public string Severity
        {
            get => _severity;
            set
            {
                _severity = value;
                OnPropertyChanged(nameof(Severity));
            }
        }

        public int InspectorId
        {
            get => _inspectorId;
            set
            {
                _inspectorId = value;
                OnPropertyChanged(nameof(InspectorId));
            }
        }

        [DisplayName("ФИО инспектора-расследователя")]
        public string InspectorFullName
        {
            get => _inspectorFullName;
            set
            {
                if (_inspectorFullName != value)
                {
                    _inspectorFullName = value;
                    OnPropertyChanged(nameof(InspectorFullName));
                }

            }
        }

        public string InspectorName
        {
            get => _inspectorName;
            set
            {
                _inspectorName = value;
                OnPropertyChanged(nameof(InspectorName));
                OnPropertyChanged(nameof(InspectorFullName)); // Обновление полного имени
            }
        }

        public string InspectorSurname
        {
            get => _inspectorSurname;
            set
            {
                _inspectorSurname = value;
                OnPropertyChanged(nameof(InspectorSurname));
                OnPropertyChanged(nameof(InspectorFullName)); // Обновление полного имени
            }
        }

        public string InspectorPatronymic
        {
            get => _inspectorPatronymic;
            set
            {
                _inspectorPatronymic = value;
                OnPropertyChanged(nameof(InspectorPatronymic));
                OnPropertyChanged(nameof(InspectorFullName)); // Обновление полного имени
            }
        }

        [DisplayName("Описание ДТП")]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }

}
