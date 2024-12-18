using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrafficPoliceDomainDLL.model;

namespace TrafficPoliceDirectoryManagementDLL
{
    /// <summary>
    /// Логика взаимодействия для DirectoryManagement.xaml
    /// </summary>
    public partial class DirectoryManagement : Window
    {
        public DirectoryManagement(string tableName)
        {
            InitializeComponent();

            DataContext = new DirectoryManagementViewModel(tableName);

            GenerateCommonTable(DirectoryDataGrid, typeof(LookupItem));

            GenerateCommonTable(ModelsDataGrid, typeof(LookupItem));
        }

        private void GenerateCommonTable(DataGrid dataGrid, Type modelType)
        {
            foreach (PropertyInfo prop in modelType.GetProperties())
            {
                // Читаем атрибут DisplayName
                var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();

                string header = displayNameAttribute != null ? displayNameAttribute.DisplayName : prop.Name;

                var column = new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding(prop.Name) // Привязка к данному свойству
                };

                dataGrid.Columns.Add(column);
            }
        }
    }
}
