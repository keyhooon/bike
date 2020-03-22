using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace bike
{
    public static class VisualTreeHelper
    {
        public static T GetParent<T>(this Element element) where T : Element
        {
            if (element is T)
            {
                return element as T;
            }
            else
            {
                if (element.Parent != null)
                {
                    return element.Parent.GetParent<T>();
                }

                return default(T);
            }
        }

        public static IEnumerable<T> GetChildren<T>(this Element element) where T : Element
        {
            var properties = element.GetType().GetRuntimeProperties();

            // try to parse the Content property
            var contentProperty = properties.FirstOrDefault(w => w.Name == "Content");
            if (contentProperty != null)
            {
                var content = contentProperty.GetValue(element) as Element;
                if (content != null)
                {
                    if (content is T)
                    {
                        yield return content as T;
                    }
                    foreach (var child in content.GetChildren<T>())
                    {
                        yield return child;
                    }
                }
            }
            else
            {
                // try to parse the Children property
                var childrenProperty = properties.FirstOrDefault(w => w.Name == "Children");
                if (childrenProperty != null)
                {
                    // loop through children
                    IEnumerable children = childrenProperty.GetValue(element) as IEnumerable;
                    foreach (var child in children)
                    {
                        var childVisualElement = child as Element;
                        if (childVisualElement != null)
                        {
                            // return match
                            if (childVisualElement is T)
                            {
                                yield return childVisualElement as T;
                            }

                            // return recursive results of children
                            foreach (var childVisual in childVisualElement.GetChildren<T>())
                            {
                                yield return childVisual;
                            }
                        }
                    }
                }
            }
        }
    }
}