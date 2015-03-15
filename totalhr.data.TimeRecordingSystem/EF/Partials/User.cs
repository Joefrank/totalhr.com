using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.data.TimeRecordingSystem.EF
{
    public partial class User
    {

        public string FullName
        {
            get
            {
                return firstname + " " + surname;
            }
        }

    }
}
