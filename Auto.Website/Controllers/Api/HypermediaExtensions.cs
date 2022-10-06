using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace Auto.Website.Controllers.Api;

public static class HypermediaExtensions {
    public static dynamic ToDynamic(this object value) {
        IDictionary<string, object> expando = new ExpandoObject();
        var properties = TypeDescriptor.GetProperties(value.GetType());
        foreach (PropertyDescriptor property in properties) {
            if (Ignore(property)) continue;
            expando.Add(property.Name, property.GetValue(value));
        }
        return (ExpandoObject)expando;
    }

    private static bool Ignore(PropertyDescriptor property) {
        if (property.Name == "LazyLoader") return (true);
        return property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();
    }

    public static dynamic Paginate(string url, int index, int count, int total)
    {
        dynamic links = new ExpandoObject();
        links.self = new { href = url };
        links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
        links.first = new { href = $"{url}?index=0&count={count}" };
        if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
        if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
        return links;
    }
}