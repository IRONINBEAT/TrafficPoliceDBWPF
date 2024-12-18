using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrafficPoliceDomainDLL.model;
using TrafficPoliceDomainDLL.repository;
using TrafficPoliceDomainDLL;
using TrafficPoliceLookupDLL;

namespace TrafficPoliceVehiclesDLL
{
    public class VehiclesWindowViewModel : ViewModelBase
    {
        public ICommand OpenVehicleDetailsCommand { get; }

        public ICommand OpenOwnerInfoCommand { get; }

        public ICommand OpenInspectorLookupCommand { get; }

        public ICommand OpenStreetLookupCommand { get; }

        public ICommand AddVehicleCommand { get; }

        public ICommand EditVehicleCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand AddDriverCommand { get; }

        public ICommand RemoveDriverCommand { get; }

        public ICommand AddLicenseCategoryCommand { get; }

        public ICommand RemoveLicenseCategoryCommand { get; }

        private bool _isVisible;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private bool _isAddOrEdit;

        public bool IsAddOrEdit
        {
            get => _isAddOrEdit;
            set
            {
                _isAddOrEdit = value;
                OnPropertyChanged(nameof(IsAddOrEdit));
            }
        }

        public ICommand IsDriverOwner { get; }


        private bool _isEdit = false;

        public bool IsEdit
        {
            get => _isEdit;
            set { _isEdit = value; OnPropertyChanged(nameof(IsEdit)); }
        }

        private bool _isVisibleOnEdit = false;

        public bool IsVisibleOnEdit
        {
            get => _isVisibleOnEdit;
            set { _isVisibleOnEdit = value; OnPropertyChanged(nameof(IsVisibleOnEdit)); }
        }

        private bool _isVisibleOnAdd = true;

        public bool IsVisibleOnAdd
        {
            get => _isVisibleOnAdd;
            set { _isVisibleOnAdd = value; OnPropertyChanged(nameof(IsVisibleOnAdd)); }
        }

        private bool _isAdd = false;

        public bool IsAdd { get => _isAdd; set => _isAdd = value; }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }
        private Window _currentWindow;

        private ObservableCollection<Driver> _drivers;

        public ObservableCollection<Driver> Drivers
        {
            get { return _drivers; }
            set { _drivers = value; OnPropertyChanged(nameof(Drivers)); }
        }

        private LicenseCategory _licenseCategory;

        public LicenseCategory LicenseCategory
        {
            get { return _licenseCategory; }
            set
            {
                _licenseCategory = value;
                OnPropertyChanged(nameof(LicenseCategory));
            }
        }

        private ObservableCollection<LicenseCategory> _licenseCategories;

        public ObservableCollection<LicenseCategory> LicenseCategories
        {
            get { return _licenseCategories; }
            set
            {
                _licenseCategories = value;
                OnPropertyChanged(nameof(LicenseCategories));
            }
        }

        private Driver _driver;

        public Driver Driver
        {
            get { return _driver; }
            set
            {
                _driver = value;

                var licenseRepo = new LicenseRepository();
                if (Driver.Id != 0)
                {
                    var licenses = licenseRepo.GetLicenceByDriverId(Driver.Id);
                    if (licenses != null)
                        LicenseCategories = new ObservableCollection<LicenseCategory>(licenses);
                }


                if (!string.IsNullOrEmpty(Driver.Surname) && !string.IsNullOrEmpty(Driver.Name) && !string.IsNullOrEmpty(Driver.Patronymic))
                    Driver.DriverFullName = Driver.Surname + " " + Driver.Name + " " + Driver.Patronymic;

                OnPropertyChanged(nameof(Driver));
                OnPropertyChanged(nameof(LicenseCategories));
            }
        }

        private TechnicalInspection _technicalInspection;

        public TechnicalInspection TechnicalInspection
        {
            get
            {
                return _technicalInspection;
            }
            set
            {
                _technicalInspection = value;
                OnPropertyChanged(nameof(TechnicalInspection));
            }
        }

        private Owner _owner;

