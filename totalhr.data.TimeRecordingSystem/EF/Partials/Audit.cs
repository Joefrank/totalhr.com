using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.data.TimeRecordingSystem.EF
{

    public partial class Audit
    {
        public string AddedByUserName { get; set; }
        public Audit() { }
        public Audit(int addedByUserId, DateTime dateAdded)
        {
           
            AddedBy = addedByUserId;
            
            AddedDate = dateAdded;
           

        }

        public Audit UpdateAudit( int? updatedByUserId, DateTime? dateUpdated)
        {
            UpdatedBy = updatedByUserId;
            UpdatedDate = dateUpdated;
            return this;
        }

    }
}
