﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.WebElement
{
    public class FoundElementsEventArgs : EventArgs
    {
        public FoundElementsEventArgs(string by, IPage webDriver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata)
        {
            By = by;
            WebDriver = webDriver;
            Elements = elements;
            ComponentsListMetadata = componentsListMetadata;
        }

        public string By { get; }
        public IPage WebDriver { get; }
        public IReadOnlyList<ILocator> Elements { get; }
        public ComponentsListMetadata ComponentsListMetadata { get; }
    }
}
