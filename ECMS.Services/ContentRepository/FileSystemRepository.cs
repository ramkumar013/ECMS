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
using RazorEngine.Templating;
using ECMS.Core;

namespace ECMS.Services.ContentRepository
{
    public class FileSystemRepository : ContentRepositoryBase
    {
        public static Dictionary<string, ContentItemHead> ContentHeadList = null;
        public static Dictionary<Guid, dynamic> ContentBodyList = null;

        static FileSystemRepository()
        {
            using (StreamReader streamReader = new StreamReader(@"J:\MyProjects\ecms\WebSite\App_Data\1\default-template.et"))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    ContentHeadList = new Dictionary<string, ContentItemHead>();
                    while (csv.Read())
                    {
                        ContentHeadList[csv.GetField("ViewName")] = csv.GetRecord<ContentItemHead>();
                    }
                }
            }

            using (StreamReader streamReader = new StreamReader(@"J:\MyProjects\ecms\WebSite\App_Data\1\content.ect"))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    ContentBodyList = new Dictionary<Guid, dynamic>();
                    while (csv.Read())
                    {
                        ContentBodyList[Guid.Parse(csv.GetField("UrlId"))] = csv.GetRecord(typeof(object));
                    }
                }
            }
        }

        public override ContentItem GetById(ValidUrl url_)
        {
            List<Task> tasks = new List<Task>();
            ContentItem item = new ContentItem();
            item.Url = url_;
            item.Body = ContentBodyList[url_.Id];

            //TODO: optimize this looping & serializing.
            using (TextReader sreader = new StringReader(JsonConvert.SerializeObject(item.Body)))
            {
                JsonReader jreader = new JsonTextReader(sreader);
                while (jreader.Read())
                {
                    if (jreader.TokenType == JsonToken.String && jreader.Value.ToString().Contains("@"))
                    {
                        var temp = jreader.Value.ToString();
                        var task = Task.Factory.StartNew(() => CreateTemplateAndSetInCache(temp));
                        tasks.Add(task);
                    }
                }
                DependencyManager.CachingService.Set<List<Task>>("Task." + url_.Id, tasks);
            }
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
            ContentItemHead itemhead = ContentHeadList[url_.View];
            return itemhead;
        }

        private void CreateTemplateAndSetInCache(string template_)
        {
            TemplateService service = new TemplateService();
            ITemplate template = service.CreateTemplate(template_, null, null);
            DependencyManager.CachingService.Set<ITemplate>(template_.GetHashCode().ToString(), template);
        }
    }
}
