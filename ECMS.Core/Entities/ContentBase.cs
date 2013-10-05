using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Entities
{
    public class ContentBase
    {
        public string Title { get; set; }
        public string KeyWords { get; set; }
        public string Description { get; set; }
        public string CopyRight { get; set; }
        public string PageMetaTags { get; set; }
        public Guid ContentId { get; set; }
    }
}
