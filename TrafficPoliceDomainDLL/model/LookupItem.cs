using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class LookupItem
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Наименование")]
        public string Title { get; set; }
    }
}
