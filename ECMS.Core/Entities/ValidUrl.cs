using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Entities
{
    public class ValidUrl
    {
        public string FriendlyUrl { get; set; }
        public string View { get; set; }
        public bool Index { get; set; }
        public bool Active { get; set; }
        public DateTime LastModified { get; set; }
        public int StatusCode { get; set; }
    }
}
