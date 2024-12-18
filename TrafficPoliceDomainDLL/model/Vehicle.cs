using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class Vehicle : ViewModelBase
    {

        [DisplayName("id")]
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

        [DisplayName("Регистрационный номер")]
        public string StateRegistrationNumber
        {
            get => _stateRegistrationNumber;
            set
            {
                if (_stateRegistrationNumber != value)
                {
                    _stateRegistrationNumber = value;
                    OnPropertyChanged(nameof(StateRegistrationNumber));
                }
            }
        }

        public int CarBrandId
        {
            get => _carBrandId;
            set
            {
                if (_carBrandId != value)
                {
                    _carBrandId = value;
                    OnPropertyChanged(nameof(CarBrandId));
                }
            }
        }

        [DisplayName("Марка")]
        public string CarBrandTitle
        {
            get => _carBrandTitle;
            set
            {
                if (_carBrandTitle != value)
                {
                    _carBrandTitle = value;
                    OnPropertyChanged(nameof(CarBrandTitle));
                }
            }
        }

        public int CarModelId
        {
            get => _carModelId;
            set
            {
                if (_carModelId != value)
                {
                    _carModelId = value;
                    OnPropertyChanged(nameof(CarModelId));
                }
            }
        }

        [DisplayName("Модель")]
        public string CarModelTitle
        {
            get => _carModelTitle;
            set
            {
                if (_carModelTitle != value)
                {
                    _carModelTitle = value;
                    OnPropertyChanged(nameof(CarModelTitle));
                }
            }
        }

        public int ColorId
        {
            get => _colorId;
            set
            {
                if (_colorId != value)
                {
                    _colorId = value;
                    OnPropertyChanged(nameof(ColorId));
                }
            }
        }

        [DisplayName("Цвет")]
        public string ColorTitle
        {
            get => _colorTitle;
            set
            {
                if (_colorTitle != value)
                {
                    _colorTitle = value;
                    OnPropertyChanged(nameof(ColorTitle));
                }
            }
        }

        public int CarBodyModelId
        {
            get => _carBodyModelId;
            set
            {
                if (_carBodyModelId != value)
                {
                    _carBodyModelId = value;
                    OnPropertyChanged(nameof(CarBodyModelId));
                }
            }
        }

        [DisplayName("Модель кузова")]
        public string BodyModelTitle
        {
            get => _bodyModelTitle;
            set
            {
                if (_bodyModelTitle != value)
                {
                    _bodyModelTitle = value;
                    OnPropertyChanged(nameof(BodyModelTitle));
                }
            }
        }

        [DisplayName("Номер кузова")]
        public string CarBodyNumber
        {
            get => _carBodyNumber;
            set
            {
                if (_carBodyNumber != value)
                {
                    _carBodyNumber = value;
                    OnPropertyChanged(nameof(CarBodyNumber));
                }
            }
        }

        [DisplayName("Номер шасси")]
        public string ChassisNumber
        {
            get => _chassisNumber;
            set
            {
                if (_chassisNumber != value)
                {
                    _chassisNumber = value;
                    OnPropertyChanged(nameof(ChassisNumber));
                }
            }
        }

        [DisplayName("Номер двигателя")]
        public string EngineNumber
        {
            get => _engineNumber;
            set
            {
                if (_engineNumber != value)
                {
                    _engineNumber = value;
                    OnPropertyChanged(nameof(EngineNumber));
                }
            }
        }

        [DisplayName("Объём двигателя, л")]
        public double EngineVolume
        {
            get => _engineVolume;
            set
            {
                if (_engineVolume != value)
                {
                    _engineVolume = value;
                    OnPropertyChanged(nameof(EngineVolume));
                }
            }
        }

        [DisplayName("Мощность двигателя, л.с")]
        public double EnginePower
        {
            get => _enginePower;
            set
            {
                if (_enginePower != value)
                {
                    _enginePower = value;
                    OnPropertyChanged(nameof(EnginePower));
                }
            }
        }

        [DisplayName("Годовой налог, руб.")]
        public decimal AnnualTax
        {
            get => _annualTax;
            set
            {
                if (_annualTax != value)
                {
                    _annualTax = value;
                    OnPropertyChanged(nameof(AnnualTax));
                }
            }
        }

        [DisplayName("Ориентация руля")]
        public string SteeringWheelOrientationTitle
        {
            get => SteeringWheelOrientation == false ? "Левый" : "Правый";
            set
            {
                _steeringWheelOrientationTitle = value;
                OnPropertyChanged(nameof(SteeringWheelOrientationTitle));
            }
        }

        public bool SteeringWheelOrientation
        {
            get => _steeringWheelOrientation;
            set
            {
                _steeringWheelOrientation = value;
                OnPropertyChanged(nameof(SteeringWheelOrientation));
            }
        }

        [DisplayName("Полный привод")]
        public string AWDTitle
        {
            get => AWD == false ? "Отсутствует" : "Есть";
            set
            {
                _awdTitle = value;
                OnPropertyChanged(nameof(AWD));

            }
        }
        public bool AWD
        {
            get => _awd;
            set
            {
                _awd = value;
                OnPropertyChanged(nameof(AWD));

            }
        }

        [DisplayName("Дата выпуска")]
        public DateTime DateOfRelease
        {
            get => _dateOfRelease;
            set
            {
                if (_dateOfRelease != value)
                {
                    _dateOfRelease = value;
                    OnPropertyChanged(nameof(DateOfRelease));
                }
            }
        }

        [DisplayName("Номер тех.талона")]
        public string TechnicalTicketNumber
        {
            get => _technicalTicketNumber;
            set
            {
                if (_technicalTicketNumber != value)
                {
                    _technicalTicketNumber = value;
                    OnPropertyChanged(nameof(TechnicalTicketNumber));
                }
            }
        }

        [DisplayName("Дата выдачи тех.талона")]
        public DateTime TechnicalTicketDateOfRelease
        {
            get => _technicalTicketDateOfRelease;
            set
            {
                if (_technicalTicketDateOfRelease != value)
                {
                    _technicalTicketDateOfRelease = value;
                    OnPropertyChanged(nameof(TechnicalTicketDateOfRelease));
                }
            }
        }

        public int OwnerId
        {
            get => _ownerId;
            set
            {
                if (_ownerId != value)
                {
                    _ownerId = value;
                    OnPropertyChanged(nameof(OwnerId));
                }
            }
        }

        [DisplayName("Владелец")]
        public string OwnerNameTitle
        {
            get => _ownerNameTitle;
            set
            {
                if (_ownerNameTitle != value)
                {
                    _ownerNameTitle = value;
                    OnPropertyChanged(nameof(OwnerNameTitle));
                }
            }
        }

        [DisplayName("Дата регистрации")]
        public DateTime DateOfRegistration
        {
            get => _dateOfRegistration;
            set
            {
                if (_dateOfRegistration != value)
                {
                    _dateOfRegistration = value;
                    OnPropertyChanged(nameof(DateOfRegistration));
                }
            }
        }

        [DisplayName("VIN номер")]
        public string VIN
        {
            get => _vin;
            set
            {
                if (_vin != value)
                {
                    _vin = value;
                    OnPropertyChanged(nameof(VIN));
                }
            }
        }

        public Vehicle Clone()
        {
            return (Vehicle)this.MemberwiseClone(); // Создаём поверхностную копию
        }

        private int _id;
        private string _stateRegistrationNumber;
        private int _carBrandId;
        private string _carBrandTitle;
        private int _carModelId;
        private string _carModelTitle;
        private int _colorId;
        private string _colorTitle;
        private int _carBodyModelId;
        private string _bodyModelTitle;
        private string _carBodyNumber;
        private string _chassisNumber;
        private string _engineNumber;
        private double _engineVolume;
        private double _enginePower;
        private decimal _annualTax;
        private bool _steeringWheelOrientation;
        private string _steeringWheelOrientationTitle;
        private bool _awd;
        private string _awdTitle;
        private DateTime _dateOfRelease;
        private string _technicalTicketNumber;
        private DateTime _technicalTicketDateOfRelease;
        private int _ownerId;
        private string _ownerNameTitle;
        private DateTime _dateOfRegistration;
        private string _vin;

    }
}
