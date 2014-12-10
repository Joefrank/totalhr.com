using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.data.TimeRecordingSystem.Models;

namespace totalhr.data.TimeRecordingSystem.Models
{
    public class Audit
    {
        public int AddedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public Audit(){ }
        public Audit(int addedByUserId, int? updatedByUserId, DateTime dateAdded, DateTime? dateUpdated)
        {
            AddedByUserId = addedByUserId;
            UpdatedByUserId = UpdatedByUserId;
            DateAdded = dateAdded;
            DateUpdated = dateUpdated;

        }

    }
}
