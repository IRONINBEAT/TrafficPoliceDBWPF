using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class Accident_Vehicle
    {
        public int Id { get; set; }

        public int AccidentId { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Регистрационный номер")]
        public string StateRegistrationNumber { get; set; }

        [DisplayName("Дата")]
        public string FormattedDate => Date.ToString("d");

        [DisplayName("Степень тяжести")]
        public string Severty { get; set; }

        public int VehicleId { get; set; }



    }
}
