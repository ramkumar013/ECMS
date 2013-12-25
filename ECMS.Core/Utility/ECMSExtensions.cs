using ECMS.Core.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Extensions
{
    public static class ECMSExtensions
    {
        public static void LoadFromJObject(this ContentItemHead contentItemHead_, JObject object_)
        {
            foreach (JToken token in object_.Children())
            {
                if (token is JProperty)
                {
                    switch ((token as JProperty).Name.ToString())
                    {
                        case "Title":
                            contentItemHead_.Title = (token as JProperty).Value.ToString();
                            break;
                        case "KeyWords":
                            contentItemHead_.KeyWords = (token as JProperty).Value.ToString();
                            break;
                        case "Description":
                            contentItemHead_.Description = (token as JProperty).Value.ToString();
                            break;
                        case "PageMetaTags":
                            contentItemHead_.PageMetaTags = (token as JProperty).Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}
