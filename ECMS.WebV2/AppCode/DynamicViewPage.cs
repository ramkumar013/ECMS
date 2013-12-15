using System;
using System.Dynamic;
using System.Reflection;
using System.Web.Mvc;

namespace MvcHelpers
{
    public class ReflectionDynamicObject : DynamicObject
    {
        internal object RealObject { get; set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Get the property value
            result = RealObject.GetType().InvokeMember(
                binder.Name,
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                RealObject,
                null);

            // Always return true, since InvokeMember would have thrown if something went wrong
            return true;
        }
    }
}
