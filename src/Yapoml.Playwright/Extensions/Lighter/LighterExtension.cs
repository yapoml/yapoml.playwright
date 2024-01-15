//using OpenQA.Selenium;
//using System;
//using System.Drawing;
//using Yapoml.Framework.Options;
//using Yapoml.Playwright.Events;
//using Yapoml.Playwright.Events.Args.Element;

//namespace Yapoml.Playwright
//{
//    public static class LighterExtension
//    {
//        private static int _delay;

//        private static int _fadeOutSpeed;

//        private static Color _color = Color.FromArgb(70, 255, 255, 0);

//        public static ISpaceOptions UseLighter(this ISpaceOptions spaceOptions, int delay = 200, int fadeOutSpeed = 200, Color color = default)
//        {
//            _delay = delay;
//            _fadeOutSpeed = fadeOutSpeed;

//            if (color != default)
//            {
//                _color = color;
//            }

//            var eventSource = spaceOptions.Services.Get<IEventSource>();

//            eventSource.ComponentEventSource.OnFoundComponent += ComponentEventSource_OnFoundComponent;
//            eventSource.ComponentEventSource.OnFoundComponents += ComponentEventSource_OnFoundComponents;

//            return spaceOptions;
//        }

//        private static void ComponentEventSource_OnFoundComponent(object sender, FoundElementEventArgs e)
//        {
//            HightlightElement(e.Driver, e.Element);
//        }

//        private static void ComponentEventSource_OnFoundComponents(object sender, FoundElementsEventArgs e)
//        {
//            foreach (var element in e.Elements)
//            {
//                HightlightElement(e.Driver, element);
//            }
//        }

//        private static void HightlightElement(IWebDriver driver, IElement element)
//        {
//            var jsExecutor = driver as IJavaScriptExecutor;

//            if (jsExecutor != null)
//            {
//                try
//                {
//                    var backgroundColor = element.GetCssValue("background-color");

//                    jsExecutor.ExecuteScript($"arguments[0].setAttribute('style', 'background: rgba({_color.R}, {_color.G}, {_color.B}, {(float)_color.A / 100});');", element);

//                    System.Threading.Thread.Sleep(_delay);

//                    jsExecutor.ExecuteScript($"arguments[0].setAttribute('style', 'background: {backgroundColor}; transition: all {_fadeOutSpeed}ms ease-in-out;');", element);


//                }
//                catch (Exception) { }
//            }
//        }
//    }
//}
