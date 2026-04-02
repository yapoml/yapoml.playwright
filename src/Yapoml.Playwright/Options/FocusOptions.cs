namespace Yapoml.Playwright.Options
{
    /// <summary>
    /// An optional object for controlling aspects of the focusing process.
    /// </summary>
    public class FocusOptions
    {
        /// <summary>
        /// A boolean value indicating whether or not the browser should scroll the document to bring the newly-focused element into view.
        /// A value of <c>false</c> means that the browser will scroll the element into view after focusing it.
        /// Default <c>false</c> />
        /// </summary>
        public bool PreventScroll { get; set; } = false;

        /// <summary>
        /// Whether it should force visible indication that the element is focused. This would improve accessibility for users.
        /// Default <c>true</c> />
        /// </summary>
        public bool FocusVisible { get; set; } = true;

        /// <summary>Serializes the focus options to a JSON string.</summary>
        public string ToJson()
        {
            return $"{{preventScroll: \"{PreventScroll.ToString().ToLowerInvariant()}\", focusVisible: \"{FocusVisible.ToString().ToLowerInvariant()}\"}}";
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Prevent scroll: {PreventScroll}, Focus visible: {FocusVisible}";
        }
    }
}
