using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrafficPoliceDomainDLL.model;
using TrafficPoliceDomainDLL;
using TrafficPoliceDomainDLL.repository;
using TrafficPoliceLookupDLL;

namespace TrafficPoliceDirectoryManagementDLL
{
    public class DirectoryManagementViewModel : ViewModelBase
    {
        public string TextBlockName { get; set; } = "Наименование";

        public string TextBoxContent { get; set; } = "Наименование";
        public string CheckBoxName { get; set; } = "test";

        private string _tableName;

        public string TextBlockModelName { get; set; }

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
        public ICommand EditCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand AddCommand { get; }

        public ICommand DeleteCommand { get; }
        public bool IsAddOrEdit { get; set; } = false;

        private bool IsAdd { get; set; }

        private bool IsEdit { get; set; }

        public bool IsBrandAndModel { get; set; } = false;

        public bool IsInspectorDirectory { get; set; } = false;

        public ICommand SelectPostCommand { get; }

        private DirectoryManagementRepository _directoryManagementRepository;

        private string _searchQuery;

        private string _searchQueryModels;

        public Inspector SelectedInspector { get; set; }
        public ObservableCollection<LookupItem> AllItems { get; set; }
        public ObservableCollection<LookupItem> FilteredItems { get; set; }

        public ObservableCollection<LookupItem> AllModels { get; set; }
        public ObservableCollection<LookupItem> FilteredModels { get; set; }

        public ObservableCollection<String> AllPosts { get; set; }

        public LookupItem SelectedPost { get; set; }

        private LookupItem _selectedItem;
        public LookupItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;


                TextBoxContent = SelectedItem.Title;


                AllModels = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetModelsByBrandId(SelectedItem.Id));
                FilteredModels = new ObservableCollection<LookupItem>(AllModels);

