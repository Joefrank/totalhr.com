using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.Shared.Models
{
    public class ContractFillViewInfo
    {
         public int FormId { get; set; }
         public int ContractId  { get; set; }
         public int UserId  { get; set; }
         public int FormTypeId  { get; set; }
         public string Data { get; set; }
         public int CreatedBy { get; set; }
    }
}