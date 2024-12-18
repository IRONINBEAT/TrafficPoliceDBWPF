using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class LicenseCategory : ViewModelBase
    {

        public int Id { get; set; }
        [DisplayName("Код")]
        public string Code { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
    }
}
