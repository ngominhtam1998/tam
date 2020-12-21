using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bán_hàng.Areas.Admin.Models
{
    [Serializable]
    public class ReponsResult
    {
        public bool IsSuccess { get; set; }
        public string Mescode { get; set; }
        public string MesTring { get; set; }
        public ReponsResult(bool _IsSuccess, string _Mescode, string _MesTring)
        {
            this.IsSuccess = _IsSuccess;
            this.Mescode = _Mescode;
            this.MesTring = _MesTring;
        }
    }
}
