using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TiaPortalOpennessDemo.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for TreeViewHelper
    public sealed class TreeViewHelper
    {
        #region SelectedItem

        /// <summary>Gets the selected item.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>Object</returns>
        /// TODO Edit XML Comment Template for GetSelectedItem
        public static object GetSelectedItem(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemProperty);
        }

        /// <summary>Sets the selected item.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        /// TODO Edit XML Comment Template for SetSelectedItem
        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// The selected item property
        /// </summary>
        /// TODO Edit XML Comment Template for SelectedItemProperty
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new UIPropertyMetadata(null, SelectedItemChanged));

        /// <summary>Selecteds the item changed.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for SelectedItemChanged
        private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var view = obj as TreeView;

            if (view == null || e.NewValue == null)
                return;

            view.SelectedItemChanged += (sender, e2) => SetSelectedItem(view, e2.NewValue);

            var command = (ICommand)view.GetValue(SelectedItemChangedProperty);
            if (command != null)
            {
                if (command.CanExecute(null))
                    command.Execute(new DependencyPropertyEventArgs(e));
            }
        }

        #endregion

        #region Selected Item Changed

        /// <summary>Gets the selected item changed.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>ICommand</returns>
        /// TODO Edit XML Comment Template for GetSelectedItemChanged
        public static ICommand GetSelectedItemChanged(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectedItemProperty);
        }

        /// <summary>Sets the selected item changed.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        /// TODO Edit XML Comment Template for SetSelectedItemChanged
        public static void SetSelectedItemChanged(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// The selected item changed property
        /// </summary>
        /// TODO Edit XML Comment Template for SelectedItemChangedProperty
        public static readonly DependencyProperty SelectedItemChangedProperty =
            DependencyProperty.RegisterAttached("SelectedItemChanged", typeof(ICommand), typeof(TreeViewHelper));

        #endregion

        #region Expanding Behavior

        /// <summary>
        /// The expanding behavior property
        /// </summary>
        /// TODO Edit XML Comment Template for ExpandingBehaviorProperty
        public static readonly DependencyProperty ExpandingBehaviorProperty =
                DependencyProperty.RegisterAttached("ExpandingBehavior", typeof(ICommand), typeof(TreeViewHelper),
                    new UIPropertyMetadata(OnExpandingBehaviorChanged));


        /// <summary>Sets the expanding behavior.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        /// TODO Edit XML Comment Template for SetExpandingBehavior
        public static void SetExpandingBehavior(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ExpandingBehaviorProperty, value);
        }

        /// <summary>Gets the expanding behavior.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>ICommand</returns>
        /// TODO Edit XML Comment Template for GetExpandingBehavior
        public static ICommand GetExpandingBehavior(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ExpandingBehaviorProperty);
        }

        /// <summary>
        /// Called when [expanding behavior changed].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for OnExpandingBehaviorChanged
        private static void OnExpandingBehaviorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var view = obj as TreeView;

            if (view == null || e.NewValue == null)
                return;

            view.SelectedItemChanged += (sender, e2) => SetSelectedItem(view, e2.NewValue);

            var command = (ICommand)view.GetValue(SelectedItemChangedProperty);
            if (command != null)
            {
                if (command.CanExecute(null))
                    command.Execute(new DependencyPropertyEventArgs(e));
            }
        }

        #endregion


        /// <summary>
        /// Prevents a default instance of the <see cref="TreeViewHelper"/> class from being created.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        private TreeViewHelper() { }
    }
    #region Event Args

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    /// TODO Edit XML Comment Template for DependencyPropertyEventArgs
    public class DependencyPropertyEventArgs : EventArgs
    {
        /// <summary>Gets the dependency property changed event arguments.</summary>
        /// <value>The dependency property changed event arguments.</value>
        /// TODO Edit XML Comment Template for DependencyPropertyChangedEventArgs
        public DependencyPropertyChangedEventArgs DependencyPropertyChangedEventArgs { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyEventArgs"/> class.
        /// </summary>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// TODO Edit XML Comment Template for #ctor
        public DependencyPropertyEventArgs(DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            DependencyPropertyChangedEventArgs = dependencyPropertyChangedEventArgs;
        }
    }

    #endregion
}
