
namespace ECMS.WebV2.AppCode
{
    
    public class FakePageOutputCache : System.Web.Caching.OutputCacheProvider
    {

        public override object Add(string key, object entry, System.DateTime utcExpiry)
        {
            return entry;
        }

        public override object Get(string key)
        {
            return null;
        }


        public override void Remove(string key)
        {
        }


        public override void Set(string key, object entry, System.DateTime utcExpiry)
        {
        }

    }
}