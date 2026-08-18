using Microsoft.Playwright;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Options;
using Yapoml.Playwright.Services.Factory;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Base class for all page components providing fluent interactions and condition-based expectations.
/// </summary>
/// <typeparam name="TComponent">The concrete component type for fluent chaining.</typeparam>
/// <typeparam name="TConditions">The chainable conditions type used for multi-condition expectations.</typeparam>
/// <typeparam name="TCondition">The one-time conditions type used for single expectations.</typeparam>
public abstract partial class BaseComponent<TComponent, TConditions, TCondition> : BaseComponent
    where TComponent : BaseComponent<TComponent, TConditions, TCondition>
    where TConditions : BaseComponentConditions<TConditions>
    where TCondition : BaseComponentConditions<TComponent>
{
    /// <summary>The concrete component instance for fluent chaining.</summary>
    protected TComponent component;

    /// <summary>The chainable conditions instance.</summary>
    protected TConditions conditions;

    /// <summary>The one-time conditions instance.</summary>
    protected TCondition oneTimeConditions;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseComponent{TComponent, TConditions, TCondition}"/> class.
    /// </summary>
    protected BaseComponent(BasePage page, BaseComponent parentComponent, IPage driver, IElementHandler elementHandler, ComponentMetadata metadata, ISpaceOptions spaceOptions)
        : base(page, parentComponent, driver, elementHandler, metadata, spaceOptions)
    {

    }

    /// <summary>
    /// Various awaitable conditions on the component.
    /// </summary>
    public virtual TCondition Expect()
    {
        return Share(oneTimeConditions);
    }

    /// <summary>
    /// Various awaitable and chainable conditions on the component.
    /// </summary>
    public virtual TComponent Expect(Action<TConditions> it)
    {
        it(Share(conditions));

        return component;
    }

    /// <summary>
    /// Various awaitable and chainable conditions on the component with async lambda support.
    /// </summary>
    public virtual TComponent Expect(Func<TConditions, Task> it)
    {
        Enqueue(async () => await it(Share(conditions)));

        return component;
    }

    /// <summary>Executes the pending steps and returns the component back.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public TaskAwaiter<TComponent> GetAwaiter()
    {
        return AwaitChainAsync().GetAwaiter();
    }

    private async Task<TComponent> AwaitChainAsync()
    {
        await Chain.RunAsync().ConfigureAwait(false);

        return component;
    }
}

/// <summary>
/// Non-generic base class for all page components, providing core properties and element interaction primitives.
/// </summary>
public abstract class BaseComponent
{
    /// <summary>The parent component in the hierarchy, or <c>null</c> for top-level components.</summary>
    protected BaseComponent parentComponent;
    /// <summary>Gets the parent page that owns this component.</summary>
    protected BasePage Page { get; }
    /// <summary>Gets the Playwright page instance.</summary>
    protected IPage Driver { get; private set; }

    /// <summary>The element handler responsible for locating this component.</summary>
    protected IElementHandler _elementHandler;
    private readonly Lazy<AttributesCollection> _attributes;
    private readonly Lazy<StylesCollection> _styles;
    /// <summary>The logger for tracing component operations.</summary>
    protected ILogger _logger;

    /// <summary>The maximum duration to wait when locating this component.</summary>
    protected TimeSpan _locateTimeout;
    /// <summary>The polling interval used when locating this component.</summary>
    protected TimeSpan _locatePollingInterval;

    /// <summary>Gets the underlying Playwright locator for this component.</summary>
    protected virtual ILocator WrappedElement => _elementHandler.Locate(_locateTimeout, _locatePollingInterval);

    /// <summary>Gets the metadata describing this component.</summary>
    protected ComponentMetadata Metadata { get; }

    /// <summary>Gets the chain of pending asynchronous steps shared with the page and parent component.</summary>
    internal Chain Chain { get; }

    /// <summary>
    /// Enqueues a step to be executed when the component is awaited.
    /// </summary>
    protected void Enqueue(Func<Task> step)
    {
        Chain.Add(step);
    }

    /// <summary>Makes the child conditions build into the same chain as this component.</summary>
    private protected T Share<T>(T child) where T : BaseConditions
    {
        child.Chain = Chain;

        return child;
    }

