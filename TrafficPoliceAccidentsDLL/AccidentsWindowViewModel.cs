using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrafficPoliceDomainDLL;
using TrafficPoliceDomainDLL.model;
using TrafficPoliceDomainDLL.repository;
using TrafficPoliceLookupDLL;

namespace TrafficPoliceAccidentsDLL
{
    public class AccidentsWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Accident_Vehicle> _accidents;
        private ObservableCollection<Vehicle> _participateVehicles;
        private Accident_Vehicle _selectedAccident;
        private Vehicle _selectedVehicle;
        private string _severity;
        private bool _isAddedParticipates = false;


        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicle));
            }
        }
        public string Severity
        {
            get { return _severity; }
            set
            {
                _severity = value;
                if (_severity == "лёгкое")
                {
                    Accident.Severity = "лёгкое";
                }
                else if (_severity == "средней тяжести")
                {
                    Accident.Severity = "средней тяжести";
                }
                else if (_severity == "тяжёлое")
                {
                    Accident.Severity = "тяжёлое";
                }
                OnPropertyChanged(nameof(Accident.Severity));
                OnPropertyChanged(nameof(Severity));
            }
        }
        public ICommand SaveChanges { get; }

        public ICommand OpenStreetLookupCommand { get; }

        public ICommand OpenInspectorLookupCommand { get; }

        public ICommand AddAccidentParticipateCommand { get; }

        public ICommand AddAccidentCommand { get; }

        public ICommand RemoveSelectedVehicleCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        private bool _isEdit = false;

        public bool IsEdit
        {
            get => _isEdit;
            set { _isEdit = value; OnPropertyChanged(nameof(IsEdit)); }
        }

        private bool _isVisibleOnAdd = true;

        public bool IsVisibleOnAdd
        {
            get => _isVisibleOnAdd;
            set { _isVisibleOnAdd = value; OnPropertyChanged(nameof(IsVisibleOnAdd)); }
        }

        private bool _isAddParticipates = false;

        public bool IsAddParticipates { get => _isAddParticipates; set => _isAddParticipates = value; }

        private bool _isAddAccident = false;

        public bool IsAddAccident
        {
            get => _isAddAccident;
            set { _isAddAccident = value; OnPropertyChanged(nameof(IsAddAccident)); }
        }

        public ObservableCollection<Vehicle> ParticipateVehicles
        {
            get { return _participateVehicles; }
            set
            {
                _participateVehicles = value;
                OnPropertyChanged(nameof(ParticipateVehicles));
            }
        }
        private string _searchText;
        private DateTime? _searchDate;

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
        public ICommand OpenAccodentDetailsCommand { get; }

        public ObservableCollection<Accident_Vehicle> Accidents
        {
            get => _accidents;
            set
            {
                _accidents = value;
                OnPropertyChanged(nameof(Accidents));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    PerformSearch(); // Выполняем фильтрацию при изменении строки поиска
                }
            }
        }

        public DateTime? SearchDate
        {
            get => _searchDate;
            set
            {
                _searchDate = value;
                OnPropertyChanged(nameof(SearchDate));
                PerformSearch();
            }
        }

        public Accident_Vehicle SelectedAccident
        {
            get => _selectedAccident;
            set
            {
                _selectedAccident = value;

                var accidentRepo = new AccidentRepository();
                Accident = accidentRepo.GetAccidentInfoById(_selectedAccident.AccidentId);

                GetAccidentParticipates();

                if (Accident != null) { Severity = Accident.Severity; }



                OnPropertyChanged(nameof(Accident));
                OnPropertyChanged(nameof(SelectedAccident));
                OnPropertyChanged(nameof(Severity));

            }
        }

        private Accident _accident;

        public Accident Accident
        {
            get => _accident;
            set
            {
                _accident = value;
                OnPropertyChanged(nameof(Accident));
            }
        }

        public AccidentsWindowViewModel()
        {
            SaveChanges = new RelayCommand(o => SaveAccidentInfo());
            OpenStreetLookupCommand = new RelayCommand(o => OpenStreetLookup());
            OpenInspectorLookupCommand = new RelayCommand(o => OpenInspectorLookup());
            AddAccidentParticipateCommand = new RelayCommand(o => AddAccidentParticipate());
            RemoveSelectedVehicleCommand = new RelayCommand(o => RemoveSelectedVehicle());
            EditCommand = new RelayCommand(o => OnEditAccident());
            AddAccidentCommand = new RelayCommand(o => OnAddAccident());
            CancelCommand = new RelayCommand(o => OnCancel());

            GetAccidentsList();

            SelectedAccident = Accidents.FirstOrDefault();

            SelectedTabIndex = 0;
        }


        private void GetAccidentsList()
        {
            // Получаем список происшествий из репозитория
            var accidentRepo = new AccidentRepository();
            var accidentsList = accidentRepo.GetAllAccidents();

            Accidents = new ObservableCollection<Accident_Vehicle>(accidentsList);
        }

        private void PerformSearch()
        {
            if (string.IsNullOrEmpty(SearchText) && SearchDate == null)
            {
                // Если оба фильтра пусты, показываем все записи
                GetAccidentsList();
                return;
            }

            var searchTextLower = SearchText?.ToLower();
            DateTime? searchDate = SearchDate;

            var filteredAccidents = _accidents.Where(a =>
                // Фильтрация по гос. номеру
                (string.IsNullOrEmpty(searchTextLower) ||
                 (a.StateRegistrationNumber != null && a.StateRegistrationNumber.ToLower().Contains(searchTextLower)))
                &&
                // Фильтрация по дате
                (searchDate == null ||
                 a.Date.Date == searchDate.Value.Date) // Сравниваем только даты
            ).ToList();

            // Обновляем коллекцию для отображения отфильтрованных данных
            Accidents = new ObservableCollection<Accident_Vehicle>(filteredAccidents);
        }

        private bool CanOpenAccidentDetails(object parameter)
        {

            return SelectedAccident != null;
        }

        public void GetAccidentParticipates()
        {
            var vehicles = new List<Vehicle>();

            var acccidentRepo = new AccidentRepository();
            var accidentById = acccidentRepo.GetAccidentInfoById(SelectedAccident.AccidentId);

            var vehiclesRepo = new VehicleRepository();

            foreach (var accident in Accidents)
            {
                if (accident.AccidentId == SelectedAccident.AccidentId)
                {
                    vehicles.Add(vehiclesRepo.GetVehicleByVehicleId(accident.VehicleId));
                }
            }

            ParticipateVehicles = new ObservableCollection<Vehicle>(vehicles);
            OnPropertyChanged(nameof(ParticipateVehicles));
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
                    Accident.InspectorId = selectedItem.Id;
                    var fullName = selectedItem.Title.Split(' ')
                                 .Take(3)
                                 .Aggregate((part1, part2) => part1 + " " + part2);
                    Accident.InspectorFullName = fullName;
                    OnPropertyChanged(nameof(Accident.InspectorId));
                    OnPropertyChanged(nameof(Accident.InspectorFullName));
                    lookupWindow.Close();
                }
            };

            lookupWindow.DataContext = lookupViewModel;
            lookupWindow.ShowDialog();
        }

        public void SaveAccidentInfo()
        {
            if (ValidAccident())
            {


                if (_isAddAccident == false && _isEdit == true)
                {
                    var userResponse = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (userResponse == MessageBoxResult.Yes)
                    {
                        var accidentRepo = new AccidentRepository();

                        accidentRepo.UpdateAccident(Accident);

                        if (_isAddedParticipates == true)
                        {
                            accidentRepo.AddAccidentParticipate(Accident, ParticipateVehicles.ToList());
                            _isAddedParticipates = false;
                        }

                        IsEdit = false;
                        GetAccidentsList();
                    }
                }
                else
                {
                    var accidentRepo = new AccidentRepository();

                    var accidentId = accidentRepo.AddAccident(Accident);
                    if (accidentId == 0)
                    {
                        MessageBox.Show("Ошибка при добавлении ДТП.");
                        return;
                    }
                    Accident.Id = accidentId;
                    Accident.Severity = Severity;

                    if (ParticipateVehicles.Count() == 0)
                    {
                        MessageBox.Show("Вы не указали участников ДТП!");
                        return;
                    }

                    accidentRepo.AddAccidentParticipate(Accident, ParticipateVehicles.ToList());

                    IsAddAccident = false;
                    IsEdit = false;
                    GetAccidentsList();
                }
            }
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
                    Accident.StreetId = selectedItem.Id;  // Прямо модифицируем переданный объект
                    Accident.StreetTitle = selectedItem.Title;
                    OnPropertyChanged(nameof(Accident.StreetId));
                    OnPropertyChanged(nameof(Accident.StreetTitle));
                    lookupWindow.Close();
                }
            };

            lookupWindow.DataContext = lookupViewModel;
            lookupWindow.ShowDialog();

        }

        private void AddAccidentParticipate()
        {
            var lookupRepository = new LookupRepository();
            var cars = lookupRepository.GetLookupItems("vehicle");
            var lookupWindow = new LookupWindow();
            var lookupViewModel = new LookupViewModel(cars);

            lookupViewModel.OnItemSelected = selectedItem =>
            {
                if (selectedItem != null)
                {
                    // Проверяем, есть ли участник с таким же Id в списке
                    if (ParticipateVehicles.Any(vehicle => vehicle.Id == selectedItem.Id))
                    {
                        MessageBox.Show(
                            "Этот участник уже добавлен.",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return; // Завершаем выполнение, если участник уже есть
                    }

                    // Добавляем нового участника
                    ParticipateVehicles.Add(new Vehicle
                    {
                        Id = selectedItem.Id,
                        StateRegistrationNumber = selectedItem.Title.Split(' ')[0],
                        OwnerNameTitle = selectedItem.Title.IndexOf("владелец: ") != -1
                                        ? selectedItem.Title.Substring(selectedItem.Title.IndexOf("владелец: ") + "владелец: ".Length)
                                        : string.Empty
                    });

                    OnPropertyChanged(nameof(ParticipateVehicles));
                    _isAddedParticipates = true;
                    lookupWindow.Close();
                }
            };

            lookupWindow.DataContext = lookupViewModel;
            lookupWindow.ShowDialog();
        }

        private void RemoveSelectedVehicle()
        {
            if (SelectedVehicle != null)
            {
                var userResponse = MessageBox.Show("Вы уверены, что хотите удалить участника ДТП?\nЕсли он один, то запись о ДТП удалится без возможности восстановления.", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);


                if (userResponse == MessageBoxResult.Yes)
                {
                    var accidentRepo = new AccidentRepository();
                    var isSuccess = accidentRepo.RemoveAccidentParticipate(SelectedVehicle.Id, Accident.Id);

                    if (isSuccess)
                    {
                        ParticipateVehicles.Remove(SelectedVehicle);
                        OnPropertyChanged(nameof(ParticipateVehicles));
                        MessageBox.Show("Участник ДТП успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        GetAccidentsList();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить участника ДТП.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите участника ДТП для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnEditAccident()
        {
            IsEdit = true;
        }

        private void OnAddAccident()
        {
            SelectedAccident = new Accident_Vehicle();
            Accident = new Accident();
            ParticipateVehicles = new ObservableCollection<Vehicle>();
            IsAddAccident = true;
            SelectedTabIndex = 1;
        }

        private void OnCancel()
        {
            IsAddAccident = false;
            IsEdit = false;
        }

        private bool ValidAccident()
        {
            if (ParticipateVehicles.Count == 0)
            {
                MessageBox.Show("Не указаны участники ДТП!");
                return false;
            }
            if (Accident.Date == DateTime.MinValue)
            {
                MessageBox.Show("Не указана дата ДТП!");
                return false;
            }
            if (Accident.InspectorId == 0 || string.IsNullOrEmpty(Accident.InspectorFullName))
            {
                MessageBox.Show("Инспектор не указан!");
                return false;
            }
            if (string.IsNullOrEmpty(Severity))
            {
                MessageBox.Show("Степень тяжести не указана!");
                return false;
            }
            if (Accident.StreetId == 0 || string.IsNullOrEmpty(Accident.StreetTitle))
            {
                MessageBox.Show("Улица не указана!");
                return false;
            }
            if (string.IsNullOrEmpty(Accident.NearHouseNumber))
            {
                MessageBox.Show("Ближайший дом не указан!");
                return false;
            }
            if (string.IsNullOrEmpty(Accident.Description))
            {
                MessageBox.Show("Описание не указано!");
                return false;
            }
            return true;
        }
    }
}
