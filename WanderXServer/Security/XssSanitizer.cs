using System.Net;
using System.Text.RegularExpressions;

namespace WanderXServer.Security;

public static class XssSanitizer
{
    // Regex pattern to identify and strip script tags and their content
    private static readonly Regex ScriptTagRegex = new Regex(
        @"<script[^>]*>[\s\S]*?</script>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Regex pattern to identify javascript: URLs
    private static readonly Regex JavascriptUrlRegex = new Regex(
        @"javascript\s*:\s*\S*",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Regex pattern to identify common HTML event handlers (e.g. onload, onerror, onclick)
    private static readonly Regex HtmlEventRegex = new Regex(
        @"\bon\w+\s*=\s*""[^""]*""|\bon\w+\s*=\s*'[^']*'|\bon\w+\s*=\s*\S*",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Sanitizes an input string by HTML encoding it and stripping out script tags/events.
    /// </summary>
    public static string Sanitize(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // 1. Basic HTML encode to turn characters like '<', '>', '&', '"' into safe HTML entities
        var encoded = WebUtility.HtmlEncode(input);

        // 2. Proactively strip script blocks and inline javascript event handlers
        encoded = ScriptTagRegex.Replace(encoded, string.Empty);
        encoded = JavascriptUrlRegex.Replace(encoded, string.Empty);
        encoded = HtmlEventRegex.Replace(encoded, string.Empty);

        return encoded;
    }
}