        public Owner Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }


        private Vehicle _selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get
            {
                return _selectedVehicle;
            }
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicle));

                // Если выбран автомобиль, устанавливаем марку и модель
                if (_selectedVehicle != null)
                {

                    // Загружаем марку
                    SelectedBrand = CarBrands.FirstOrDefault(b => b.Id == _selectedVehicle.CarBrandId);

                    // Загружаем модель
                    SelectedModel = CarModels.FirstOrDefault(m => m.Id == _selectedVehicle.CarModelId);

                    SelectedColor = Colors.FirstOrDefault(c => c.Id == _selectedVehicle.ColorId);

                    SelectedBodyModel = BodyModels.FirstOrDefault(b => b.Id == _selectedVehicle.CarBodyModelId);


                    SteeringWheelOrientation = SelectedVehicle.SteeringWheelOrientationTitle;

                    // Обновляем тех. осмотр
                    var techInspRepo = new TechnicalInspectionRepository();
                    TechnicalInspection = techInspRepo.GetTechnicalInspectionByVehicleId(SelectedVehicle.Id);
                    OnPropertyChanged(nameof(TechnicalInspection));

                    var ownerRepo = new OwnerRepository();
                    Owner = ownerRepo.GetOwnerById(SelectedVehicle.OwnerId);
                    OnPropertyChanged(nameof(Owner));

                    var driverRepo = new DriverRepository();
                    Drivers = new ObservableCollection<Driver>(driverRepo.GetDriversByVehicleId(SelectedVehicle.Id));
                    OnPropertyChanged(nameof(Drivers));

                    if (Drivers.Count() > 0)
                    {
                        Driver = Drivers.FirstOrDefault();
                        var licenseRepo = new LicenseRepository();
                        LicenseCategories = new ObservableCollection<LicenseCategory>(licenseRepo.GetLicenceByDriverId(Driver.Id));
                    }




                    LegalRelation = Owner?.LegalRelationTitle;

                    // Обновляем привязанные значения
                    SelectedVehicleStateRegistrationNumber = _selectedVehicle?.StateRegistrationNumber ?? string.Empty;
                    SelectedVehicleOwnerName = _selectedVehicle?.OwnerNameTitle ?? string.Empty;
                }
            }
        }


        private ObservableCollection<LookupItem> _colors;
        public ObservableCollection<LookupItem> Colors
        {
            get { return _colors; }
            set
            {
                _colors = value;
                OnPropertyChanged(nameof(Colors));
            }
        }

        private LookupItem _selectedColor;
        public LookupItem SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));

                if (_selectedVehicle != null && _selectedColor != null)
                {
                    SelectedVehicle.ColorId = SelectedColor.Id;
                    SelectedVehicle.ColorTitle = _selectedColor?.Title;
                }
            }
        }

        private ObservableCollection<LookupItem> _bodyModels;
        public ObservableCollection<LookupItem> BodyModels
        {
            get { return _bodyModels; }
            set
            {
                _bodyModels = value;
                OnPropertyChanged(nameof(BodyModels));
            }
        }

        private LookupItem _selectedBodyModel;
        public LookupItem SelectedBodyModel
        {
            get { return _selectedBodyModel; }
            set
            {
                _selectedBodyModel = value;
                OnPropertyChanged(nameof(_selectedBodyModel));

                if (_selectedVehicle != null && _selectedBodyModel != null)
                {
                    SelectedVehicle.CarBodyModelId = _selectedBodyModel.Id;
                    SelectedVehicle.BodyModelTitle = _selectedBodyModel?.Title;
                }
            }
        }

        private ObservableCollection<LookupItem> _carBrands;
        private ObservableCollection<LookupItem> _carModels;
        private LookupItem _selectedBrand;
        private LookupItem _selectedModel;

        public ObservableCollection<LookupItem> CarBrands
        {
            get { return _carBrands; }
            set
            {
                _carBrands = value;
                OnPropertyChanged(nameof(CarBrands));
            }
        }

        public ObservableCollection<LookupItem> CarModels
        {
            get { return _carModels; }
            set
            {
                _carModels = value;
                OnPropertyChanged(nameof(CarModels));
            }
        }

        private string _steeringWheelOrientation;

        public string SteeringWheelOrientation
        {
            get => _steeringWheelOrientation;
            set
            {
                _steeringWheelOrientation = value;
                if (_steeringWheelOrientation == "Левый")
                {
                    SelectedVehicle.SteeringWheelOrientation = false;
                    SelectedVehicle.SteeringWheelOrientationTitle = "Левый";
                }
                if (_steeringWheelOrientation == "Правый")
                {
                    SelectedVehicle.SteeringWheelOrientation = true;
                    SelectedVehicle.SteeringWheelOrientationTitle = "Правый";
                }
                OnPropertyChanged(nameof(SelectedVehicle.SteeringWheelOrientation));
                OnPropertyChanged(nameof(SelectedVehicle.SteeringWheelOrientationTitle));
                OnPropertyChanged(nameof(SteeringWheelOrientation));
            }
        }

        private string _legalRelation;

        public string LegalRelation
        {
            get => _legalRelation;
            set
            {
                _legalRelation = value;
                if (_legalRelation == "Физическое лицо")
                {
                    Owner.LegalRelation = false;
                    Owner.LegalRelationTitle = "Физическое лицо";
                    IsVisible = true;
                }
                if (_legalRelation == "Юридическое лицо")
                {
                    Owner.LegalRelation = true;
                    Owner.LegalRelationTitle = "Юридическое лицо";
                    IsVisible = false;
                }
                OnPropertyChanged(nameof(Owner.LegalRelation));
                OnPropertyChanged(nameof(Owner.LegalRelationTitle));
                OnPropertyChanged(nameof(LegalRelation));
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public LookupItem SelectedBrand
        {
            get { return _selectedBrand; }
            set
            {
                _selectedBrand = value;
                OnPropertyChanged(nameof(SelectedBrand));

                // Загружаем модели для выбранной марки
                if (_selectedBrand != null)
                {
                    LoadModelsByBrand(_selectedBrand.Id);
                }
                if (_selectedVehicle != null && _selectedBrand != null)
                {
                    SelectedVehicle.CarBrandId = _selectedBrand.Id;
                    SelectedVehicle.CarBrandTitle = _selectedBrand.Title;
                }

            }
        }

        public LookupItem SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                OnPropertyChanged(nameof(SelectedModel));

                if (_selectedVehicle != null && _selectedModel != null)
                {
                    SelectedVehicle.CarModelId = _selectedModel.Id;
                    SelectedVehicle.CarModelTitle = _selectedModel.Title;
                }
            }
        }

        private string _selectedVehicleStateRegistrationNumber;
        public string SelectedVehicleStateRegistrationNumber
        {
            get => _selectedVehicleStateRegistrationNumber;
            set
            {
                _selectedVehicleStateRegistrationNumber = value;
                OnPropertyChanged(nameof(SelectedVehicleStateRegistrationNumber));
            }
        }

        private string _selectedVehicleOwnerName;
        public string SelectedVehicleOwnerName
        {
            get => _selectedVehicleOwnerName;
            set
            {
                _selectedVehicleOwnerName = value;
                OnPropertyChanged(nameof(SelectedVehicleOwnerName));
            }
        }

        private ObservableCollection<Vehicle> _allVehicles; // Полная коллекция всех автомобилей
        private ObservableCollection<Vehicle> _vehicles;   // Отфильтрованная коллекция

        public ObservableCollection<Vehicle> Vehicles
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                OnPropertyChanged(nameof(Vehicles));
            }
        }

        private string _searchText; // Текст для поиска
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                PerformSearch(); // Выполняем поиск при изменении текста
            }
        }



        public VehiclesWindowViewModel(Window currentWindow)
        {
            LoadColors();

            LoadBodyModels();

            LoadBrands();

            GetVehiclesList();

            _currentWindow = currentWindow;
            EditVehicleCommand = new RelayCommand(o => OnEditVehicle());

            SaveCommand = new RelayCommand(o => SaveChanges());
            DeleteCommand = new RelayCommand(o => DeleteVehicle());
            AddVehicleCommand = new RelayCommand(o => OnAddVehicle());
            OpenInspectorLookupCommand = new RelayCommand(o => OpenInspectorLookup());
            OpenStreetLookupCommand = new RelayCommand(o => OpenStreetLookup());
            CancelCommand = new RelayCommand(o => Cancel());
            AddDriverCommand = new RelayCommand(o => AddDriverToVehicle());
            RemoveDriverCommand = new RelayCommand(o => RemoveDriver());
            IsDriverOwner = new RelayCommand(o => FillDriverInfoIfIsOwner());
            AddLicenseCategoryCommand = new RelayCommand(o => AddLicenseCategory());
            RemoveLicenseCategoryCommand = new RelayCommand(o => RemoveLicenseCategory());

            SelectedVehicle = Vehicles.FirstOrDefault();




        }

        private void GetVehiclesList()
        {
            var vehicleRepo = new VehicleRepository();
            var vehicleList = vehicleRepo.GetAllVehicles();

            // Округляем EnginePower до двух знаков у всех объектов
            foreach (var vehicle in vehicleList)
            {
                vehicle.EngineVolume = Math.Round(vehicle.EngineVolume, 2);
            }

            _allVehicles = new ObservableCollection<Vehicle>(vehicleList); // Сохраняем оригинальную коллекцию
            Vehicles = new ObservableCollection<Vehicle>(_allVehicles);   // Копируем для отображения
        }

        private void PerformSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                // Если поле поиска пустое, показываем все записи
                Vehicles = new ObservableCollection<Vehicle>(_allVehicles);
            }
            else
            {
                // Приводим строки к одному регистру для сравнения
                var filteredVehicles = _allVehicles
                    .Where(v => v.StateRegistrationNumber != null &&
                                v.StateRegistrationNumber
                                    .ToLower()
                                    .Contains(SearchText.ToLower()))
                    .ToList();

                Vehicles = new ObservableCollection<Vehicle>(filteredVehicles);
            }
        }

        private void LoadColors()
        {
            var colorRepo = new LookupRepository();
            var colorList = colorRepo.GetLookupItems("colour");

            Colors = new ObservableCollection<LookupItem>(colorList);
        }

        private void LoadBodyModels()
        {
            var bodyModelRepo = new LookupRepository();
            var bodyModelsList = bodyModelRepo.GetLookupItems("body_model");

            BodyModels = new ObservableCollection<LookupItem>(bodyModelsList);
        }

        private bool CanOpenVehicleDetails(object parameter)
        {
            // Команда доступна только если выбрана машина
            return SelectedVehicle != null;
        }

        private void LoadBrands()
        {
            var brandRepo = new LookupRepository();
            var brandList = brandRepo.GetLookupItems("car_brand");

            CarBrands = new ObservableCollection<LookupItem>(brandList);
        }

        private void LoadModelsByBrand(int brandId)
        {
            var modelRepo = new LookupRepository();
            var modelList = modelRepo.GetLookupItems("car_model", brandId);

            CarModels = new ObservableCollection<LookupItem>(modelList);
        }

        private void OpenInspectorLookup()
        {
            var lookupRepository = new LookupRepository();
            var inspectors = lookupRepository.GetLookupItems("inspector");
            var lookupWindow = new LookupWindow();
            var lookupViewModel = new LookupViewModel(inspectors);

            lookupViewModel.OnItemSelected = selectedItem =>
            {
                if (selectedItem != null)
                {
                    TechnicalInspection.InspectorId = selectedItem.Id;
                    var fullName = selectedItem.Title.Split(' ')
                                 .Take(3)
                                 .Aggregate((part1, part2) => part1 + " " + part2);
                    TechnicalInspection.InspectorFullName = fullName;
                    OnPropertyChanged(nameof(TechnicalInspection.InspectorId));
                    OnPropertyChanged(nameof(TechnicalInspection.InspectorFullName));
                    lookupWindow.Close();
                }
            };

            lookupWindow.DataContext = lookupViewModel;
            lookupWindow.ShowDialog();
        }

        private void OpenStreetLookup()
        {
            var lookupRepository = new LookupRepository();
            var streets = lookupRepository.GetLookupItems("street");
            var lookupWindow = new LookupWindow();
            var lookupViewModel = new LookupViewModel(streets);

            lookupViewModel.OnItemSelected = selectedItem =>
            {
                if (selectedItem != null)
                {
                    Owner.StreetId = selectedItem.Id;  // Прямо модифицируем переданный объект
                    Owner.StreetTitle = selectedItem.Title;
                    OnPropertyChanged(nameof(Owner.StreetId));
                    OnPropertyChanged(nameof(Owner.StreetTitle));
                    lookupWindow.Close();
                }
            };

            lookupWindow.DataContext = lookupViewModel;
            lookupWindow.ShowDialog();

        }

        private void SaveChanges()
        {
            if (ValidateOwnerData() && ValidateVehicleData() && ValidateTechnicalInspection() && ValidateDriver())
            {
                if (IsAdd == false && IsEdit == true)
                {
                    var userResponse = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (userResponse == MessageBoxResult.Yes)
                    {
                        var vehicleRepository = new VehicleRepository();
                        var ownerRepository = new OwnerRepository();
                        var techInspRepo = new TechnicalInspectionRepository();
                        var driverRepo = new DriverRepository();
                        var licenseRepo = new LicenseRepository();

                        var savedVehicle = SelectedVehicle;
                        var savedOwner = Owner;
                        var savedTechInsp = TechnicalInspection;

                        vehicleRepository.UpdateVehicle(SelectedVehicle);

                        ownerRepository.UpdateOwner(Owner);

                        techInspRepo.UpdateTechnicalInspection(TechnicalInspection);

                        foreach (var driver in Drivers)
                        {
                            if (Driver.Id == 0)
                            {
                                driverRepo.AddDriver(Driver);

                            }
                        }


                        driverRepo.AddDriversToVehicle(SelectedVehicle.Id, Drivers.ToList());

                        licenseRepo.AddLicenceToDriver(Driver.Id, LicenseCategories.ToList());

                        foreach (var driver in Drivers)
                        {
                            driverRepo.UpdateDriverById(driver);
                        }

                        GetVehiclesList();

                        SelectedVehicle = savedVehicle;
                        Owner = savedOwner;
                        TechnicalInspection = savedTechInsp;

                        IsEdit = false;
                        IsVisibleOnEdit = false;
                        IsAddOrEdit = false;
                    }
                }
                else
                {


                    // 1. Добавляем владельца
                    var ownerRepository = new OwnerRepository();


                    var ownerId = ownerRepository.AddOwner(Owner); // Метод добавления владельца в БД
                    if (ownerId == 0)
                    {
                        MessageBox.Show("Ошибка при добавлении владельца.");
                        return;
                    }
                    // 2. Добавляем транспортное средство
                    var vehicleRepository = new VehicleRepository();
                    SelectedVehicle.OwnerId = ownerId;

                    var vehicleId = vehicleRepository.AddVehicle(SelectedVehicle); // Метод добавления ТС в БД
                    if (vehicleId == 0)
                    {
                        MessageBox.Show("Ошибка при добавлении транспортного средства.");
                        return;
                    }

                    // 3. Добавляем техосмотр
                    var technicalInspectionRepository = new TechnicalInspectionRepository();

                    TechnicalInspection.VehicleId = vehicleId;
                    TechnicalInspection.StateRegistrationNumber = SelectedVehicle.StateRegistrationNumber;

                    var result = technicalInspectionRepository.AddTechnicalInspection(TechnicalInspection); // Метод добавления техосмотра
                    if (!result)
                    {
                        MessageBox.Show("Ошибка при добавлении техосмотра.");
                        return;
                    }

                    // Если все прошло успешно, обновляем список автомобилей и переходим к новым данным
                    GetVehiclesList();

                    IsAdd = false;
                    IsVisibleOnAdd = true;
                    IsAddOrEdit = false;

                }
            }
        }

        private bool ValidateOwnerData()
        {
            if (string.IsNullOrEmpty(Owner.Name) || string.IsNullOrEmpty(Owner.Surname) || string.IsNullOrEmpty(Owner.Patronymic))
            {
                MessageBox.Show("Неполное ФИО владельца!");
                return false;
            }
            if (string.IsNullOrEmpty(LegalRelation))
            {
                {
                    MessageBox.Show("Необходимо указать тип отношения!");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(Owner.PhoneNumber) || Owner.PhoneNumber.Length != 10 || !IsNumberValid(Owner.PhoneNumber))
            {
                MessageBox.Show("Номер телефона заполнен неверно!");
                return false;
            }


            if (Owner.LegalRelation == false && (Owner.PassportDateOfRelease == DateTime.MinValue ||
                                                 string.IsNullOrEmpty(Owner.PassportNumber) ||
                                                 string.IsNullOrEmpty(Owner.PassportSeries) ||
                                                 string.IsNullOrEmpty(Owner.PassportReleaseOrganization)))
            {
                MessageBox.Show("Физ.лицу необходимо указать паспортные данные!");
                return false;
            }
            if (Owner.LegalRelation == false && (Owner.PassportDateOfRelease == DateTime.MinValue ||
                                                 Owner.PassportNumber.Count() != 6 ||
                                                 Owner.PassportSeries.Count() != 4 ||
                                                 !IsNumberValid(Owner.PassportSeries) ||
                                                 !IsNumberValid(Owner.PassportNumber) ||
                                                 string.IsNullOrEmpty(Owner.PassportReleaseOrganization)))
            {
                MessageBox.Show("Неверно заполнены паспортные данные владельца!");
                return false;
            }
            if (Owner.LegalRelation == true && string.IsNullOrEmpty(Owner.OrganizationName))
            {
                MessageBox.Show("Юр.лицу необходимо указать наименование организации!");
                return false;
            }
            if (Owner.PostalCode.Count() != 6 || string.IsNullOrEmpty(Owner.PostalCode) || !IsNumberValid(Owner.PostalCode))
            {
                MessageBox.Show("Индекс указан неверно!");
                return false;
            }
            if (string.IsNullOrEmpty(Owner.City) || string.IsNullOrEmpty(Owner.StreetTitle) ||
                string.IsNullOrEmpty(Owner.HouseNumber) || string.IsNullOrEmpty(Owner.AppartmentNumber) || string.IsNullOrEmpty(Owner.PostalCode))
            {
                MessageBox.Show("Неполный адрес владельца!");
                return false;
            }


            return true;
        }

        private bool ValidateVehicleData()
        {
            if (string.IsNullOrEmpty(SelectedVehicle.StateRegistrationNumber))
            {
                MessageBox.Show("Необходимо указать гос.номер!");
                return false;
            }
            if (SelectedBrand == null)
            {
                MessageBox.Show("Марка не указана!");
                return false;
            }
            if (SelectedModel == null)
            {
                MessageBox.Show("Модель не указана!");
                return false;
            }
            if (SelectedColor == null)
            {
                MessageBox.Show("Цвет не указан!");
                return false;
            }
            if (SelectedBodyModel == null)
            {
                MessageBox.Show("Тип кузова не указан!");
                return false;
            }
            if (SelectedVehicle.DateOfRelease == DateTime.MinValue)
            {
                MessageBox.Show("Дата выпуска ТС не указана!");
                return false;
            }
            if (string.IsNullOrEmpty(SteeringWheelOrientation))
            {
                MessageBox.Show("Положение руля не указано!");
                return false;
            }
            if (SelectedVehicle.EngineVolume == 0)
            {
                MessageBox.Show("Объем двигателя указан неверно!");
                return false;
            }
            if (SelectedVehicle.EnginePower == 0)
            {
                MessageBox.Show("Мощность двигателя указана неверно!");
                return false;
            }
            if (SelectedVehicle.AnnualTax == 0)
            {
                MessageBox.Show("Годовой налог указан неверно!");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedVehicle.VIN))
            {
                MessageBox.Show("VIN номер не указан!");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedVehicle.EngineNumber))
            {
                MessageBox.Show("Номер двигателя не указан!");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedVehicle.CarBodyNumber))
            {
                MessageBox.Show("Номер кузова не указан!");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedVehicle.ChassisNumber))
            {
                MessageBox.Show("Номер шасси не указан!");
                return false;
            }
            if (SelectedVehicle.TechnicalTicketDateOfRelease == DateTime.MinValue)
            {
                MessageBox.Show("Дата выдачи тех.талона не указана!");
                return false;
            }
            if (string.IsNullOrEmpty(SelectedVehicle.TechnicalTicketNumber))
            {
                MessageBox.Show("Номер тех.талона не указан!");
                return false;
            }
            if (SelectedVehicle.DateOfRegistration == DateTime.MinValue)
            {
                MessageBox.Show("Дата регистрации не указана!");
                return false;
            }
            return true;
        }

        private bool ValidateTechnicalInspection()
        {
            if (TechnicalInspection.DateOfInspection == DateTime.MinValue)
            {
                MessageBox.Show("Дата прохождения ТО не указана!");
                return false;
            }
            if (TechnicalInspection.InspectorId == 0)
            {
                MessageBox.Show("Инспектор не указан!");
                return false;
            }
            if (TechnicalInspection.Mileage == 0)
            {
                MessageBox.Show("Пробег указан неверно!");
                return false;
            }
            if (TechnicalInspection.InspectionPrice == 0)
            {
                MessageBox.Show("Плата за ТО указана неверно!");
                return false;
            }
            if (TechnicalInspection.SignPrice == 0)
            {
                MessageBox.Show("Плата за знак ТО указана неверно!");
                return false;
            }
            return true;
        }

        private bool ValidateDriver()
        {
            if (Drivers.Count() == 0)
            {
                MessageBox.Show("Водители не указаны!");
                return false;
            }
            if (string.IsNullOrEmpty(Driver.Name) || string.IsNullOrEmpty(Driver.Surname) || string.IsNullOrEmpty(Driver.Patronymic))
            {
                MessageBox.Show("Неполное ФИО водителя!");
                return false;
            }
            if (string.IsNullOrEmpty(Driver.PhoneNumber) || Driver.PhoneNumber.Length != 10 || !IsNumberValid(Driver.PhoneNumber))
            {
                MessageBox.Show("Номер телефона заполнен неверно!");
                return false;
            }
            if ((Driver.PassportDateOfRelease == DateTime.MinValue ||
                                                 Driver.PassportNumber.Count() != 6 ||
                                                 Driver.PassportSeries.Count() != 4 ||
                                                 !IsNumberValid(Driver.PassportSeries) ||
                                                 !IsNumberValid(Driver.PassportNumber) ||
                                                 string.IsNullOrEmpty(Driver.PassportReleaseOrganization)))
            {
                MessageBox.Show("Неверно заполнены паспортные данные водителя!");
                return false;
            }
            if (string.IsNullOrEmpty(Driver.LicenseNumber) || !IsNumberValid(Driver.LicenseNumber))
            {
                MessageBox.Show("Неверно заполнен номер водительского удостоверения!");
                return false;
            }
            if (Driver.LicenseDateOfRelease == DateTime.MinValue)
            {
                MessageBox.Show("Не указана дата выдачи водительского удостоверения!");
                return false;
            }
            return true;
        }

        public void DeleteVehicle()
        {
            var userResponse1 = MessageBox.Show("Вы уверены, что хотите удалить транспортное средство без возможности восстановаления?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (userResponse1 == MessageBoxResult.Yes)
            {
                var userResponse2 = MessageBox.Show("Подтвердите удаление?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (userResponse2 == MessageBoxResult.Yes)
                {
                    var vehicleRepository = new VehicleRepository();
                    bool result = vehicleRepository.DeleteVehicle(SelectedVehicle.Id);

                    GetVehiclesList();
                }
            }
        }

        private void OnAddVehicle()
        {
            SelectedVehicle = new Vehicle();
            Owner = new Owner();
            TechnicalInspection = new TechnicalInspection();
            LicenseCategories = new ObservableCollection<LicenseCategory>();
            SelectedTabIndex = 1;

            IsAdd = true;
            IsVisibleOnAdd = false;
            IsAddOrEdit = true;
        }

        private void Cancel()
        {
            IsAdd = false;


            IsEdit = false;

            IsAddOrEdit = false;
        }

        public void OnEditVehicle()
        {
            IsEdit = true;
            IsVisibleOnEdit = true;
            IsAddOrEdit = true;
        }

        private bool IsNumberValid(string number)
        {

            // Проверка, что номер состоит только из цифр
            return Regex.IsMatch(number, "^[0-9]*$");
        }

        private void AddDriverToVehicle()
        {
            if (Drivers.Count > 0 && Owner.LegalRelation == false)
            {
                MessageBox.Show("У физ.лица должно быть не более одного водителя!", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            else
            {
                Driver = new Driver();
                Drivers.Add(Driver);
            }

        }

        private void RemoveDriver()
        {
            if (Driver != null)
            {
                var userResponse1 = MessageBox.Show($"Вы уверены, что хотите удалить водителя {Driver.DriverFullName} без возможности восстановаления?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (userResponse1 == MessageBoxResult.Yes)
                {
                    var userResponse2 = MessageBox.Show("Подтвердите удаление?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (userResponse2 == MessageBoxResult.Yes)
                    {

                        var driverRepo = new DriverRepository();
                        driverRepo.RemoveDriverById(Driver.Id);

                        Drivers.Remove(Driver);
                        GetVehiclesList();
                    }
                }
            }

        }

        private void FillDriverInfoIfIsOwner()
        {
            if (Owner.LegalRelation == false)
            {

                Driver = new Driver();
                Drivers.Clear();
                Driver.Name = Owner.Name;
                Driver.Surname = Owner.Surname;
                Driver.Patronymic = Owner.Patronymic;
                Driver.DriverFullName = Owner.Surname + " " + Owner.Name + " " + Owner.Patronymic;
                Driver.PhoneNumber = Owner.PhoneNumber;

                Driver.PassportSeries = Owner.PassportSeries;
                Driver.PassportNumber = Owner.PassportNumber;
                Driver.PassportDateOfRelease = Owner.PassportDateOfRelease;
                Driver.PassportReleaseOrganization = Owner.PassportReleaseOrganization;

                Drivers.Add(Driver);

                OnPropertyChanged(nameof(IsDriverOwner));
                OnPropertyChanged(nameof(Driver));
                OnPropertyChanged(nameof(Drivers));
            }
            else
            {
                MessageBox.Show("Юр. лицо не может быть водителем!", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void AddLicenseCategory()
        {
            LicenseCategory = new LicenseCategory();
            var lookupRepository = new LookupRepository();
            var categories = lookupRepository.GetLookupItems("license_category");
            var lookupWindow = new LookupWindow();
            var lookupViewModel = new LookupViewModel(categories);

            lookupViewModel.OnItemSelected = selectedItem =>
            {
                if (selectedItem != null)
                {
                    if (LicenseCategories.Any(category => category.Id == selectedItem.Id))
                    {
                        MessageBox.Show(
                            "Эта категория уже добавлена!.",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    LicenseCategory.Id = selectedItem.Id;  // Прямо модифицируем переданный объект
                    LicenseCategory.Code = selectedItem.Title.Split(',')[0];
                    LicenseCategory.Description = selectedItem.Title.Split(',')[1];
                    LicenseCategories.Add(LicenseCategory);
                    OnPropertyChanged(nameof(LicenseCategory.Id));
                    OnPropertyChanged(nameof(LicenseCategory.Code));
                    OnPropertyChanged(nameof(LicenseCategory.Description));
                    OnPropertyChanged(nameof(LicenseCategories));
                    lookupWindow.Close();
                }
            };

            lookupWindow.DataContext = lookupViewModel;
            lookupWindow.ShowDialog();


        }

        private void RemoveLicenseCategory()
        {
            if (LicenseCategory != null && Driver != null)
            {
                var userResponse1 = MessageBox.Show($"Вы уверены, что хотите удалить категорию {LicenseCategory.Code} у {Driver.DriverFullName} без возможности восстановаления?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (userResponse1 == MessageBoxResult.Yes)
                {
                    var userResponse2 = MessageBox.Show("Подтвердите удаление?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (userResponse2 == MessageBoxResult.Yes)
                    {

                        var licenseRepo = new LicenseRepository();
                        licenseRepo.RemoveLicenceFromDriver(Driver.Id, LicenseCategory.Id);

                        LicenseCategories.Remove(LicenseCategory);
                        GetVehiclesList();
                    }
                }
            }
        }
    }
}
