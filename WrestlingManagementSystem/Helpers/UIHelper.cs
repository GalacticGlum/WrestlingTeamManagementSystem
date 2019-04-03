using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WrestlingManagementSystem.Helpers
{
    public static class UIHelper
    {
        public static List<Control> GetChildren(this DependencyObject parent)
        {
            List<Control> controls = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is Control control)
                {
                    controls.Add(control);
                }

                controls.AddRange(child.GetChildren());
            }

            return controls;
        }
    }
}
