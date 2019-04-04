/*
 * Author: Shon Verch
 * File Name: MemberTabControlContentTemplateSelector.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/2019
 * Modified Date: 04/03/2019
 * Description: A data template selector for the member tab control which enables dynamic data grid generation.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WrestlingManagementSystem.Helpers;

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
                IEnumerable<PropertyInfo> memberAttributes = GetMemberAttributes(memberTabType);
                if (memberAttributes == null) return;

                foreach (PropertyInfo propertyInfo in memberAttributes)
                {
                    MemberPropertyAttribute attribute = propertyInfo.GetCustomAttribute<MemberPropertyAttribute>();
                    Binding binding = new Binding(string.IsNullOrEmpty(attribute.OverrideBindingPath) ? propertyInfo.Name : attribute.OverrideBindingPath);
                    if (propertyInfo.PropertyType == typeof(DateTime))
                    {
                        binding.StringFormat = "MM/dd/yyy";
                    }

                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = GetProperPropertyName(propertyInfo),
                        Binding = binding
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

            mainWindow.ResetInspector();
            MainWindowDataContext dataContext = (MainWindowDataContext)mainWindow.DataContext;

            // If there are no added items, we are deselecting something.
            if (args.AddedItems.Count == 0)
            {
                dataContext.IsMemberSelected = false;
                return;
            }

            dataContext.IsMemberSelected = true;

            Member member = (Member) args.AddedItems[0];
            if (member == null) return;

            IEnumerable<PropertyInfo> memberAttributes = GetMemberAttributes(member?.GetType());
            if (memberAttributes == null) return;

            // Bind the type combobox to the member type
            mainWindow.TypeSelectionComboBox.SelectedItem = member.GetType();

            foreach (PropertyInfo propertyInfo in memberAttributes)
            {
                MemberPropertyAttribute attribute = propertyInfo.GetCustomAttribute<MemberPropertyAttribute>();
                InspectorInput inspectorInput = new InspectorInput
                {
                    InputName = GetProperPropertyName(propertyInfo)
                };

                // Use the type of the property to generate the input widget
                Control inputWidget;

                string bindingPath = propertyInfo.Name;
                Binding binding = new Binding(bindingPath);

                if (attribute.IsReadonly)
                {
                    // A readonly attribute is simply a label with its value.
                    inputWidget = new Label
                    {
                        DataContext = member
                    };

                    // A one-way binding simply gets the property in the UI.
                    // A readonly property cannot be bound two-way since it can't
                    // be set.
                    binding.Mode = BindingMode.OneWay;
                    inputWidget.SetBinding(ContentControl.ContentProperty, binding);
                }
                else
                {
                    // Numeric and string types use a text box
                    // We must make sure the type is NOT an enum since an enum is type of numeric.
                    if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType.IsNumericType() && !propertyInfo.PropertyType.IsEnum)
                    {
                        inputWidget = new TextBox
                        {
                            DataContext = member
                        };

                        inputWidget.SetBinding(TextBox.TextProperty, binding);
                    }
                    // Enum types use a combobox 
                    else if (propertyInfo.PropertyType.IsEnum)
                    {
                        inputWidget = new ComboBox
                        {
                            ItemsSource = Enum.GetValues(propertyInfo.PropertyType),
                            DataContext = member
                        };

                        inputWidget.SetBinding(Selector.SelectedItemProperty, binding);
                    }
                    // Datetime types use a datepicker
                    else if (propertyInfo.PropertyType == typeof(DateTime))
                    {
                        inputWidget = new DatePicker
                        {
                            SelectedDateFormat = DatePickerFormat.Short,
                            DataContext = member
                        };

                        binding.StringFormat = "MM/dd/yyyy";
                        inputWidget.SetBinding(DatePicker.SelectedDateProperty, binding);
                    }
                    // Boolean types use a checkbox
                    else if (propertyInfo.PropertyType == typeof(bool))
                    {
                        inputWidget = new CheckBox
                        {
                            DataContext = member
                        };

                        inputWidget.SetBinding(ToggleButton.IsCheckedProperty, binding);
                    }
                    else
                    {
                        // The input widget only implements text (string and numeric types), dropdown-based input (enum types),
                        // date pickers (datetime), and checkboxes (bool).

                        // Any other types are undefined.
                        throw new NotImplementedException();
                    }
                }

                inspectorInput.InputStackPanel.Children.Add(inputWidget);
                mainWindow.InspectorStackPanel.Children.Add(inspectorInput);
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
        public static IEnumerable<PropertyInfo> GetMemberAttributes(Type memberType)
        {        
            PropertyInfo[] properties = memberType?.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(MemberPropertyAttribute), false)).ToArray();
           
            // Sort the properties based on their order specified in the attribute.
            return properties?.OrderBy(p => p.GetCustomAttribute<MemberPropertyAttribute>().Order).ToArray();
        }

        /// <summary>
        /// Convert the pascal-case name to a proper space-separated header
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        private static string GetProperPropertyName(PropertyInfo propertyInfo) => Regex.Replace(propertyInfo.Name, "(\\B[A-Z])", " $1");
    }
}
