using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class EmployeeContractModel
    {

        public UserDetails User { get; set; }

        public IEnumerable<FieldData> FieldDataList { get; set; } 
        

            public class FieldData
            {
                public string Label { get; set; }

                public string Content { get; set; }
            }

            public class UserDetails
            {
                public string Username { get; set; }

                public string UserId { get; set; }

            }
    }

    
}
