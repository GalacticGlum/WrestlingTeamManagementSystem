/*
 * Author: Shon Verch
 * File Name: UIHelper.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/2019
 * Modified Date: 04/03/2019
 * Description: WPF UI helpers.
 */

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WrestlingManagementSystem.Helpers
{
    /// <summary>
    /// WPF UI helpers.
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Retrieves all the child controls of the specified parent <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="parent">The parent <see cref="DependencyObject"/>.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="Control"/> objects.</returns>
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
