using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class UserSearchInfo
    {
        public int? Id { get; set; }
        public int LanguageId { get; set; }
        public int UserTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PartialAddress { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string HrefLocation { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        public IEnumerable<ListItemStruct> UserList { get; set; }
        public IEnumerable<ListItemStruct> DepartmentList { get; set; }
    }
}
