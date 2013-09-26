using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.HttpModules
{
    public class UrlValidationConfig : ConfigurationSection
    {
        /// <summary>
        /// The value of the property here "Folders" needs to match that of the config file section
        /// </summary>
        [ConfigurationProperty("InvalidUrlPart")]
        public UrlValidationConfigCollection ConfigCollection
        {
            get { return ((UrlValidationConfigCollection)(base["InvalidUrlPart"])); }
        }


    }

    /// <summary>
    /// The collection class that will store the list of each element/item that
    /// is returned back from the configuration manager.
    /// </summary>
    [ConfigurationCollection(typeof(UrlValidationElement))]
    public class UrlValidationConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlValidationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlValidationElement)(element)).Action;
        }

        public UrlValidationElement this[int idx]
        {
            get
            {
                return (UrlValidationElement)BaseGet(idx);
            }
        }
    }

    /// <summary>
    /// The class that holds onto each element returned by the configuration manager.
    /// </summary>
    public class UrlValidationElement : ConfigurationElement
    {

        [ConfigurationProperty("Action", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Action
        {
            get
            {
                return ((string)(base["Action"]));
            }
            set
            {
                base["Action"] = value;
            }
        }

        [ConfigurationProperty("InvalidValue", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string InvalidValue
        {
            get
            {
                return ((string)(base["InvalidValue"]));
            }
            set
            {
                base["InvalidValue"] = value;
            }
        }
    }
}
