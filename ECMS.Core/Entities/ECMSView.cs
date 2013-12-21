using System;
using ECMS.Core.Framework;
namespace ECMS.Core.Entities
{
    public class ECMSView 
    {
        public Guid Id { get; set; }
        public string ViewHtml { get; set; }
        public string ViewName { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public int SiteId { get; set; }
        public ContentViewType ViewType { get; set; }
    }
}
