using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Entities
{
    public class TemplateSettings
    {
        public int OutputCacheDuration { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string MetaTags { get; set; }
        public bool IsActive { get; set; }
    }
}
