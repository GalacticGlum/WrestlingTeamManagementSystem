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
        /// Specifies the order of the property in the datagrid columns.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Initializesa  new <see cref="MemberPropertyAttribute"/>.
        /// </summary>
        /// <param name="order">The order of this <see cref="MemberPropertyAttribute"/> in the datagrid columns.</param>
        public MemberPropertyAttribute(int order = 0)
        {
            Order = order;
        }
    }
}
