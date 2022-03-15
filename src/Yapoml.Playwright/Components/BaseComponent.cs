using Microsoft.Playwright;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Yapoml.Options;
using Yapoml.Playwright.Events;

namespace Yapoml.Playwright.Components
{
    /// <inheritdoc/>
    public partial class BaseComponent : ILocator
    {
        protected IPage Page { get; private set; }

        public ILocator WrappedLocator { get; private set; }

        protected ISpaceOptions SpaceOptions { get; private set; }

        protected IComponentEventSource EventSource { get; private set; }

        public BaseComponent(IPage page, ILocator locator, ISpaceOptions spaceOptions)
        {
            Page = page;
            WrappedLocator = locator;
            SpaceOptions = spaceOptions;

            EventSource = spaceOptions.Get<IEventSource>().ComponentEventSource;
        }

        public ILocator First => WrappedLocator.First;

        public ILocator Last => WrappedLocator.Last;

        IPage ILocator.Page => WrappedLocator.Page;

        public Task<IReadOnlyList<string>> AllInnerTextsAsync()
        {
            return WrappedLocator.AllInnerTextsAsync();
        }

        public Task<IReadOnlyList<string>> AllTextContentsAsync()
        {
            return WrappedLocator?.AllTextContentsAsync();
        }

        public Task<LocatorBoundingBoxResult> BoundingBoxAsync(LocatorBoundingBoxOptions options = null)
        {
            return WrappedLocator.BoundingBoxAsync(options);
        }

        public Task CheckAsync(LocatorCheckOptions options = null)
        {
            return WrappedLocator.CheckAsync(options);
        }

        public Task ClickAsync(LocatorClickOptions options = null)
        {
            return WrappedLocator.ClickAsync(options);
        }

        public Task<int> CountAsync()
        {
            return WrappedLocator.CountAsync();
        }

        public Task DblClickAsync(LocatorDblClickOptions options = null)
        {
            return WrappedLocator.DblClickAsync(options);
        }

        public Task DispatchEventAsync(string type, object eventInit = null, LocatorDispatchEventOptions options = null)
        {
            return WrappedLocator.DispatchEventAsync(type, eventInit, options);
        }

        public Task DragToAsync(ILocator target, LocatorDragToOptions options = null)
        {
            return WrappedLocator.DragToAsync(target, options);
        }

        public Task<IElementHandle> ElementHandleAsync(LocatorElementHandleOptions options = null)
        {
            return WrappedLocator.ElementHandleAsync(options);
        }

        public Task<IReadOnlyList<IElementHandle>> ElementHandlesAsync()
        {
            return WrappedLocator.ElementHandlesAsync();
        }

        public Task<T> EvaluateAsync<T>(string expression, object arg = null, LocatorEvaluateOptions options = null)
        {
            return WrappedLocator.EvaluateAsync<T>(expression, arg, options);
        }

        public Task<T> EvaluateAllAsync<T>(string expression, object arg = null)
        {
            return WrappedLocator.EvaluateAllAsync<T>(expression, arg);
        }

        public Task<IJSHandle> EvaluateHandleAsync(string expression, object arg = null, LocatorEvaluateHandleOptions options = null)
        {
            return WrappedLocator.EvaluateHandleAsync(expression, arg, options);
        }

        public Task FillAsync(string value, LocatorFillOptions options = null)
        {
            return WrappedLocator.FillAsync(value, options);
        }

        public Task FocusAsync(LocatorFocusOptions options = null)
        {
            return WrappedLocator.FocusAsync(options);
        }

        public IFrameLocator FrameLocator(string selector)
        {
            return WrappedLocator.FrameLocator(selector);
        }

        public Task<string> GetAttributeAsync(string name, LocatorGetAttributeOptions options = null)
        {
            return WrappedLocator.GetAttributeAsync(name, options);
        }

        public Task HoverAsync(LocatorHoverOptions options = null)
        {
            return WrappedLocator.HoverAsync(options);
        }

        public Task<string> InnerHTMLAsync(LocatorInnerHTMLOptions options = null)
        {
            return WrappedLocator.InnerHTMLAsync(options);
        }

        public Task<string> InnerTextAsync(LocatorInnerTextOptions options = null)
        {
            return WrappedLocator.InnerTextAsync(options);
        }

        public Task<string> InputValueAsync(LocatorInputValueOptions options = null)
        {
            return WrappedLocator.InputValueAsync(options);
        }

