using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class TechnicalInspection : ViewModelBase
    {
        private int _id;
        private int _vehicleId;
        private string _stateRegistrationNumber;
        private DateTime _dateOfInspection;
        private int _inspectorId;
        private string _inspectorSurname;
        private string _inspectorName;
        private string _inspectorPatronymic;
        private int _mileage;
        private decimal _inspectionPrice;
        private decimal _signPrice;
        private string _inspectorFullName;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged(nameof(VehicleId));
            }
        }

        [DisplayName("Регистрационный номер")]
        public string StateRegistrationNumber
        {
            get => _stateRegistrationNumber;
            set
            {
                _stateRegistrationNumber = value;
                OnPropertyChanged(nameof(StateRegistrationNumber));
            }
        }

        [DisplayName("Дата прохождения ТО")]
        public DateTime DateOfInspection
        {
            get => _dateOfInspection;
            set
            {
                _dateOfInspection = value;
                OnPropertyChanged(nameof(DateOfInspection));
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

        public string InspectorSurname
        {
            get => _inspectorSurname;
            set
            {
                _inspectorSurname = value;
                OnPropertyChanged(nameof(InspectorSurname));
                OnPropertyChanged(nameof(InspectorFullName)); // Обновляем также связанное свойство
            }
        }

        public string InspectorName
        {
            get => _inspectorName;
            set
            {
                _inspectorName = value;
                OnPropertyChanged(nameof(InspectorName));
                OnPropertyChanged(nameof(InspectorFullName)); // Обновляем также связанное свойство
            }
        }

        public string InspectorPatronymic
        {
            get => _inspectorPatronymic;
            set
            {
                _inspectorPatronymic = value;
                OnPropertyChanged(nameof(InspectorPatronymic));
                OnPropertyChanged(nameof(InspectorFullName)); // Обновляем также связанное свойство
            }
        }

        [DisplayName("ФИО инспектора")]
        public string InspectorFullName
        {
            get => _inspectorFullName;
            set
            {
                _inspectorFullName = value;
                OnPropertyChanged(nameof(InspectorFullName));
            }
        }

        [DisplayName("Пробег")]
        public int Mileage
        {
            get => _mileage;
            set
            {
                _mileage = value;
                OnPropertyChanged(nameof(Mileage));
            }
        }

        [DisplayName("Плата за ТО")]
        public decimal InspectionPrice
        {
            get => _inspectionPrice;
            set
            {
                _inspectionPrice = value;
                OnPropertyChanged(nameof(InspectionPrice));
            }
        }

        [DisplayName("Плата за знак ТО")]
        public decimal SignPrice
        {
            get => _signPrice;
            set
            {
                _signPrice = value;
                OnPropertyChanged(nameof(SignPrice));
            }
        }
    }
}
