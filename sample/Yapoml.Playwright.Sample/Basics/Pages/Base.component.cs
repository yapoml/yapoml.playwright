﻿using System;

namespace Yapoml.Playwright.Sample.Basics.Pages
{
    partial class BaseComponent<TComponent, TConditions, TCondition>
    {
        public override TComponent ScrollIntoView()
        {
            Console.WriteLine($"I am invoked each time when {Metadata.Name} component is scrolling into view");

            return base.ScrollIntoView();
        }

        partial class Conditions<TSelf>
        {
            public TSelf IsNotWhite()
            {
                using var logScope = Logger.BeginLogScope($"Expect {this.ElementHandler.ComponentMetadata.Name} color is not white");

                return logScope.Execute(() =>
                {
                    return Styles.BackgroundColor.DoesNotContain("255, 255, 255");
                });
            }
        }
    }
}
