using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using System.Web;
using System.Reflection;
using System.Web.Configuration;
using System.IO;
namespace ECMS.Core.Utility
{
    public class ECMSUtility
    {
        private static List<IExpressionParser> ExpressionParsers = null;
        static ECMSUtility()
        {
            LoadExpressionParser();
        }

        static void LoadExpressionParser()
        {
            ExpressionParsers = new List<IExpressionParser>();
            var type = typeof(IExpressionParser);
            // check for types in all loaded and referenced assemblies.
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GlobalAssemblyCache == false).ToArray();
            foreach (Assembly assembly in assemblies)
            {
                var expressions = (from t in assembly.GetTypes()
                                   where type.IsAssignableFrom(t) && type.Name != t.Name
                                   select type.Assembly.CreateInstance(t.Name) as IExpressionParser).ToList();

                foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
                {
                    expressions.AddRange(from t in Assembly.Load(referencedAssembly).GetTypes()
                                         where type.IsAssignableFrom(t) && type.Name != t.Name
                                         select Assembly.Load(referencedAssembly).CreateInstance(t.FullName) as IExpressionParser);
                }

                if (expressions.Count() > 0)
                {
                    ExpressionParsers.AddRange(expressions.ToList<IExpressionParser>());
                }
            }
        }

        public static string ParseExpression(string content_,ValidUrl validUrl_, HttpContextBase context_)
        {
            StringBuilder builder = new StringBuilder(content_);
            foreach (IExpressionParser expressionParser in ExpressionParsers)
            {
                expressionParser.CurrentUrl = validUrl_;
                expressionParser.CurrentContext = context_;
                expressionParser.ParseExpression(builder);
            }
            return builder.ToString();
        }

        public static string ParseExpression(dynamic content_, ValidUrl validUrl_, HttpContextBase context_)
        {
            throw new NotImplementedException();
        }
    }
}
