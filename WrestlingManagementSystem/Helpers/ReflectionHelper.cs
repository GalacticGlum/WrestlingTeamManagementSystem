/*
 * Author: Shon Verch
 * File Name: ReflectionHelper.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/2019
 * Modified Date: 04/03/2019
 * Description: Utilities for reflection.
 */

using System;

namespace WrestlingManagementSystem.Helpers
{
    /// <summary>
    /// Utilities for reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Determines whether an Type is numeric.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>A boolean indicating whether the specified type is numeric.</returns>
        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
