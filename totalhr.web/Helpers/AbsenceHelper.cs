using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.Shared;

namespace totalhr.web.Helpers
{
    public class AbsenceHelper
    {
        public static string GetAbsenceStatusClass(Variables.AbsenceRequestStatus status)
        {
            switch (status)
            {
                case Variables.AbsenceRequestStatus.NewAbsence:
                    return "label-info";
                case Variables.AbsenceRequestStatus.ApprovedAbsence:
                    return "label-success";
                case Variables.AbsenceRequestStatus.RejectedAbsence:
                    return "label-important";
                default: return "";
            }
        }
    }
}