namespace Yapoml.Playwright.Options
{
    /// <summary>
    /// Options for controlling the scroll-into-view behavior when scrolling a component into the viewport.
    /// </summary>
    public class ScrollIntoViewOptions
    {
        /// <summary>
        /// Defines the transition animation.
        /// Default <see cref="ScrollIntoViewBehavior.Auto" />
        /// </summary>
        public ScrollIntoViewBehavior Behavior { get; set; } = ScrollIntoViewBehavior.Auto;

        /// <summary>
        /// Defines vertical alignment.
        /// Default <see cref="ScrollIntoViewBlock.Start" />
        /// </summary>
        public ScrollIntoViewBlock Block { get; set; } = ScrollIntoViewBlock.Start;

        /// <summary>
        /// Defines horizontal alignment.
        /// Default <see cref="ScrollIntoViewInline.Nearest" />
        /// </summary>
        public ScrollIntoViewInline Inline { get; set; } = ScrollIntoViewInline.Nearest;

        /// <summary>Serializes the scroll-into-view options to a JSON string.</summary>
        public string ToJson()
        {
            return $"{{behavior: \"{Behavior.ToString().ToLowerInvariant()}\", block: \"{Block.ToString().ToLowerInvariant()}\", inline: \"{Inline.ToString().ToLowerInvariant()}\"}}";
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Behavior: {Behavior}, Vertical aligment: {Block}, Horizontal aligment: {Inline}";
        }
    }

    /// <summary>
    /// Defines the scroll transition animation behavior.
    /// </summary>
    public enum ScrollIntoViewBehavior
    {
        /// <summary>
        /// The browser determines the scroll behavior.
        /// </summary>
        Auto,

        /// <summary>
        /// Scroll with a smooth animation.
        /// </summary>
        Smooth
    }

    /// <summary>
    /// Defines the vertical alignment when scrolling an element into view.
    /// </summary>
    public enum ScrollIntoViewBlock
    {
        /// <summary>
        /// Aligns the element to the top of the scrolling area.
        /// </summary>
        Start,

        /// <summary>
        /// Aligns the element to the center of the scrolling area.
        /// </summary>
        Center,

        /// <summary>
        /// Aligns the element to the bottom of the scrolling area.
        /// </summary>
        End,

        /// <summary>
        /// Scrolls the minimum amount to make the element visible.
        /// </summary>
        Nearest
    }

    /// <summary>
    /// Defines the horizontal alignment when scrolling an element into view.
    /// </summary>
    public enum ScrollIntoViewInline
    {
        /// <summary>
        /// Aligns the element to the left of the scrolling area.
        /// </summary>
        Start,

        /// <summary>
        /// Aligns the element to the horizontal center of the scrolling area.
        /// </summary>
        Center,

        /// <summary>
        /// Aligns the element to the right of the scrolling area.
        /// </summary>
        End,

        /// <summary>
        /// Scrolls the minimum amount horizontally to make the element visible.
        /// </summary>
        Nearest
    }
}
