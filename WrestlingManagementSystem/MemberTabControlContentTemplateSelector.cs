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

                // Retrieve all the properties in the subclass and base class Member
                // that are marked with the MemberPropertyAttribute.
                PropertyInfo[] properties = memberTabType?.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.IsDefined(typeof(MemberPropertyAttribute), false)).ToArray();

                if (properties == null) return;

                // Sort the properties based on their order specified in the attribute.
                properties = properties.OrderBy(p => p.GetCustomAttribute<MemberPropertyAttribute>().Order).ToArray();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    // Convert the pascal-case name to a proper space-separated header
                    string properHeader = Regex.Replace(propertyInfo.Name, "(\\B[A-Z])", " $1");
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = properHeader,
                        Binding = new Binding(propertyInfo.Name)
                    });
                }
            }));

            // Initialize the grid line colours of the data grid.
            SolidColorBrush gridLineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8D8D8"));
            dataGridFactory.SetValue(DataGrid.HorizontalGridLinesBrushProperty, gridLineBrush);
            dataGridFactory.SetValue(DataGrid.VerticalGridLinesBrushProperty, gridLineBrush);

            return new DataTemplate
            {
                VisualTree = scrollViewerFactory
            };
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
    }
}
