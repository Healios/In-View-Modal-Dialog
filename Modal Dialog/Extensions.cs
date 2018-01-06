using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Modal_Dialog
{
    internal static class Extensions
    {
        /// <summary>
        /// Written by CrimsonX at https://stackoverflow.com/questions/636383/how-can-i-find-wpf-controls-by-name-or-type.
        /// Finds an element of a specified type and with a specified name in the specified parents visual tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">A parent of the specified child.</param>
        /// <param name="childName">x:Name or Name of child.</param>
        /// <returns>If the specified child can be found, it is returned. If it can not, null is returned.</returns>
        public static T FindElement<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            var checkType = false;

            // Validate passed data.
            if (parent == null)
                throw new Exception("The parent must not be null.");

            if (string.IsNullOrEmpty(childName) || string.IsNullOrWhiteSpace(childName))
                checkType = true;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    if (checkType)
                    {
                        foundChild = child as T;
                        break;
                    }

                    var frameworkElement = (FrameworkElement)child;
                    if (frameworkElement.Name == childName)
                    {
                        foundChild = child as T;
                        break;
                    }
                }

                foundChild = child.FindElement<T>(childName);
                if (foundChild != null) break;
            }

            return foundChild;
        }
    }
}