using Microsoft.Playwright;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Yapoml.Options;
using Yapoml.Playwright.Events;

namespace Yapoml.Playwright.Components
{
    /// <inheritdoc/>
    public partial class BaseComponent : IElementHandle
    {
        protected IPage Page { get; private set; }

        public IElementHandle WrappedElementHandle { get; private set; }

        protected ISpaceOptions SpaceOptions { get; private set; }

        protected IComponentEventSource EventSource { get; private set; }

        public BaseComponent(IPage page, IElementHandle elementHandle, ISpaceOptions spaceOptions)
        {
            Page = page;
            WrappedElementHandle = elementHandle;
            SpaceOptions = spaceOptions;

            EventSource = spaceOptions.Get<IEventSource>().ComponentEventSource;
        }

        public Task<ElementHandleBoundingBoxResult> BoundingBoxAsync()
        {
            return WrappedElementHandle.BoundingBoxAsync();
        }

        public Task CheckAsync(ElementHandleCheckOptions options = null)
        {
            return WrappedElementHandle.CheckAsync(options);
        }

        public Task ClickAsync(ElementHandleClickOptions options = null)
        {
            return WrappedElementHandle.ClickAsync(options);
        }

        public Task<IFrame> ContentFrameAsync()
        {
            return WrappedElementHandle.ContentFrameAsync();
        }

        public Task DblClickAsync(ElementHandleDblClickOptions options = null)
        {
            return WrappedElementHandle.DblClickAsync(options);
        }

        public Task DispatchEventAsync(string type, object eventInit = null)
        {
            return WrappedElementHandle.DispatchEventAsync(type, eventInit);
        }

        public Task<T> EvalOnSelectorAsync<T>(string selector, string expression, object arg = null)
        {
            return WrappedElementHandle.EvalOnSelectorAsync<T>(selector, expression, arg);
        }

        public Task<T> EvalOnSelectorAllAsync<T>(string selector, string expression, object arg = null)
        {
            return WrappedElementHandle.EvalOnSelectorAllAsync<T>(selector, expression, arg);
        }

        public Task FillAsync(string value, ElementHandleFillOptions options = null)
        {
            return WrappedElementHandle.FillAsync(value, options);
        }

        public Task FocusAsync()
        {
            return WrappedElementHandle.FocusAsync();
        }

        public Task<string> GetAttributeAsync(string name)
        {
            return WrappedElementHandle.GetAttributeAsync(name);
        }

        public Task HoverAsync(ElementHandleHoverOptions options = null)
        {
            return WrappedElementHandle.HoverAsync(options);
        }

        public Task<string> InnerHTMLAsync()
        {
            return WrappedElementHandle.InnerHTMLAsync();
        }

        public Task<string> InnerTextAsync()
        {
            return WrappedElementHandle.InnerTextAsync();
        }

        public Task<string> InputValueAsync(ElementHandleInputValueOptions options = null)
        {
            return WrappedElementHandle.InputValueAsync(options);
        }

        public Task<bool> IsCheckedAsync()
        {
            return WrappedElementHandle.IsCheckedAsync();
        }

        public Task<bool> IsDisabledAsync()
        {
            return WrappedElementHandle.IsDisabledAsync();
        }

        public Task<bool> IsEditableAsync()
        {
            return WrappedElementHandle.IsEditableAsync();
        }

        public Task<bool> IsEnabledAsync()
        {
            return WrappedElementHandle.IsEnabledAsync();
        }

        public Task<bool> IsHiddenAsync()
        {
            return WrappedElementHandle?.IsHiddenAsync();
        }

        public Task<bool> IsVisibleAsync()
        {
            return WrappedElementHandle.IsVisibleAsync();
        }

        public Task<IFrame> OwnerFrameAsync()
        {
            return WrappedElementHandle.OwnerFrameAsync();
        }

        public Task PressAsync(string key, ElementHandlePressOptions options = null)
        {
            return WrappedElementHandle.PressAsync(key, options);
        }

        public Task<IElementHandle> QuerySelectorAsync(string selector)
        {
            return WrappedElementHandle.QuerySelectorAsync(selector);
        }

        public Task<IReadOnlyList<IElementHandle>> QuerySelectorAllAsync(string selector)
        {
            return WrappedElementHandle.QuerySelectorAllAsync(selector);
        }

        public Task<byte[]> ScreenshotAsync(ElementHandleScreenshotOptions options = null)
        {
            return WrappedElementHandle.ScreenshotAsync(options);
        }

