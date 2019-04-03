/*
 * Author: Shon Verch
 * File Name: MemberPropertyAttribute.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/19
 * Modified Date: 04/03/19
 * Description: Attribute that marks a property as a member
 *              property that should be displayed in the datagrid.
 */

using System;

namespace WrestlingManagementSystem
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MemberPropertyAttribute : Attribute
    {
        /// <summary>
        /// Specifies the order of the property in the data grid columns.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Overrides the data grid binding path of this <see cref="MemberPropertyAttribute"/>.
        /// </summary>
        public string OverrideBindingPath { get; }

        /// <summary>
        /// Initializes a  new <see cref="MemberPropertyAttribute"/>.
        /// </summary>
        /// <param name="order">The order of this <see cref="MemberPropertyAttribute"/> in the data grid columns.</param>
        /// <param name="overrideBindingPath">Overrides the data grid binding path of this <see cref="MemberPropertyAttribute"/>.</param>
        public MemberPropertyAttribute(int order = 0, string overrideBindingPath = null)
        {
            Order = order;
            OverrideBindingPath = overrideBindingPath;
        }
    }
}
