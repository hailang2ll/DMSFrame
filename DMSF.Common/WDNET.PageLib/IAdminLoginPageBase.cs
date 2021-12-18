using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDNET.PageLib
{
    public interface IAdminLoginPageBase
    {
        int UserID { get; }
        string UserName { get; }
    }
}
