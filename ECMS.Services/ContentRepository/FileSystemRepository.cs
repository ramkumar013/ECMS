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
        private static Dictionary<string, Dictionary<string, JObject>> ContentHeadList = new Dictionary<string, Dictionary<string, JObject>>();
        private static Dictionary<int, Dictionary<Guid, JObject>> ContentBodyList = null;
        private const string ECMS_FILE_EXTENSION = ".etxt";
        static FileSystemRepository()
        {
            string[] dirinfo = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\app_data");
            foreach (string dir in dirinfo)
            {
                string[] contentTypes = new string[] { "10", "20" };
                foreach (var type in contentTypes)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    LoadMetaTags(dirInfo, type);
                }
                //LoadPageContents(dirInfo);
            }
        }

        private static void LoadMetaTags(DirectoryInfo dirInfo, string contentTypeDir_)
        {
            using (StreamReader streamReader = new StreamReader(dirInfo.FullName + "\\" + contentTypeDir_ + "\\default-template.etxt"))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    var temp = new Dictionary<string, JObject>();
                    while (csv.Read())
                    {
                        temp[csv.GetField("ViewName")] = JObject.FromObject(csv.GetRecord<object>());
                    }
                    ContentHeadList[GetCacheKeyForContent(dirInfo.Name, contentTypeDir_)] = temp;
                }
            }
        }

        private static void LoadPageContents(DirectoryInfo dirInfo)
        {
            using (StreamReader streamReader = new StreamReader(dirInfo.FullName + "\\content.etxt" ))
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

        private JObject LoadPageContents(ValidUrl url_, ContentViewType viewType_)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\" + url_.SiteId + "\\" + Convert.ToInt32(viewType_).ToString() + "\\" + url_.Id + ECMS_FILE_EXTENSION;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    csv.Read();
                    return JObject.FromObject(csv.GetRecord<object>());
                }
            }
        }


        public override ContentItem GetById(ValidUrl url_, ContentViewType viewType_)
        {
            ContentItem item = new ContentItem();
            item.Url = url_;
            JObject jsonBody = LoadPageContents(url_, viewType_);
            item.Body = jsonBody;
            item.Head = GetHeadContentByViewName(url_, jsonBody, viewType_);

            string temp2 = null;
            foreach (JToken token in jsonBody.Children())
            {
                if (token is JProperty)
                {
                    temp2 = (token as JProperty).Value.ToString();
                    if (temp2.Contains("@"))
                    {
                        string hashCode = temp2.GetHashCode().ToString();
                        if (DependencyManager.CachingService.Get<ITemplate>(hashCode) == null)
                        {
                            var task = Task.Factory.StartNew(() => CreateTemplateAndSetInCache(hashCode, (token as JProperty).Value.ToString()));
                            DependencyManager.CachingService.Set<Task>("Task." + hashCode, task);
                        }
                    }
                }
            }
            return item;
        }

        public override ContentItem GetByUrl(ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Save(ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override ContentItemHead GetHeadContentByViewName(ValidUrl url_, ContentViewType viewType_)
        {
            JObject jsonBody = ContentBodyList[url_.SiteId][url_.Id];
            JObject jsonHead = ContentHeadList[GetCacheKeyForContent(url_.SiteId.ToString(), Convert.ToInt32(viewType_).ToString())][url_.View.Trim(new char[] { '/' })];
            jsonHead.MergeInto(jsonBody);
            ContentItemHead itemhead = new ContentItemHead();
            itemhead.LoadFromJObject(jsonHead);
            return itemhead;
        }

        public ContentItemHead GetHeadContentByViewName(ValidUrl url_, JObject jsonBody, ContentViewType viewType_)
        {
            JObject jsonHead = ContentHeadList[GetCacheKeyForContent(url_.SiteId.ToString(), Convert.ToInt32(viewType_).ToString())][url_.View.Trim(new char[] { '/' })];
            jsonHead.MergeInto(jsonBody);
            ContentItemHead itemhead = new ContentItemHead();
            itemhead.LoadFromJObject(jsonHead);
            return itemhead;
        }

        private void CreateTemplateAndSetInCache(string key_, string template_)
        {
            TemplateService service = new TemplateService();
            ITemplate template = service.CreateTemplate(template_, null, null);
            DependencyManager.CachingService.Set<ITemplate>(key_, template);
        }

        private static string GetCacheKeyForContent(string val1_, string val2_)
        {
            return string.Format("{0}-{1}", val1_, val2_);
        }
    }
}
