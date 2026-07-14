using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace WanderXServer.Security;

public class XssSanitizationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value == null)
            {
                continue;
            }

            var type = argument.Value.GetType();

            if (type == typeof(string))
            {
                context.ActionArguments[argument.Key] = XssSanitizer.Sanitize((string)argument.Value);
            }
            else if (!type.IsPrimitive && type != typeof(decimal) && type != typeof(DateTime) && type != typeof(Guid))
            {
                SanitizeObjectProperties(argument.Value);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after execution
    }

    private void SanitizeObjectProperties(object obj)
    {
        if (obj == null)
        {
            return;
        }

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            if (prop.PropertyType == typeof(string) && prop.CanWrite && prop.CanRead)
            {
                var val = (string?)prop.GetValue(obj);
                if (val != null)
                {
                    prop.SetValue(obj, XssSanitizer.Sanitize(val));
                }
            }
        }
    }
}
