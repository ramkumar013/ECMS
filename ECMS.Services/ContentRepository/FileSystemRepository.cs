using ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ECMS.Core.Framework;

namespace ECMS.Services.ContentRepository
{
    public class FileSystemRepository : ContentRepositoryBase
    {
        public override ContentBase GetById(Guid id_)
        {
            // TODO : parameter type.
            VerifyContentWithDefault(JsonConvert.DeserializeObject("{ \"FirstName\" : \"Vishal\", \"LastName\" : \"Sharma\", \"FlatNo\" : \"2104A\", \"Models\" : [{\"Make\":\"Maruti\",\"Model\":\"Alto\"},{\"Make\":\"Ranault\",\"Model\":\"Duster\"}]  }"));
            throw new NotImplementedException();
        }

        public override ContentBase GetByUrl(string incomingUrl_)
        {
            throw new NotImplementedException();
        }

        public override void Save(ContentBase content_)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ContentBase content_)
        {
            throw new NotImplementedException();
        }
    }
}
