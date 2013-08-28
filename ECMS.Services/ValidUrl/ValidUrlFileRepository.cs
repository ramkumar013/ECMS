using ECMS.Core.Interfaces;
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

namespace ECMS.Services
{
    public class ValidUrlFileRepository : IValidURLRepository
    {
        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_)
        {
            return GetFromCache(siteId_, friendlyurl_);
        }

        public ValidUrl GetById(int urlId_)
        {
            throw new NotImplementedException();
        }

        private ValidUrl GetFromCache(int siteId_, string friendlyurl_)
        {
            // TODO : Lock no file before reading.
            Dictionary<string, ValidUrl> dict = DependencyManager.CachingService.Get<Dictionary<string, ValidUrl>>(siteId_.ToString());

            if (dict == null)
            {
                dict = LoadFromDisk(siteId_);
                DependencyManager.CachingService.Set<Dictionary<string, ValidUrl>>(siteId_.ToString(), dict);
            }
            return dict[friendlyurl_];
        }

        private Dictionary<string, ValidUrl> LoadFromDisk(int siteId_)
        {
            Dictionary<string, ValidUrl> dict = new Dictionary<string, ValidUrl>();
            string path = ConfigurationManager.AppSettings["UrlFilePath"] + siteId_ + "\\urls.json";
            ValidUrl temp = null;
            using (StreamReader sreader = new StreamReader(path))
            {
                using (Newtonsoft.Json.JsonReader jreader = new JsonTextReader(sreader))
                {
                    while (jreader.Depth <= 0)
                    {
                        jreader.Read();
                    }
                    while (jreader.Depth >= 1)
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
                                case "friendlyurl":
                                    jreader.Read();
                                    temp.FriendlyUrl = jreader.Value.ToString();
                                    break;
                                case "View":
                                    jreader.Read();
                                    temp.View = jreader.Value.ToString();
                                    break;
                                case "Active":
                                    jreader.Read();
                                    temp.Active = Convert.ToBoolean(jreader.Value.ToString());
                                    break;
                                case "Indexing":
                                    jreader.Read();
                                    temp.Index = Convert.ToBoolean(jreader.Value.ToString());
                                    break;
                                case "StatusCode":
                                    jreader.Read();
                                    temp.StatusCode = Convert.ToInt32(jreader.Value.ToString());
                                    break;
                            }
                        }
                        else if (jreader.TokenType == JsonToken.EndObject)
                        {
                            if (!dict.ContainsKey(temp.FriendlyUrl.ToLower())) // TODO : To remove this if condition
                            {
                                dict.Add(temp.FriendlyUrl.ToLower(), temp);
                            }
                        }
                    }
                }
            }
            return dict;
        }

        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool useCache_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetById(int siteId_, string friendlyurl_, bool useCache_)
        {
            throw new NotImplementedException();
        }

        public void Save(ValidUrl url_)
        {
            throw new NotImplementedException();
        }

        public void Update(ValidUrl url_)
        {
            throw new NotImplementedException();
        }

        public void Delete(ValidUrl url_)
        {
            throw new NotImplementedException();
        }    
    }
}
