using ECMS.Core.Entities;
using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECMS.Services.ExpressionParser
{
    public class HttpRequestExpressionParser : IExpressionParser
    {
        public string ParseExpression(StringBuilder contentToParse_)
        {
            throw new NotImplementedException();
        }

        public HttpContextBase CurrentContext
        {
            get;
            set;
        }

        public ValidUrl CurrentUrl
        {
            get;
            set;
        }

        public bool ParserResult
        {
            get;
            set;
        }
    }
}
