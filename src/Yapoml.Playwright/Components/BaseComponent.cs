﻿using System;
using Yapoml.Framework.Options;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Options;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Components
{
    public abstract partial class BaseComponent<TComponent, TConditions, TCondition> : BaseComponent
        where TComponent : BaseComponent<TComponent, TConditions, TCondition>
        where TConditions : BaseComponentConditions<TConditions>
        where TCondition : BaseComponentConditions<TComponent>
    {
        protected TComponent component;

        protected TConditions conditions;
        protected TCondition oneTimeConditions;

        protected BaseComponent(BasePage page, BaseComponent parentComponent, IPage driver, IElementHandler elementHandler, ComponentMetadata metadata, ISpaceOptions spaceOptions)
            : base(page, parentComponent, driver, elementHandler, metadata, spaceOptions)
        {

        }

        /// <summary>
        /// Various awaitable conditions on the component.
        /// </summary>
        public virtual TCondition Expect()
        {
            return oneTimeConditions;
        }

        /// <summary>
        /// Various awaitable and chainable conditions on the component.
        /// </summary>
        public virtual TComponent Expect(Action<TConditions> it)
        {
            it(conditions);

            return component;
        }

        public static bool operator ==(BaseComponent<TComponent, TConditions, TCondition> component, string value)
        {
            if (component is null)
            {
                return value == null;
            }

            return component.Text == value;
        }

        public static bool operator !=(BaseComponent<TComponent, TConditions, TCondition> component, string value)
        {
            return !(component == value);
        }
    }

    public abstract class BaseComponent
    {
        protected BaseComponent parentComponent;
        protected BasePage Page { get; }
        protected IPage Driver { get; private set; }

        protected IElementHandler _elementHandler;
        private readonly Lazy<AttributesCollection> _attributes;
        private readonly Lazy<StylesCollection> _styles;
        protected ILogger _logger;

        protected TimeSpan _locateTimeout;
        protected TimeSpan _locatePollingInterval;

        protected virtual ILocator WrappedElement => _elementHandler.Locate(_locateTimeout, _locatePollingInterval);

        protected ComponentMetadata Metadata { get; }

        protected ISpaceOptions SpaceOptions { get; private set; }

        protected IEventSource EventSource { get; private set; }

        public BaseComponent(BasePage page, BaseComponent parentComponent, IPage driver, IElementHandler elementHandler, ComponentMetadata metadata, ISpaceOptions spaceOptions)
        {
            Page = page;
            this.parentComponent = parentComponent;
            Driver = driver;
            _elementHandler = elementHandler;
            Metadata = metadata;
            SpaceOptions = spaceOptions;

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
        public virtual string Text => RelocateOnStaleReference(() => Task.Run(() => WrappedElement.TextContentAsync()).GetAwaiter().GetResult().Trim());

        /// <summary>
        /// Used to indicate whether a component can respond to user interactions or not.
        /// <para>
        /// It returns a boolean value: <c>true</c> if the element is enabled, and <c>false</c> if the element is disabled.
        /// </para>
        /// <para>
        /// For example, you can use it to check if a checkbox is checked or unchecked, or if a text field is editable or read-only.
        /// </para>
        /// </summary>
        public virtual bool IsEnabled => RelocateOnStaleReference(() => Task.Run(() => WrappedElement.IsEnabledAsync()).GetAwaiter().GetResult());

        /// <summary>
        /// Indicates whether a component currently is checked or not.
        /// <para>
        /// It returns a boolean value: <c>true</c> if the component is checked, and <c>false</c> if the component is unchecked.
        /// </para>
        /// </summary>
        public virtual bool IsChecked => RelocateOnStaleReference(() => Task.Run(() => WrappedElement.IsCheckedAsync()).GetAwaiter().GetResult());

        /// <summary>
        /// Indicates whether a component currently is partially visible within viewport or not.
        /// <para>
        /// It returns a boolean value: <c>true</c> if the component is in viewport, and <c>false</c> if the component is not.
        /// </para>
        /// </summary>
        public virtual bool IsInView
        {
            get
            {
                throw new NotImplementedException();
            }
        }

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
        public virtual bool IsDisplayed
        {
            get
            {
                return Task.Run(() => WrappedElement.IsVisibleAsync()).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Indicates whether a component currently has logical focus or not.
        /// <para>
        /// It returns a boolean value: <c>true</c> if the component has focus, and <c>false</c> if the component does not have focus.
        /// </para>
        /// </summary>
        public virtual bool IsFocused
        {
            get
            {
                var isFocusedRes = Task.Run(() => WrappedElement.EvaluateAsync("node => document.activeElement === node")).GetAwaiter().GetResult();

                return bool.Parse(isFocusedRes.ToString());
            }
        }

        /// <summary>
        /// Returns a value of the component.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return Task.Run(() => WrappedElement.InputValueAsync()).GetAwaiter().GetResult();
            }
        }

        public override bool Equals(object obj)
        {
            var str = obj as string;

            if (str != null)
            {
                return str.Equals(Text);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a text for the current component.
        /// </summary>
        /// <returns>Text of the currrent component.</returns>
        public override string ToString()
        {
            return Text;
        }

        protected T RelocateOnStaleReference<T>(Func<T> act)
        {
            return act();
        }

        protected void RelocateOnStaleReference(Action act)
        {
            act();
        }
    }
}