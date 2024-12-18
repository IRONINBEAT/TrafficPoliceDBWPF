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

namespace TrafficPoliceVehiclesDLL
{
    /// <summary>
    /// Логика взаимодействия для VehiclesWindow.xaml
    /// </summary>
    public partial class VehiclesWindow : Window
    {
        public VehiclesWindow()
        {
            InitializeComponent();

            DataContext = new VehiclesWindowViewModel(this);

            GenerateVehiclesDataGridColumns(VehiclesDataGrid, typeof(Vehicle));

            GenerateDriversDataGridColumns(DriversDataGrid, typeof(Driver));

            GenerateLicenseCategoriesDataGridColumns(CategoriesDataGrid, typeof(LicenseCategory));
        }

        private void GenerateVehiclesDataGridColumns(DataGrid dataGrid, Type modelType)
        {
            foreach (PropertyInfo prop in modelType.GetProperties())
            {
                // Читаем атрибут DisplayName
                var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                string header = displayNameAttribute != null ? displayNameAttribute.DisplayName : prop.Name;

                // Если имя свойства заканчивается на "Id", то нам нужно показать соответствующее "Title"
                if (prop.Name.EndsWith("Id"))
                {
                    continue;
                }

                else if (prop.Name == "DateOfRegistration" ||
                         prop.Name == "TechnicalTicketDateOfRelease" ||
                         prop.Name == "DateOfRelease" ||
                         prop.Name == "AWD" ||
                         prop.Name == "SteeringWheelOrientation" ||
                         prop.Name == "IsLeftOrientation" ||
                         prop.Name == "IsRightOrientation") continue;

                else if (prop.Name == "DateOfRegistration" ||
                         prop.Name == "TechnicalTicketDateOfRelease" ||
                         prop.Name == "DateOfRelease")
                {
                    // Для полей типа DateTime создаем колонки с правильным сортированием
                    var column = new DataGridTextColumn
                    {
                        Header = header,
                        Binding = new Binding(prop.Name), // Привязка к строковому свойству
                        SortMemberPath = prop.Name + "Value" // Указываем свойство типа DateTime для сортировки
                    };

                    dataGrid.Columns.Add(column);
                }
                else
                {
                    // Для всех остальных свойств (не заканчивающихся на "Id" и не являющихся датой) создаем обычные колонки
                    var column = new DataGridTextColumn
                    {
                        Header = header,
                        Binding = new Binding(prop.Name) // Привязка к данному свойству
                    };

                    dataGrid.Columns.Add(column);
                }
            }
        }

        private void GenerateDriversDataGridColumns(DataGrid dataGrid, Type modelType)
        {
            foreach (PropertyInfo prop in modelType.GetProperties())
            {
                // Читаем атрибут DisplayName
                var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                string header = displayNameAttribute != null ? displayNameAttribute.DisplayName : prop.Name;

                // Если имя свойства заканчивается на "Id", то нам нужно показать соответствующее "Title"
                if (prop.Name == "DriverFullName" || prop.Name == "PhoneNumber")
                {
                    // Для всех остальных свойств (не заканчивающихся на "Id" и не являющихся датой) создаем обычные колонки
                    var column = new DataGridTextColumn
                    {
                        Header = header,
                        Binding = new Binding(prop.Name) // Привязка к данному свойству
                    };

                    dataGrid.Columns.Add(column);
                }

                else
                {
                    continue;
                }
            }
        }

        private void GenerateLicenseCategoriesDataGridColumns(DataGrid dataGrid, Type modelType)
        {
            foreach (PropertyInfo prop in modelType.GetProperties())
            {
                // Читаем атрибут DisplayName
                var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                string header = displayNameAttribute != null ? displayNameAttribute.DisplayName : prop.Name;

                // Если имя свойства заканчивается на "Id", то нам нужно показать соответствующее "Title"
                if (prop.Name.EndsWith("Id"))
                {
                    continue;
                }

                else
                {
                    // Для всех остальных свойств (не заканчивающихся на "Id" и не являющихся датой) создаем обычные колонки
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
}
