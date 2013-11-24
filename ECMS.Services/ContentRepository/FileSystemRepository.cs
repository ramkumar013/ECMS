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
        public static Dictionary<int,Dictionary<string, ContentItemHead>> ContentHeadList = null;
        public static Dictionary<int,Dictionary<Guid, dynamic>> ContentBodyList = null;  
      
        static FileSystemRepository()
        {
            string[] dirinfo = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\app_data");
            foreach (string dir in dirinfo)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                LoadMetaTags(dirInfo);
                LoadPageContents(dirInfo);
            }
        }

        private static void LoadMetaTags(DirectoryInfo dirInfo)
        {
            using (StreamReader streamReader = new StreamReader(dirInfo.FullName + "\\default-template.et"))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    ContentHeadList = new Dictionary<int, Dictionary<string, ContentItemHead>>();
                    var temp = new Dictionary<string, ContentItemHead>();
                    while (csv.Read())
                    {
                        temp[csv.GetField("ViewName")] = csv.GetRecord<ContentItemHead>();
                    }
                    ContentHeadList[Convert.ToInt32(dirInfo.Name)] = temp;
                }
            }
        }

        private static void LoadPageContents(DirectoryInfo dirInfo)
        {
            using (StreamReader streamReader = new StreamReader(dirInfo.FullName + "\\content.ect"))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    ContentBodyList = new Dictionary<int, Dictionary<Guid, dynamic>>();
                    var temp = new Dictionary<Guid, dynamic>();
                    while (csv.Read())
                    {
                        temp[Guid.Parse(csv.GetField("UrlId"))] = csv.GetRecord(typeof(object));
                    }
                    ContentBodyList[Convert.ToInt32(dirInfo.Name)] = temp;
                }
            }
        }
       

        public override ContentItem GetById(ValidUrl url_)
        {
            List<Task> tasks = new List<Task>();
            ContentItem item = new ContentItem();
            item.Url = url_;
            item.Body = ContentBodyList[url_.SiteId][url_.Id];

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
            ContentItemHead itemhead = ContentHeadList[url_.SiteId][url_.View];
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
