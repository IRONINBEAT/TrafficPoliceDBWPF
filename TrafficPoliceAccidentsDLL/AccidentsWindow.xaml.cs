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

namespace TrafficPoliceAccidentsDLL
{
    /// <summary>
    /// Логика взаимодействия для AccidentsWindow.xaml
    /// </summary>
    public partial class AccidentsWindow : Window
    {
        public AccidentsWindow()
        {
            InitializeComponent();

            DataContext = new AccidentsWindowViewModel();

            GenerateAccidentsDataGridColumns(AccidentsDataGrid, typeof(Accident_Vehicle));

            GenerateAccidentParticipatesColumns(ParticipatesDataGrid, typeof(Vehicle));
        }


        private void GenerateAccidentsDataGridColumns(DataGrid dataGrid, Type modelType)
        {
            foreach (PropertyInfo prop in modelType.GetProperties())
            {
                // Читаем атрибут DisplayName
                var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                string header = displayNameAttribute != null ? displayNameAttribute.DisplayName : prop.Name;

                // Если имя свойства заканчивается на "Id", то нам нужно показать соответствующее "Title"
                if (prop.Name.EndsWith("Id") || prop.Name == "Date")
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

        private void GenerateAccidentParticipatesColumns(DataGrid dataGrid, Type modelType)
        {
            foreach (PropertyInfo prop in modelType.GetProperties())
            {
                // Читаем атрибут DisplayName
                var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                string header = displayNameAttribute != null ? displayNameAttribute.DisplayName : prop.Name;

                // Если имя свойства заканчивается на "Id", то нам нужно показать соответствующее "Title"
                if (prop.Name == "StateRegistrationNumber" || prop.Name == "OwnerNameTitle")
                {

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
    }
}
