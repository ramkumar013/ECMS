using ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ECMS.Core.Framework;

namespace ECMS.Services.ContentRepository
{
    public class FileSystemRepository : ContentRepositoryBase
    {
        public override ContentItem GetById(ValidUrl url_)
        {
            ContentItem item = new ContentItem();
            item.Url = url_;
            item.Body = JsonConvert.DeserializeObject("{ \"FirstName\" : \"Vishal\", \"LastName\" : \"Sharma\", \"FlatNo\" : \"2104A\", \"Models\" : [{\"Make\":\"Maruti\",\"Model\":\"Alto\", \"Year\":\"@DateTime.Now.Year.ToString()\"},{\"Make\":\"Ranault\",\"Model\":\"Duster\", \"Year\":\"@DateTime.Now.Year.ToString()\"}]  }");
            item.Head = GetHeadContentByViewName(url_);
            return item;
        }

        public override ContentItem GetByUrl(ValidUrl url_)
        {
            throw new NotImplementedException();
        }

        public override void Save(ContentItem content_)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ContentItem content_)
        {
            throw new NotImplementedException();
        }

        public override ContentItemHead GetHeadContentByViewName(ValidUrl url_)
        {
            throw new NotImplementedException();
        }
    }
}