        public Task ScrollIntoViewIfNeededAsync(ElementHandleScrollIntoViewIfNeededOptions options = null)
        {
            return WrappedElementHandle.ScrollIntoViewIfNeededAsync(options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(string values, ElementHandleSelectOptionOptions options = null)
        {
            return WrappedElementHandle.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IElementHandle values, ElementHandleSelectOptionOptions options = null)
        {
            return WrappedElementHandle.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<string> values, ElementHandleSelectOptionOptions options = null)
        {
            return WrappedElementHandle.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(SelectOptionValue values, ElementHandleSelectOptionOptions options = null)
        {
            return WrappedElementHandle.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<IElementHandle> values, ElementHandleSelectOptionOptions options = null)
        {
            return WrappedElementHandle.SelectOptionAsync(values, options);
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<SelectOptionValue> values, ElementHandleSelectOptionOptions options = null)
        {
            return WrappedElementHandle.SelectOptionAsync(values, options);
        }

        public Task SelectTextAsync(ElementHandleSelectTextOptions options = null)
        {
            return WrappedElementHandle.SelectTextAsync(options);
        }

        public Task SetCheckedAsync(bool checkedState, ElementHandleSetCheckedOptions options = null)
        {
            return WrappedElementHandle.SetCheckedAsync(checkedState, options);
        }

        public Task SetInputFilesAsync(string files, ElementHandleSetInputFilesOptions options = null)
        {
            return WrappedElementHandle.SetInputFilesAsync(files, options);
        }

        public Task SetInputFilesAsync(IEnumerable<string> files, ElementHandleSetInputFilesOptions options = null)
        {
            return WrappedElementHandle.SetInputFilesAsync(files, options);
        }

        public Task SetInputFilesAsync(FilePayload files, ElementHandleSetInputFilesOptions options = null)
        {
            return WrappedElementHandle.SetInputFilesAsync(files, options);
        }

        public Task SetInputFilesAsync(IEnumerable<FilePayload> files, ElementHandleSetInputFilesOptions options = null)
        {
            return WrappedElementHandle.SetInputFilesAsync(files, options);
        }

        public Task TapAsync(ElementHandleTapOptions options = null)
        {
            return WrappedElementHandle.TapAsync(options);
        }

        public Task<string> TextContentAsync()
        {
            return WrappedElementHandle.TextContentAsync();
        }

        public Task TypeAsync(string text, ElementHandleTypeOptions options = null)
        {
            return WrappedElementHandle.TypeAsync(text, options);
        }

        public Task UncheckAsync(ElementHandleUncheckOptions options = null)
        {
            return WrappedElementHandle.UncheckAsync(options);
        }

        public Task WaitForElementStateAsync(ElementState state, ElementHandleWaitForElementStateOptions options = null)
        {
            return WrappedElementHandle.WaitForElementStateAsync(state, options);
        }

        public Task<IElementHandle> WaitForSelectorAsync(string selector, ElementHandleWaitForSelectorOptions options = null)
        {
            return WrappedElementHandle.WaitForSelectorAsync(selector, options);
        }

        public Task<JsonElement?> EvalOnSelectorAsync(string selector, string expression, object arg = null)
        {
            return WrappedElementHandle.EvalOnSelectorAsync(selector, expression, arg);
        }

        public IElementHandle AsElement()
        {
            return WrappedElementHandle.AsElement();
        }

        public Task<T> EvaluateAsync<T>(string expression, object arg = null)
        {
            return WrappedElementHandle.EvaluateAsync<T>(expression, arg);
        }

        public Task<IJSHandle> EvaluateHandleAsync(string expression, object arg = null)
        {
            return WrappedElementHandle.EvaluateHandleAsync(expression, arg);
        }

        public Task<Dictionary<string, IJSHandle>> GetPropertiesAsync()
        {
            return WrappedElementHandle.GetPropertiesAsync();
        }

        public Task<IJSHandle> GetPropertyAsync(string propertyName)
        {
            return WrappedElementHandle.GetPropertyAsync(propertyName);
        }

        public Task<T> JsonValueAsync<T>()
        {
            return WrappedElementHandle.JsonValueAsync<T>();
        }

        public Task<JsonElement?> EvaluateAsync(string expression, object arg = null)
        {
            return WrappedElementHandle.EvaluateAsync(expression, arg);
        }

        public ValueTask DisposeAsync()
        {
            return WrappedElementHandle.DisposeAsync();
        }
    }
}