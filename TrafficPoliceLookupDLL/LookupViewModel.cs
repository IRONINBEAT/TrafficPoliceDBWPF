using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrafficPoliceDomainDLL;
using TrafficPoliceDomainDLL.model;


namespace TrafficPoliceLookupDLL
{
    public class LookupViewModel : ViewModelBase
    {
        private string _searchQuery;
        public ObservableCollection<LookupItem> AllItems { get; set; }
        public ObservableCollection<LookupItem> FilteredItems { get; set; }
        public LookupItem SelectedItem { get; set; }

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
                FilterItems(); // Вызываем фильтрацию при изменении строки поиска
            }
        }

        public LookupViewModel(IEnumerable<LookupItem> items)
        {
            AllItems = new ObservableCollection<LookupItem>(items);
            FilteredItems = new ObservableCollection<LookupItem>(AllItems);

            SelectCommand = new RelayCommand(SelectItem);
        }

        private void SelectItem(object parameter)
        {
            OnItemSelected?.Invoke(SelectedItem);
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
    }
}