    /// <summary>Gets the space options configuration.</summary>
    protected ISpaceOptions SpaceOptions { get; private set; }

    /// <summary>Gets the event source for raising lifecycle events.</summary>
    protected IEventSource EventSource { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseComponent"/> class.
    /// </summary>
    public BaseComponent(BasePage page, BaseComponent parentComponent, IPage driver, IElementHandler elementHandler, ComponentMetadata metadata, ISpaceOptions spaceOptions)
    {
        Page = page;
        this.parentComponent = parentComponent;
        Driver = driver;
        _elementHandler = elementHandler;
        Metadata = metadata;
        SpaceOptions = spaceOptions;

        Chain = Chain.Resolve(spaceOptions);

        EventSource = spaceOptions.Services.Get<IEventSource>();
        _logger = spaceOptions.Services.Get<ILogger>();
        _locateTimeout = spaceOptions.Services.Get<TimeoutOptions>().Timeout;
        _locatePollingInterval = spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;

        _attributes = new Lazy<AttributesCollection>(() => new AttributesCollection(elementHandler, spaceOptions));
        _styles = new Lazy<StylesCollection>(() => new StylesCollection(elementHandler, spaceOptions));
    }

    /// <summary>
    /// Gets the value of an attribute of a component as a string. It can also retrieve component properties, such as an anchor tag’s href attribute.
    /// <para>
    /// For example, you can use it to check if an input element has a value attribute that matches the expected input,
    /// or if an image element has an alt attribute that describes the image.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Well-known attributes are accessible shortly.
    /// <code>
    /// var value = driver.Ya().HomePage.SearchInput.Attributes.Value;
    /// // or
    /// var href = driver.Ya().HomePage.Logo.Attributes.Href;
    /// </code>
    /// </remarks>
    public virtual AttributesCollection Attributes => _attributes.Value;

    /// <summary>
    /// Gets the value of a CSS property of a component as a string. It can be used to retrieve the computed style of a component, 
    /// such as its <c>color</c>, <c>font-size</c>, or <c>display</c>.
    /// <para>
    /// For example, you can use it to check if an element has a certain background color, or if an element is visible or hidden by its display property.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Well-known styles are accessible shortly.
    /// <code>
    /// var color = driver.Ya().HomePage.SearchButton.Styles.Color;
    /// // or
    /// var opacity = driver.Ya().HomePage.SearchButton.Styles.Opacity;
    /// </code>
    /// </remarks>
    public virtual StylesCollection Styles => _styles.Value;

    /// <summary>
    /// Gets the visible text of a component.
    /// <para>
    /// It returns a string value that represents the inner text of the element. For example, you can use it to check if
    /// a label displays the correct message, or if a paragraph contains the expected text.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This property may not work as expected for some components that do not have any visible text content. For example,
    /// input elements (<c>&lt;input&gt;</c>) do not have any inner text, so they will return an empty string for this property.
    /// To get the value of an input element, you may need to use the <see cref="AttributesCollection.Value"/> property.
    /// </remarks>
    public virtual Task<string> Text => ReadAsync(async () => (await RelocateOnStaleReferenceAsync(() => WrappedElement.TextContentAsync()).ConfigureAwait(false)).Trim());

    /// <summary>
    /// Used to indicate whether a component can respond to user interactions or not.
    /// <para>
    /// It returns a boolean value: <c>true</c> if the element is enabled, and <c>false</c> if the element is disabled.
    /// </para>
    /// <para>
    /// For example, you can use it to check if a checkbox is checked or unchecked, or if a text field is editable or read-only.
    /// </para>
    /// </summary>
    public virtual Task<bool> IsEnabled => ReadAsync(() => RelocateOnStaleReferenceAsync(() => WrappedElement.IsEnabledAsync()));

    /// <summary>
    /// Indicates whether a component currently is checked or not.
    /// <para>
    /// It returns a boolean value: <c>true</c> if the component is checked, and <c>false</c> if the component is unchecked.
    /// </para>
    /// </summary>
    public virtual Task<bool> IsChecked => ReadAsync(() => RelocateOnStaleReferenceAsync(() => WrappedElement.IsCheckedAsync()));

    /// <summary>
    /// Indicates whether a component currently is partially visible within viewport or not.
    /// <para>
    /// It returns a boolean value: <c>true</c> if the component is in viewport, and <c>false</c> if the component is not.
    /// </para>
    /// </summary>
    public virtual Task<bool> IsInView => throw new NotImplementedException();

    /// <summary>
    /// Indicates whether a component is visible on the page or not.
    /// <para>
    /// Returns <c>true</c> if the element is displayed, and <c>false</c> if the element is hidden or not present.
    /// </para>
    /// <para>
    /// Useful for verifying the visibility of components that may change dynamically based on user actions or page conditions.
    /// For example, you can use it to check if a dropdown menu is expanded or collapsed, or if a modal dialog is open or closed.
    /// It does not check if the component is within the viewport or not. It only checks if the element is rendered on the page,
    /// regardless of its position or size. Look at <see cref="IsInView"/> property if you need to check whether the component is within the viewport.
    /// </para>
    /// </summary>
    /// <remarks>
    /// It doesn't expect a component exists in DOM. It only returns <c>true</c> if a component is found in DOM and visible. Otherwise, it returns <c>false</c>.
    /// </remarks>
    public virtual Task<bool> IsDisplayed => ReadAsync(() => WrappedElement.IsVisibleAsync());

    /// <summary>
    /// Indicates whether a component currently has logical focus or not.
    /// <para>
    /// It returns a boolean value: <c>true</c> if the component has focus, and <c>false</c> if the component does not have focus.
    /// </para>
    /// </summary>
    public virtual Task<bool> IsFocused => ReadAsync(async () =>
    {
        var isFocusedRes = await WrappedElement.EvaluateAsync("node => document.activeElement === node").ConfigureAwait(false);

        return bool.Parse(isFocusedRes.ToString());
    });

    /// <summary>
    /// Returns a value of the component.
    /// </summary>
    public virtual Task<string> Value => ReadAsync(() => WrappedElement.InputValueAsync());

    /// <summary>
    /// Retries an async function upon stale element references.
    /// </summary>
    protected async Task<T> RelocateOnStaleReferenceAsync<T>(Func<Task<T>> func)
    {
        return await func().ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the pending steps and reads a value once the chain is drained.
    /// </summary>
    protected async Task<T> ReadAsync<T>(Func<Task<T>> read)
    {
        await Chain.RunAsync().ConfigureAwait(false);

        return await read().ConfigureAwait(false);
    }

    /// <summary>
    /// Resolves an element handler from the component's repository, creating and caching it if not found.
    /// </summary>
    protected IElementHandler ResolveElementHandler(string key, string by, ElementLocatorContext byFrom, string metadataName)
    {
        if (_elementHandler.ElementHandlerRepository.TryGet(key, out var cachedElementHandler))
            return cachedElementHandler;

        var metadata = new ComponentMetadata { Name = metadataName };
        var elementLocator = SpaceOptions.Services.Get<IElementLocator>();
        var elementHandler = new ElementHandler(Driver, _elementHandler, elementLocator, by, byFrom, metadata, _elementHandler.ElementHandlerRepository.CreateNestedRepository(), EventSource);
        _elementHandler.ElementHandlerRepository.Set(key, elementHandler);
        return elementHandler;
    }

    /// <summary>
    /// Creates an elements list handler for plural components on a component.
    /// </summary>
    protected IElementsListHandler CreateElementsListHandler(string by, ElementLocatorContext byFrom, string singularName, string pluralName)
    {
        var metadata = new ComponentMetadata { Name = singularName };
        var listMetadata = new ComponentsListMetadata { Name = pluralName, ComponentMetadata = metadata };
        var elementLocator = SpaceOptions.Services.Get<IElementLocator>();
        var factory = SpaceOptions.Services.Get<IElementsListHandlerFactory>();
        return factory.Create(Driver, _elementHandler, elementLocator, by, byFrom, listMetadata, _elementHandler.ElementHandlerRepository.CreateNestedRepository(), EventSource);
    }
}
