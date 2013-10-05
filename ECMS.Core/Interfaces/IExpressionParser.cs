using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECMS.Core.Interfaces
{
    public interface IExpressionParser
    {
        string ParseExpression(StringBuilder contentToParse_);
        HttpContextBase CurrentContext { get; set; }
        ValidUrl CurrentUrl { get; set; }
        bool ParserResult { get; set; }
    }
}
