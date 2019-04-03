/*
 * Author: Shon Verch
 * File Name: MemberTabControlContentTemplateSelector.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/2019
 * Modified Date: 04/03/2019
 * Description: A data template selector for the member tab control which enables dynamic data grid generation.
 */

using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// A <see cref="DataTemplate"/> selector for the member tab control which enables dynamic data grid generation.
    /// </summary>
    public class MemberTabControlContentTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Select the <see cref="DataTemplate"/> for the member tab.
        /// </summary>
        /// <remarks>
        /// Generates a dynamic <see cref="DataTemplate"/> using the <see cref="MemberTab"/> <see cref="Type"/>
        /// so that the columns are dynamically generated.
        /// </remarks>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;
            dynamic memberTab = item;
            Type memberTabType = memberTab.MemberType;

            FrameworkElementFactory scrollViewerFactory = new FrameworkElementFactory(typeof(ScrollViewer));
            scrollViewerFactory.AddHandler(UIElement.PreviewMouseWheelEvent, new MouseWheelEventHandler(OnPreviewMouseWheel));
            scrollViewerFactory.SetValue(ScrollViewer.CanContentScrollProperty, true);
            scrollViewerFactory.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            scrollViewerFactory.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);

            FrameworkElementFactory dataGridFactory = new FrameworkElementFactory(typeof(DataGrid));
            scrollViewerFactory.AppendChild(dataGridFactory);

            // Initialize the values, bindings, and events of the data grid.
            dataGridFactory.SetValue(FrameworkElement.NameProperty, "MembersDataGrid");
            dataGridFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            dataGridFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            dataGridFactory.SetValue(DataGrid.HeadersVisibilityProperty, DataGridHeadersVisibility.All);
            dataGridFactory.SetValue(DataGridBehavior.DisplayRowNumberProperty, true);
            dataGridFactory.SetValue(DataGrid.SelectionUnitProperty, DataGridSelectionUnit.FullRow);
            dataGridFactory.SetValue(DataGrid.SelectionModeProperty, DataGridSelectionMode.Single);
            dataGridFactory.SetValue(Control.BorderThicknessProperty, new Thickness(0));
            dataGridFactory.SetValue(DataGrid.CanUserSortColumnsProperty, false);
            dataGridFactory.SetValue(DataGrid.IsReadOnlyProperty, true);
            dataGridFactory.SetValue(DataGrid.AutoGenerateColumnsProperty, false);
            dataGridFactory.SetBinding(FrameworkElement.TagProperty, new Binding("MemberType"));
            dataGridFactory.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Data"));
            dataGridFactory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler((sender, routedEventArgs) =>
            {
                // Generate the columns of the data grid when it is loaded.
                DataGrid dataGrid = (DataGrid)routedEventArgs?.Source;
                if (dataGrid == null) return;

                dataGrid.Columns.Clear();

                foreach (PropertyInfo propertyInfo in GetMemberAttributes(memberTabType))
                {
                    MemberPropertyAttribute attribute = propertyInfo.GetCustomAttribute<MemberPropertyAttribute>();

                    // Convert the pascal-case name to a proper space-separated header
                    string properHeader = Regex.Replace(propertyInfo.Name, "(\\B[A-Z])", " $1");
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = properHeader,
                        Binding = new Binding(string.IsNullOrEmpty(attribute.OverrideBindingPath) ? propertyInfo.Name : attribute.OverrideBindingPath)
                    });
                }
            }));

            dataGridFactory.AddHandler(Selector.SelectionChangedEvent, new SelectionChangedEventHandler(OnSelectionChangedEvent));

            // Initialize the grid line colour of the data grid.
            SolidColorBrush gridLineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8D8D8"));
            dataGridFactory.SetValue(DataGrid.HorizontalGridLinesBrushProperty, gridLineBrush);
            dataGridFactory.SetValue(DataGrid.VerticalGridLinesBrushProperty, gridLineBrush);

            return new DataTemplate
            {
                VisualTree = scrollViewerFactory
            };
        }

        /// <summary>
        /// Handle the selection changed event for the data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnSelectionChangedEvent(object sender, SelectionChangedEventArgs args)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow == null) return;

            // Either way we need to clear the inspector stack panel.
            mainWindow.InspectorStackPanel.Children.Clear();

            // If there are no added items, we are deselecting something.
            if (args.AddedItems.Count == 0) return;

            Member member = (Member) args.AddedItems[0];
            foreach (PropertyInfo propertyInfo in GetMemberAttributes(member.GetType()))
            {
                mainWindow.InspectorStackPanel.Children.Add(new Label
                {
                    Content = propertyInfo.Name + " - " + propertyInfo.GetValue(member)
                });
            }
        }

        /// <summary>
        /// Handles the <see cref="ScrollViewer"/> preview mouse wheel event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnPreviewMouseWheel(object sender, MouseWheelEventArgs args)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - args.Delta);
            args.Handled = true;
        }

        /// <summary>
        /// Retrieve all the properties in the subclass and base class <see cref="Member"/> that are marked with the MemberPropertyAttribute.
        /// </summary>
        /// <param name="memberType">The type of the <see cref="Member"/> subclass.</param>
        private static PropertyInfo[] GetMemberAttributes(Type memberType)
        {        
            PropertyInfo[] properties = memberType?.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(MemberPropertyAttribute), false)).ToArray();

            // Sort the properties based on their order specified in the attribute.
            return properties?.OrderBy(p => p.GetCustomAttribute<MemberPropertyAttribute>().Order).ToArray();
        }
    }
}