        public Task<bool> IsCheckedAsync(LocatorIsCheckedOptions options = null)
        {
            return WrappedLocator.IsCheckedAsync(options);
        }

        public Task<bool> IsDisabledAsync(LocatorIsDisabledOptions options = null)
        {
            return WrappedLocator.IsDisabledAsync(options);
        }

        public Task<bool> IsEditableAsync(LocatorIsEditableOptions options = null)
        {
            return WrappedLocator.IsEditableAsync(options);
        }

        public Task<bool> IsEnabledAsync(LocatorIsEnabledOptions options = null)
        {
            return WrappedLocator.IsEnabledAsync(options);
        }

        public Task<bool> IsHiddenAsync(LocatorIsHiddenOptions options = null)
        {
            return WrappedLocator.IsHiddenAsync(options);
        }

        public Task<bool> IsVisibleAsync(LocatorIsVisibleOptions options = null)
        {
            return WrappedLocator.IsVisibleAsync(options);
        }

        public ILocator Locator(string selector, LocatorLocatorOptions options = null)
        {
            return WrappedLocator.Locator(selector, options);
        }

        public ILocator Nth(int index)
        {
            return WrappedLocator.Nth(index);
        }

        public Task PressAsync(string key, LocatorPressOptions options = null)
        {
            return WrappedLocator.PressAsync(key, options);
        }

        public Task<byte[]> ScreenshotAsync(LocatorScreenshotOptions options = null)
        {
            return WrappedLocator.ScreenshotAsync(options);
        }

        public Task ScrollIntoViewIfNeededAsync(LocatorScrollIntoViewIfNeededOptions options = null)
        {
            return WrappedLocator.ScrollIntoViewIfNeededAsync(options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(string values, LocatorSelectOptionOptions options = null)
        {
            return WrappedLocator.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IElementHandle values, LocatorSelectOptionOptions options = null)
        {
            return WrappedLocator.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<string> values, LocatorSelectOptionOptions options = null)
        {
            return WrappedLocator.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(SelectOptionValue values, LocatorSelectOptionOptions options = null)
        {
            return WrappedLocator.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<IElementHandle> values, LocatorSelectOptionOptions options = null)
        {
            return WrappedLocator.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<SelectOptionValue> values, LocatorSelectOptionOptions options = null)
        {
            return WrappedLocator.SelectOptionAsync(values, options);
        }

        public Task SelectTextAsync(LocatorSelectTextOptions options = null)
        {
            return WrappedLocator.SelectTextAsync(options);
        }

        public Task SetCheckedAsync(bool checkedState, LocatorSetCheckedOptions options = null)
        {
            return WrappedLocator.SetCheckedAsync(checkedState, options);
        }

        public Task SetInputFilesAsync(string files, LocatorSetInputFilesOptions options = null)
        {
            return WrappedLocator.SetInputFilesAsync(files, options);
        }

        public Task SetInputFilesAsync(IEnumerable<string> files, LocatorSetInputFilesOptions options = null)
        {
            return WrappedLocator.SetInputFilesAsync(files, options);
        }

        public Task SetInputFilesAsync(FilePayload files, LocatorSetInputFilesOptions options = null)
        {
            return WrappedLocator.SetInputFilesAsync(files, options);
        }

        public Task SetInputFilesAsync(IEnumerable<FilePayload> files, LocatorSetInputFilesOptions options = null)
        {
            return WrappedLocator.SetInputFilesAsync(files, options);
        }

        public Task TapAsync(LocatorTapOptions options = null)
        {
            return WrappedLocator.TapAsync(options);
        }

        public Task<string> TextContentAsync(LocatorTextContentOptions options = null)
        {
            return WrappedLocator.TextContentAsync(options);
        }

        public Task TypeAsync(string text, LocatorTypeOptions options = null)
        {
            return WrappedLocator.TypeAsync(text, options);
        }

        public Task UncheckAsync(LocatorUncheckOptions options = null)
        {
            return WrappedLocator.UncheckAsync(options);
        }

        public Task WaitForAsync(LocatorWaitForOptions options = null)
        {
            return WrappedLocator.WaitForAsync(options);
        }

        public Task<JsonElement?> EvaluateAsync(string expression, object arg = null, LocatorEvaluateOptions options = null)
        {
            return WrappedLocator.EvaluateAsync(expression, arg, options);
        }
    }
}