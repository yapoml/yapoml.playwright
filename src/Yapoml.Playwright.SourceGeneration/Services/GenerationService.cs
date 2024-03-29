﻿using Scriban.Runtime;
using System.Collections.Generic;
using System.Linq;
using Yapoml.Framework.Workspace;

namespace Yapoml.Playwright.SourceGeneration.Services
{
    internal class GenerationService : ScriptObject
    {
        public static bool IsXpath(string expression)
        {
            var isXpath = true;

            try
            {
                System.Xml.XPath.XPathExpression.Compile(expression);
            }
            catch { isXpath = false; }

            return isXpath;
        }

        private static Dictionary<ComponentContext, string> _returnTypesCache = new Dictionary<ComponentContext, string>();

        public static string GetComponentReturnType(ComponentContext component)
        {
            if (_returnTypesCache.TryGetValue(component, out var cachedRetType))
            {
                return cachedRetType;
            }
            else
            {
                var retType = $"{component.SingularName}Component";

                _returnTypesCache[component] = retType;

                return retType;
            }
        }

        private static Dictionary<ComponentContext, string> _returnFullTypesCache = new Dictionary<ComponentContext, string>();

        public static string GetComponentReturnFullType(ComponentContext component)
        {
            if (_returnFullTypesCache.TryGetValue(component, out var cachedRetType))
            {
                return cachedRetType;
            }
            else
            {
                var retType = $"global::{component.Namespace}.{component.SingularName}Component";

                _returnFullTypesCache[component] = retType;

                return retType;
            }
        }

        public static string GetPageClassName(ScriptObject page)
        {
            var name = page.GetSafeValue<string>("name");

            return GetPageClassName(name);
        }

        public static string GetPageClassNameForPage(PageContext page)
        {
            return GetPageClassName(page.Name);
        }

        private static string GetPageClassName(string name)
        {
            if (name.EndsWith("Page"))
            {
                return name;
            }
            else
            {
                return $"{name}Page";
            }
        }

        public static bool HasUserDefinedBaseComponent(ScriptObject space)
        {
            if (space.TryGetValue("components", out object oComponents))
            {
                var components = oComponents as IList<ComponentContext>;

                return components.FirstOrDefault(c => c.Name.Equals("base", System.StringComparison.InvariantCultureIgnoreCase)) != null;
            }
            else
            {
                return false;
            }
        }

        public static bool HasUserDefinedBasePage(ScriptObject space)
        {
            if (space.TryGetValue("pages", out object oPages))
            {
                var pages = oPages as IList<PageContext>;

                return pages.FirstOrDefault(
                    c => c.Name.Equals("Base", System.StringComparison.InvariantCultureIgnoreCase)
                    || c.Name.Equals("BasePage", System.StringComparison.InvariantCultureIgnoreCase)) != null;
            }
            else
            {
                return false;
            }
        }

        public static string GetPageAccessorName(string pageName)
        {
            if (pageName.EndsWith("page", System.StringComparison.OrdinalIgnoreCase))
            {
                return pageName;
            }
            else
            {
                return pageName + "Page";
            }
        }

        public static string Escape(string str)
        {
            return str.Replace("\"", "\\\"");
        }

        public static string Singularize(string str)
        {
            return new Framework.Workspace.Services.PluralizationService().Singularize(str);
        }
    }
}