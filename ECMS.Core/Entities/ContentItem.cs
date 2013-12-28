using System;
using ECMS.Core.Framework;

namespace ECMS.Core.Entities
{
    public class ContentItem
    {
        public Guid Id { get; set; }
        public ContentItem() {
            ContentId = Guid.NewGuid();
        }
        public ContentItemHead Head { get; set; }
        public Guid ContentId { get; set; }
        public dynamic Body { get; set; }
        public ValidUrl Url { get; set; }
        public ECMSView ContentView { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }

    public class ContentItemHead
    {
        public string Title { get; set; }
        public string KeyWords { get; set; }
        public string Description { get; set; }
        public string PageMetaTags { get; set; }
    }
}
