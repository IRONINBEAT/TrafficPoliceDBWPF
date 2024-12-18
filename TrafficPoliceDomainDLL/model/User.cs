using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPoliceDomainDLL.model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool R { get; set; }
        public bool W { get; set; }
        public bool E { get; set; }
        public bool D { get; set; }
    }
}
