using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace totalhr.services.Infrastructure
{
    public interface IProfileService
    {
        IEnumerable<Profile> GetProfileList();
    }
}