                if (_tableName == "inspector")
                {
                    SelectedInspector = _directoryManagementRepository.GetInspectorById(SelectedItem.Id);
                    SelectedPost = _directoryManagementRepository.GetPostById(SelectedInspector.PostId);
                }



                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(TextBoxContent));
                OnPropertyChanged(nameof(AllModels));
                OnPropertyChanged(nameof(FilteredModels));
                OnPropertyChanged(nameof(SelectedInspector));
            }
        }

        private LookupItem _selectedModel;

        public LookupItem SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                TextBlockModelName = SelectedModel.Title;
                OnPropertyChanged(nameof(SelectedModel));
                OnPropertyChanged(nameof(TextBlockModelName));
            }
        }
        public Action<LookupItem> OnItemSelected { get; set; }

        public ICommand SelectCommand { get; }
        public ICommand SearchCommand { get; }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                FilterItems();
            }
        }

        public string SearchQueryModels
        {
            get => _searchQuery;
            set
            {
                _searchQueryModels = value;
                OnPropertyChanged(nameof(SearchQueryModels));
                FilterModels();
            }
        }

        public DirectoryManagementViewModel(string tableName)
        {
            _tableName = tableName;
            _directoryManagementRepository = new DirectoryManagementRepository();
            SelectPostCommand = new RelayCommand(o => SelectPost());
            EditCommand = new RelayCommand(o => Edit());
            CancelCommand = new RelayCommand(o => Cancel());
            SaveCommand = new RelayCommand(o => SaveChanges());
            AddCommand = new RelayCommand(o => Add());
            DeleteCommand = new RelayCommand(o => Delete());
            GenerateTable();
        }

        public void GenerateTable()
        {
            if (_tableName == "street")
            {
                AllItems = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetAllStreets());
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);

                OnPropertyChanged(nameof(AllItems));
            }
            if (_tableName == "colour")
            {
                AllItems = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetAllColors());
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);
                OnPropertyChanged(nameof(AllItems));
            }
            if (_tableName == "post")
            {
                AllItems = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetAllPosts());
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);
                OnPropertyChanged(nameof(AllItems));
            }
            if (_tableName == "license_category")
            {
                AllItems = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetAllLicenseCategories());
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);
                TextBlockName = "Код и описание через запятую";
                OnPropertyChanged(nameof(AllItems));
            }
            if (_tableName == "body_model")
            {
                AllItems = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetAllBodyModels());
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);
                OnPropertyChanged(nameof(AllItems));
            }
            if (_tableName == "inspector")
            {
                AllItems = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetAllInspectors());
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);



                IsInspectorDirectory = true;
                TextBlockName = "ФИО через пробел";
                OnPropertyChanged(nameof(AllItems));
            }
            if (_tableName == "car_brand")
            {
                IsBrandAndModel = true;

                AllItems = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetAllBrands());
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);
                SelectedItem = FilteredItems.FirstOrDefault();


                AllModels = new ObservableCollection<LookupItem>(_directoryManagementRepository.GetModelsByBrandId(SelectedItem.Id));
                FilteredModels = new ObservableCollection<LookupItem>(AllModels);
            }


            OnPropertyChanged(nameof(AllItems));
            OnPropertyChanged(nameof(FilteredItems));
            OnPropertyChanged(nameof(AllModels));
            OnPropertyChanged(nameof(FilteredModels));
        }

        private void FilterItems()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                // Если строка поиска пуста, показываем все элементы
                FilteredItems = new ObservableCollection<LookupItem>(AllItems);
            }
            else
            {
                // Приводим строки к одному регистру для сравнения
                var filtered = AllItems
                    .Where(item => item.Title != null &&
                                   item.Title.ToLower().Contains(SearchQuery.ToLower()))
                    .ToList();

                FilteredItems = new ObservableCollection<LookupItem>(filtered);
            }

            OnPropertyChanged(nameof(FilteredItems));
        }

        private void FilterModels()
        {
            if (string.IsNullOrEmpty(SearchQueryModels))
            {
                // Если строка поиска пуста, показываем все элементы
                FilteredModels = new ObservableCollection<LookupItem>(AllModels);
            }
            else
            {
                // Приводим строки к одному регистру для сравнения
                var filtered = AllModels
                    .Where(item => item.Title != null &&
                                   item.Title.ToLower().Contains(SearchQueryModels.ToLower()))
                    .ToList();

                FilteredModels = new ObservableCollection<LookupItem>(filtered);
            }

            OnPropertyChanged(nameof(FilteredModels));
        }

        public void SaveChanges()
        {
            if (IsAdd == false)
            {
                var userResponse = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (userResponse == MessageBoxResult.Yes)
                {

                    if (_tableName == "street")
                    {
                        _directoryManagementRepository.UpdateStreet(SelectedItem.Id, TextBoxContent);
                    }
                    if (_tableName == "colour")
                    {
                        _directoryManagementRepository.UpdateColor(SelectedItem.Id, TextBoxContent);
                    }
                    if (_tableName == "post")
                    {
                        _directoryManagementRepository.UpdatePost(SelectedItem.Id, TextBoxContent);
                    }
                    if (_tableName == "license_category")
                    {
                        if (!string.IsNullOrEmpty(TextBoxContent))
                        {
                            var parts = TextBoxContent.Split(new[] { ',' }, 2); // Разбиваем строку по запятой, максимум на 2 части

                            if (parts.Length == 2)
                            {
                                var code = parts[0].Trim();       // Обрезаем пробелы с обеих сторон у первой части
                                var description = parts[1].Trim(); // Обрезаем пробелы с обеих сторон у второй части

                                _directoryManagementRepository.UpdateLicenseCategory(SelectedItem.Id, code, description);
                            }
                            else
                            {
                                throw new ArgumentException("Некорректный формат TextBlockName. Ожидается: 'код, описание'.");
                            }
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(TextBoxContent), "TextBlockName не может быть пустым.");
                        }

                    }
                    if (_tableName == "inspector")
                    {
                        if (!string.IsNullOrWhiteSpace(TextBoxContent))
                        {
                            var parts = TextBoxContent.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (parts.Length >= 3)
                            {
                                var surname = parts[0].Trim();  // Фамилия
                                var name = parts[1].Trim();     // Имя
                                var patronymic = parts[2].Trim(); // Отчество

                                _directoryManagementRepository.UpdateInspector(SelectedItem.Id, name, surname, patronymic, SelectedInspector.PostId);
                            }
                            else
                            {
                                throw new ArgumentException("Некорректный формат TextBlockName. Ожидается: 'Фамилия Имя Отчество'.");
                            }
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(TextBlockName), "TextBlockName не может быть пустым.");
                        }

                    }
                    if (_tableName == "body_model")
                    {
                        _directoryManagementRepository.UpdateBodyModel(SelectedItem.Id, TextBoxContent);
                    }
                    if (_tableName == "car_brand")
                    {
                        _directoryManagementRepository.UpdateBrand(SelectedItem.Id, TextBoxContent);
                        if (SelectedModel != null)
                        {
                            _directoryManagementRepository.UpdateModel(SelectedModel.Id, TextBlockModelName, SelectedItem.Id);

                        }
                    }
                }

                IsEdit = false;
            }

            if (IsAdd == true)
            {
                if (_tableName == "street")
                {
                    _directoryManagementRepository.AddStreet(TextBoxContent);
                }
                if (_tableName == "colour")
                {
                    _directoryManagementRepository.AddColor(TextBoxContent);
                }
                if (_tableName == "post")
                {
                    _directoryManagementRepository.AddPost(TextBoxContent);
                }
                if (_tableName == "license_category")
                {
                    if (!string.IsNullOrEmpty(TextBoxContent))
                    {
                        var parts = TextBoxContent.Split(new[] { ',' }, 2); // Разбиваем строку по запятой, максимум на 2 части

                        if (parts.Length == 2)
                        {
                            var code = parts[0].Trim();       // Обрезаем пробелы с обеих сторон у первой части
                            var description = parts[1].Trim(); // Обрезаем пробелы с обеих сторон у второй части

                            _directoryManagementRepository.AddLicenseCategory(code, description);
                        }
                        else
                        {
                            throw new ArgumentException("Некорректный формат TextBlockName. Ожидается: 'код, описание'.");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(TextBoxContent), "TextBlockName не может быть пустым.");
                    }

                }
                if (_tableName == "inspector")
                {
                    if (!string.IsNullOrWhiteSpace(TextBoxContent))
                    {
                        var parts = TextBoxContent.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length >= 3)
                        {
                            var surname = parts[0].Trim();  // Фамилия
                            var name = parts[1].Trim();     // Имя
                            var patronymic = parts[2].Trim(); // Отчество

                            _directoryManagementRepository.AddInspector(name, surname, patronymic, SelectedInspector.PostId);
                        }
                        else
                        {
                            throw new ArgumentException("Некорректный формат TextBlockName. Ожидается: 'Фамилия Имя Отчество'.");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(TextBlockName), "TextBlockName не может быть пустым.");
                    }

                }
                if (_tableName == "body_model")
                {
                    _directoryManagementRepository.AddBodyModel(TextBoxContent);
                }
                if (_tableName == "car_brand")
                {
                    if (SelectedTabIndex == 0)
                        _directoryManagementRepository.AddBrand(TextBoxContent);
                    if (SelectedTabIndex == 1)
                    {
                        _directoryManagementRepository.AddModel(TextBlockModelName, SelectedItem.Id);

                    }

                }

                IsAdd = false;
            }

            IsAddOrEdit = false;
            GenerateTable();


            OnPropertyChanged(nameof(IsAddOrEdit));
            OnPropertyChanged(nameof(IsEdit));
            OnPropertyChanged(nameof(IsAdd));
        }

        private void SelectPost()
        {
            var lookupRepository = new LookupRepository();
            var posts = lookupRepository.GetLookupItems("post");
            var lookupWindow = new LookupWindow();
            var lookupViewModel = new LookupViewModel(posts);

            lookupViewModel.OnItemSelected = selectedItem =>
            {
                if (selectedItem != null)
                {
                    SelectedInspector.PostId = selectedItem.Id;  // Прямо модифицируем переданный объект
                    SelectedInspector.PostTitle = selectedItem.Title;
                    OnPropertyChanged(nameof(SelectedInspector.PostId));
                    OnPropertyChanged(nameof(SelectedInspector.PostTitle));
                    lookupWindow.Close();
                }
            };

            lookupWindow.DataContext = lookupViewModel;
            lookupWindow.ShowDialog();


        }

        private void Edit()
        {
            IsEdit = true;
            IsAddOrEdit = true;
            OnPropertyChanged(nameof(IsAddOrEdit));
            OnPropertyChanged(nameof(IsEdit));
        }

        private void Cancel()
        {
            IsAdd = false;


            IsEdit = false;

            IsAddOrEdit = false;

            OnPropertyChanged(nameof(IsAddOrEdit));
            OnPropertyChanged(nameof(IsAdd));
            OnPropertyChanged(nameof(IsEdit));
        }

        private void Add()
        {
            TextBoxContent = "";
            TextBlockModelName = "";
            IsAdd = true;
            IsAddOrEdit = true;

            if (_tableName == "inspector")
            {
                SelectedInspector = new Inspector();
            }

            OnPropertyChanged(nameof(IsAdd));
            OnPropertyChanged(nameof(IsAddOrEdit));
            OnPropertyChanged(nameof(TextBoxContent));
            OnPropertyChanged(nameof(TextBlockModelName));
        }

        private void Delete()
        {
            var userResponse1 = MessageBox.Show($"Вы уверены, что хотите удалить {SelectedItem.Title}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (userResponse1 == MessageBoxResult.Yes)
            {
                var userResponse2 = MessageBox.Show($"Подтвердите удаление?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (userResponse2 == MessageBoxResult.Yes)
                {
                    if (_tableName == "street")
                    {
                        _directoryManagementRepository.DeleteStreet(SelectedItem.Id);
                    }
                    if (_tableName == "colour")
                    {
                        _directoryManagementRepository.DeleteColor(SelectedItem.Id);
                    }
                    if (_tableName == "post")
                    {
                        _directoryManagementRepository.DeletePost(SelectedItem.Id);
                    }
                    if (_tableName == "license_category")
                    {
                        _directoryManagementRepository.DeleteLicenseCategory(SelectedItem.Id);
                    }
                    if (_tableName == "inspector")
                    {
                        _directoryManagementRepository.DeleteInspector(SelectedItem.Id);
                    }
                    if (_tableName == "body_model")
                    {
                        _directoryManagementRepository.DeleteBodyModel(SelectedItem.Id);
                    }
                    if (_tableName == "car_brand")
                    {
                        if (SelectedTabIndex == 0)
                            _directoryManagementRepository.DeleteBrand(SelectedItem.Id);
                        if (SelectedTabIndex == 1)
                        {
                            _directoryManagementRepository.DeleteModel(SelectedModel.Id);
                        }

                    }
                }
            }

            GenerateTable();
        }
    }
}
