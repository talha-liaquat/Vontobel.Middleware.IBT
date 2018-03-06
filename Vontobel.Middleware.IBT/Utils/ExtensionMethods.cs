using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vontobel.Middleware.IBT.Contracts.Attributes;
using Vontobel.Middleware.IBT.Contracts.Model;

static class ExtensionMethods
{
    public static Dictionary<string, string> GeTranformableAttributes(this IVontobelEntity entity)
    {
        Dictionary<string, string> attributes = new Dictionary<string, string>();

        Type myType = entity.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        props.ToList()
            .Where(
                p => Attribute.IsDefined(p, typeof(TransformableAttribute)))
            .ToList()
            .ForEach(
                x => attributes.Add(x.GetCustomAttribute< TransformableAttribute>().Name, x.GetValue(entity).ToString()));

        return attributes;
    }
}
