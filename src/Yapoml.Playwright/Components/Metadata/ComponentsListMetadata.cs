namespace Yapoml.Playwright.Components.Metadata
{
    /// <summary>
    /// Holds metadata describing a list of components, including the list's display name and the metadata for individual components.
    /// </summary>
    public class ComponentsListMetadata
    {
        /// <summary>
        /// Gets or sets the display name of the components list (plural form).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the metadata for individual components in the list.
        /// </summary>
        public ComponentMetadata ComponentMetadata { get; set; }
    }
}
