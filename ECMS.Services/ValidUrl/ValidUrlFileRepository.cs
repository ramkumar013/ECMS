﻿using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Core.Entities;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using ECMS.Core;
using System.Configuration;
using NLog;

namespace ECMS.Services
{
    public class ValidUrlFileRepository : IValidURLRepository
    {
        private static object UrlLock = new object();
        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_)
        {
            return GetFromCache(siteId_, friendlyurl_);
        }

        public ValidUrl GetById(Guid urlId_)
        {
            throw new NotImplementedException();
        }

        private ValidUrl GetFromCache(int siteId_, string friendlyurl_)
        {
            Dictionary<string, ValidUrl> dict = DependencyManager.CachingService.Get<Dictionary<string, ValidUrl>>(siteId_.ToString());

            if (dict == null)
            {
                lock (UrlLock)
                {
                    if (dict == null)
                    {
                        foreach (var directory in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\" + siteId_ + "\\"))
                        {
                            foreach (var file in new DirectoryInfo(directory).GetFiles("active-urls.json"))
                            {
                                LoadFromDisk(dict, siteId_, true, file.FullName);
                            }
                        }
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\" + siteId_ + "\\urls" + ((loadActiveUrls_) ? "active.json" : "inactive.json");
                        if (dict.Keys.Count() > 0)
                        {
                            DependencyManager.CachingService.Set<Dictionary<string, ValidUrl>>(siteId_.ToString(), dict);
                        }
                    }
                }
            }
            return dict[friendlyurl_];
        }

        private Dictionary<string, ValidUrl> LoadFromDisk(Dictionary<string, ValidUrl> dict_, int siteId_,bool loadActiveUrls_,string filePath_)
        {
            string path = filePath_;
            ValidUrl temp = null;
            using (StreamReader sreader = new StreamReader(path))
            {
                using (JsonReader jreader = new JsonTextReader(sreader))
                {
                    while (jreader.Depth <= 0)
                    {
                        jreader.Read();
                    }
                    while (jreader.Depth >= 1)
                    {
                        try
                        {
                            jreader.Read();
                            if (jreader.TokenType == JsonToken.StartObject)
                            {
                                temp = new ValidUrl();
                            }
                            else if (jreader.TokenType == JsonToken.PropertyName)
                            {
                                switch (jreader.Value.ToString())
                                {
                                    case "FriendlyUrl":
                                        jreader.Read();
                                        temp.FriendlyUrl = jreader.Value.ToString();
                                        break;
                                    case "View":
                                        jreader.Read();
                                        temp.View = jreader.Value.ToString();
                                        break;
                                    case "Active":
                                        jreader.Read();
                                        temp.Active = Convert.ToBoolean(jreader.Value);
                                        break;
                                    case "Indexing":
                                        jreader.Read();
                                        temp.Index = Convert.ToBoolean(jreader.Value);
                                        break;
                                    case "StatusCode":
                                        jreader.Read();
                                        temp.StatusCode = Convert.ToInt16(jreader.Value);
                                        break;
                                    case "Id":
                                        jreader.Read();
                                        temp.Id = Guid.Parse(jreader.Value.ToString());
                                        break;
                                    case "Action":
                                        jreader.Read();
                                        temp.Action = jreader.Value.ToString();
                                        break;
                                    case "LastModified":
                                        jreader.Read();
                                        temp.LastModified = Convert.ToDateTime(jreader.Value.ToString());
                                        break;
                                    case "LastModifiedBy":
                                        jreader.Read();
                                        temp.LastModifiedBy = jreader.Value.ToString();
                                        break;
                                    case "ChangeFrequency":
                                        jreader.Read();
                                        temp.ChangeFrequency = jreader.Value.ToString();
                                        break;
                                    case "SitemapPriority":
                                        jreader.Read();
                                        temp.SitemapPriority =float.Parse(jreader.Value.ToString());
                                        break;
                                }
                            }
                            else if (jreader.TokenType == JsonToken.EndObject)
                            {
                                if (!dict_.ContainsKey(temp.FriendlyUrl.ToLower())) // TODO : To remove this if condition
                                {
                                    dict_.Add(temp.FriendlyUrl.ToLower(), temp);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, "Error while reading urls for siteid: " + siteId_ + "\r\n" + ex.ToString());
                            DependencyManager.Logger.Log(info);
                        }
                    }
                }
            }
            return dict_;
        }

        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool isPublish_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetById(int siteId_, Guid urlId_, bool isPublish_)
        {
            throw new NotImplementedException();
        }

        public void Save(ValidUrl url_)
        {
            
        }

        public void Update(ValidUrl url_)
        {
            throw new NotImplementedException();
        }

        public void Delete(ValidUrl url_)
        {
            throw new NotImplementedException();
        }


        public List<ValidUrl> GetAll(int siteId_, bool isPublish_)
        {
            Dictionary<string, ValidUrl> dict = DependencyManager.CachingService.Get<Dictionary<string, ValidUrl>>(siteId_.ToString());
            return dict.Values.ToList<ValidUrl>();
        }

        public Tuple<long, List<ValidUrl>> FindAndGetAll(int siteId_, string searchField, string searchString_, string searchOperator, string sortField, string sortDirection_, int pageNo_, int records_, bool isSearchRq_)
        {
            throw new NotImplementedException();
        }


        public long GetTotalUrlCount(int siteId_)
        {
            throw new NotImplementedException();
        }
    }


}
