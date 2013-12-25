using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Entities
{
    public class ContentItem
    {
        public ContentItemHead Head { get; set; }
        public Guid ContentId { get; set; }
        public dynamic Body { get; set; }
        public ValidUrl Url { get; set; }
    }

    public class ContentItemHead
    {
        public string Title { get; set; }
        public string KeyWords { get; set; }
        public string Description { get; set; }
        public string PageMetaTags { get; set; }
    }
}
