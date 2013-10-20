using ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ECMS.Core.Framework;
using System.IO;
using CsvHelper;

namespace ECMS.Services.ContentRepository
{
    public class FileSystemRepository : ContentRepositoryBase
    {
        public static List<ContentItemHead> ContentHeadList = null;
        public static Dictionary<Guid, dynamic> ContentBodyList = null;

        static FileSystemRepository()
        {
            using (StreamReader streamReader = new StreamReader(@"J:\MyProjects\ecms\WebSite\App_Data\1\default-template.et"))
            {
                var csv = new CsvReader(streamReader);
                ContentHeadList = csv.GetRecords<ContentItemHead>().ToList<ContentItemHead>();
            }

            using (StreamReader streamReader = new StreamReader(@"J:\MyProjects\ecms\WebSite\App_Data\1\content.ect"))
            {
                var csv = new CsvReader(streamReader);
                ContentBodyList = new Dictionary<Guid, dynamic>();
                while (csv.Read())
                {
                    ContentBodyList[Guid.Parse(csv.GetField("UrlId"))] = csv.GetRecord(typeof(object));
                }
            }
        }

        public override ContentItem GetById(ValidUrl url_)
        {
            ContentItem item = new ContentItem();
            item.Url = url_;
            //item.Body = JsonConvert.DeserializeObject("{ \"FirstName\" : \"Vishal\", \"LastName\" : \"Sharma\", \"FlatNo\" : \"2104A\", \"Models\" : [{\"Make\":\"Maruti\",\"Model\":\"Alto\", \"Year\":\"@DateTime.Now.Year.ToString()\"},{\"Make\":\"Ranault\",\"Model\":\"Duster\", \"Year\":\"@DateTime.Now.Year.ToString()\"}]  }");
            item.Body = ContentBodyList[url_.Id];
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
            //TODO: dummy implementation to be change
            //ContentItemHead itemhead = JsonConvert.DeserializeObject<ContentItemHead>("{ \"Title\" : \"page title\", \"KeyWords\" : \"page keywords\", \"Description\" : \"test page\"}");
            ContentItemHead itemhead = ContentHeadList[0];
            return itemhead;
        }
    }
}
