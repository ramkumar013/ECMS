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
using Newtonsoft.Json.Linq;
using ECMS.Core.Extensions;

namespace ECMS.Services.ContentRepository
{
    public class FileSystemRepository : ContentRepositoryBase
    {
        private static Dictionary<int, Dictionary<string, JObject>> ContentHeadList = null;
        private static Dictionary<int, Dictionary<Guid, JObject>> ContentBodyList = null;  
      
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
            using (StreamReader streamReader = new StreamReader(dirInfo.FullName + "\\default-template.ect"))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    ContentHeadList = new Dictionary<int, Dictionary<string, JObject>>();
                    var temp = new Dictionary<string, JObject>();
                    while (csv.Read())
                    {
                        temp[csv.GetField("ViewName")] = JObject.FromObject(csv.GetRecord<object>());
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
                    ContentBodyList = new Dictionary<int, Dictionary<Guid, JObject>>();
                    var temp = new Dictionary<Guid, JObject>();
                    while (csv.Read())
                    {
                        temp[Guid.Parse(csv.GetField("UrlId"))] = JObject.FromObject(csv.GetRecord<object>());
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
            JObject jsonBody = ContentBodyList[url_.SiteId][url_.Id];
            item.Body = jsonBody;
            item.Head = GetHeadContentByViewName(url_);
            
            string temp2 = null;
            foreach (JToken token in jsonBody.Children())
            {
                if (token is JProperty)
                {
                    temp2 = (token as JProperty).Value.ToString();
                    if (temp2.Contains("@"))
                    {
                        var task = Task.Factory.StartNew(() => CreateTemplateAndSetInCache(temp2));
                        tasks.Add(task);
                        DependencyManager.CachingService.Set<List<Task>>("Task." + url_.Id, tasks);
                    }
                }
            }
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
            JObject jsonBody = ContentBodyList[url_.SiteId][url_.Id];
            JObject jsonHead = ContentHeadList[url_.SiteId][url_.View.Trim(new char[] { '/' })];
            jsonHead.MergeInto(jsonBody);
            ContentItemHead itemhead = new ContentItemHead();
            itemhead.LoadFromJObject(jsonHead);            
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
