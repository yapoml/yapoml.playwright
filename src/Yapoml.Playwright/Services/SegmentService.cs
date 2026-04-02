using System.Collections.Generic;

namespace Yapoml.Playwright.Services;

/// <summary>
/// Replaces placeholder segments in URL templates with actual values.
/// </summary>
public class SegmentService
{
    /// <summary>
    /// Replaces <c>{key}</c> placeholders in the value string with corresponding segment values.
    /// </summary>
    /// <param name="value">The template string containing placeholders.</param>
    /// <param name="segments">The key-value pairs to substitute into the template.</param>
    /// <returns>The string with all placeholders replaced.</returns>
    public string Replace(string value, IList<KeyValuePair<string, string>> segments)
    {
        if (segments != null)
        {
            foreach (var segment in segments)
            {
                value = value.Replace($"{{{segment.Key}}}", segment.Value);
            }
        }

        return value;
    }
}
