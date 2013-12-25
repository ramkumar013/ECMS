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
        private static Dictionary<string, JObject> ContentHeadList = new Dictionary<string, JObject>();
        private static Dictionary<int, Dictionary<Guid, JObject>> ContentBodyList = null;
        private const string ECMS_FILE_EXTENSION = ".etxt";
        static FileSystemRepository()
        {
            //TODO:headcontent and body content directories should be created at app init.
            string[] dirinfo = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\app_data");
            foreach (string dir in dirinfo)
            {
                string[] contentTypes = new string[] { "10", "20" }; // TODO:Remove HardCoding
                foreach (var type in contentTypes)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir + "\\" + type + "\\headcontent");
                    LoadMetaTags(dirInfo, type);
                }
                //LoadPageContents(dirInfo);
            }
        }

        private static void LoadMetaTags(DirectoryInfo dirInfo, string contentTypeDir_)
        {
            foreach (var file in dirInfo.GetFiles("*.etxt"))
            {
                using (StreamReader streamReader = new StreamReader(file.FullName))
                {
                    using (var csv = new CsvReader(streamReader))
                    {
                        var temp = new Dictionary<string, JObject>();
                        while (csv.Read())
                        {
                            //temp[csv.GetField("ViewName")] = JObject.FromObject(csv.GetRecord<object>());
                            ContentHeadList.Add(dirInfo.Parent.Parent.Name + "-" + contentTypeDir_ + "-" + file.Name.Replace("-default-content.etxt", string.Empty), JObject.FromObject(csv.GetRecord<object>()));
                        }
                    }
                }
            }
        }

        //private static void LoadPageContents(DirectoryInfo dirInfo)
        //{
        //    using (StreamReader streamReader = new StreamReader(dirInfo.FullName + "\\content.etxt"))
        //    {
        //        using (var csv = new CsvReader(streamReader))
        //        {
        //            ContentBodyList = new Dictionary<int, Dictionary<Guid, JObject>>();
        //            var temp = new Dictionary<Guid, JObject>();
        //            while (csv.Read())
        //            {
        //                temp[Guid.Parse(csv.GetField("UrlId"))] = JObject.FromObject(csv.GetRecord<object>());
        //            }
        //            ContentBodyList[Convert.ToInt32(dirInfo.Name)] = temp;
        //        }
        //    }
        //}

        private JObject LoadPageContents(ValidUrl url_, ContentViewType viewType_, bool forBodyContent_)
        {
            string filePath = ConstructPath(url_, viewType_, forBodyContent_);
            return ReadPageContentFromDisk(filePath);
        }

        private string ConstructPath(ValidUrl url_, ContentViewType viewType_, bool forBodyContent_)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\" + url_.SiteId + "\\" + Convert.ToInt32(viewType_).ToString() + (forBodyContent_ ? "\\bodycontent\\" : "\\headcontent\\") +url_.Id + ECMS_FILE_EXTENSION;
            //if (!File.Exists(filePath))
            //{
            //    filePath = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\" + url_.SiteId + "\\" + Convert.ToInt32(viewType_).ToString() + (forBodyContent_ ? "\\bodycontent\\" : "\\headcontent\\") + url_.View + "-default-content" + ECMS_FILE_EXTENSION;
            //}
            return filePath;
        }

        private string ConstructPath(ECMSView view_, bool forBodyContent_)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\" + view_.SiteId + "\\" + Convert.ToInt32(view_.ViewType).ToString() + (forBodyContent_ ? "\\bodycontent\\" : "\\headcontent\\") + view_.ViewName + "-default-content" + ECMS_FILE_EXTENSION;
            //if (!File.Exists(filePath))
            //{
            //    filePath = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\" + view_.SiteId + "\\" + Convert.ToInt32(view_.ViewType).ToString() + (forBodyContent_ ? "\\bodycontent\\" : "\\headcontent\\") + view_.ViewName + "-default-content" + ECMS_FILE_EXTENSION;
            //}
            return filePath;
        }

        private JObject ReadPageContentFromDisk(string filePath)
        {
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
            JObject jsonBody = LoadPageContents(url_, viewType_,true);
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
            string bodyContentFilePath = ConstructPath(content_.Url, viewType_, true);
            string headContentFilePath = ConstructPath(content_.Url, viewType_, false);

            File.WriteAllText(bodyContentFilePath, Convert.ToString(content_.Body[0]));

            using (StringWriter stringWriter = new StringWriter())
            {
                using (CsvWriter csvWriter = new CsvWriter(stringWriter))
                {
                    csvWriter.WriteHeader<ContentItemHead>();
                    csvWriter.WriteRecord<ContentItemHead>(content_.Head);
                    File.WriteAllText(headContentFilePath, stringWriter.ToString());
                }
            }
        }

        public override void Delete(ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override ContentItemHead GetHeadContentByViewName(ValidUrl url_, ContentViewType viewType_)
        {
            JObject jsonBody = ContentBodyList[url_.SiteId][url_.Id];
            JObject jsonHead = ContentHeadList[url_.SiteId.ToString() + "-" + Convert.ToInt32(viewType_).ToString() + "-" + url_.View.Trim(new char[] { '/' })];
            jsonHead.MergeInto(jsonBody);
            ContentItemHead itemhead = new ContentItemHead();
            itemhead.LoadFromJObject(jsonHead);
            return itemhead;
        }

        public ContentItemHead GetHeadContentByViewName(ValidUrl url_, JObject jsonBody, ContentViewType viewType_)
        {
            JObject jsonHead = ContentHeadList[url_.SiteId.ToString() + "-" + Convert.ToInt32(viewType_).ToString() + "-" + url_.View.Trim(new char[] { '/' })];
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

        public override void Save(ContentItem content_, ECMSView view_)
        {
            string bodyContentFilePath = ConstructPath(view_, true);
            string headContentFilePath = ConstructPath(view_, false);

            File.WriteAllText(bodyContentFilePath, Convert.ToString(content_.Body[0]));

            using (StringWriter stringWriter = new StringWriter())
            {
                using (CsvWriter csvWriter = new CsvWriter(stringWriter))
                {
                    csvWriter.WriteHeader<ContentItemHead>();
                    csvWriter.WriteRecord<ContentItemHead>(content_.Head);
                    File.WriteAllText(headContentFilePath, stringWriter.ToString());
                }
            }
        }
        public override ContentItem GetContentForEditing(ECMSView view_)
        {
            string bodyContentFilePath = ConstructPath(view_, true);
            string headContentFilePath = ConstructPath(view_, false);
            ContentItem contentItem = new ContentItem();
            using (StreamReader streamReader = new StreamReader(headContentFilePath))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    while (csv.Read())
                    {
                        contentItem.Head = csv.GetRecord<ContentItemHead>();
                    }
                }
            }
            contentItem.Body = (dynamic)File.ReadAllText(bodyContentFilePath);
            return contentItem;
        }

        public override ContentItem GetContentForEditing(ValidUrl url_,ContentViewType viewType_)
        {
            string bodyContentFilePath = ConstructPath(url_, viewType_, true);
            string headContentFilePath = ConstructPath(url_, viewType_, false);
            ContentItem contentItem = new ContentItem();
            using (StreamReader streamReader = new StreamReader(headContentFilePath))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    while (csv.Read())
                    {
                        contentItem.Head = csv.GetRecord<ContentItemHead>();
                    }
                }
            }
            contentItem.Body = (dynamic)File.ReadAllText(bodyContentFilePath);
            return contentItem;
        }
    }
}
