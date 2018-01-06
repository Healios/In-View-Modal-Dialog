using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Prism.Common;
using Prism.Interactivity;
using Prism.Interactivity.InteractionRequest;

namespace Modal_Dialog
{
    /// Copyright 2018 Marcus Raevmann Brown
    ///
    /// Licensed under the Apache License, Version 2.0 (the "License");
    /// you may not use this file except in compliance with the License.
    /// You may obtain a copy of the License at
    ///
    /// http://www.apache.org/licenses/LICENSE-2.0
    ///
    /// Unless required by applicable law or agreed to in writing, software
    /// distributed under the License is distributed on an "AS IS" BASIS,
    /// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    /// See the License for the specific language governing permissions and
    /// limitations under the License.
    /// 
    /// <summary>
    /// An "in-view" dialog action. See <see cref="PopupWindowAction"/> for a dialog in a seperate window.
    /// </summary>
    internal class InViewDialogAction : TriggerAction<FrameworkElement>
    {
        // Properties.
        /// <summary>
        /// The content of the child FrameworkElement to display as part of the popup.
        /// </summary>
        public static readonly DependencyProperty FrameworkElementContentProperty =
        DependencyProperty.Register("FrameworkElementContent", typeof(FrameworkElement), typeof(InViewDialogAction), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the content of the FrameworkElement.
        /// </summary>
        public FrameworkElement FrameworkElementContent
        {
            get { return (FrameworkElement)GetValue(FrameworkElementContentProperty); }
            set { SetValue(FrameworkElementContentProperty, value); }
        }

        /// <summary>
        /// The type of content of the child FrameworkElement to display as part of the dialog.
        /// </summary>
        public static readonly DependencyProperty FrameworkElementContentTypeProperty =
        DependencyProperty.Register("FrameworkElementContentType", typeof(Type), typeof(InViewDialogAction), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the type of content of the child FrameworkElement.
        /// </summary>
        public Type FrameworkElementContentType
        {
            get { return (Type)GetValue(FrameworkElementContentTypeProperty); }
            set { SetValue(FrameworkElementContentTypeProperty, value); }
        }


        // Methods.
        /// <summary>
        /// Displays the child FrameworkElement and collects results for <see cref="IInteractionRequest"/>.
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null) return;

            // Add specified framework element to the current window.
            var window = Window.GetWindow(AssociatedObject);

            // Create a DockPanel that will act as a shadow for the specified framework element. It will also make the framework element "modal" by blocking off the rest of the window as long as the framework element is active.
            var backgroundElement = new DockPanel { Background = new SolidColorBrush(Color.FromArgb(90, 0, 0, 0)), VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            backgroundElement.SetValue(Grid.ColumnSpanProperty, 20);
            backgroundElement.SetValue(Grid.RowSpanProperty, 20);

            // Find the first grid in the window,- works well enough for my purpose. One could modify this so that it accepts a name and/or type, so that one can specify individual parts of a view.
            Grid grid = window.FindElement<Grid>("");
            grid.Children.Add(backgroundElement);

            // If we haven't specified any content to show, alert the programmer.
            if (FrameworkElementContent == null) throw new Exception("No FrameworkElement has been specified as dialog content.");
            backgroundElement.Children.Add(FrameworkElementContent);

            Action<IInteractionRequestAware> setNotificationAndClose = (iira) =>
            {
                iira.Notification = args.Context;
                iira.FinishInteraction = () =>
                {
                    // Remove the child FrameworkElement (the dialog) and execute the callback method.
                    backgroundElement.Children.Remove(FrameworkElementContent);
                    grid.Children.Remove(backgroundElement);
                    args.Callback();
                };
            };

            MvvmHelpers.ViewAndViewModelAction(FrameworkElementContent, setNotificationAndClose);
        }
    }
}
