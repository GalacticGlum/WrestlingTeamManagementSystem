/*
 * Author: Shon Verch
 * File Name: LoggerDestination.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/31/2019
 * Modified Date: 03/31/2019
 * Description: The different types of destinations the Logger can log to.
 */

using System;

namespace WrestlingManagementSystem.Logging
{
    /// <summary>
    /// The different types of destinations the <see cref="Logger"/> can log to.
    /// </summary>
    [Flags]
    public enum LoggerDestination
    {
        /// <summary>
        /// Do not log to anywhere.
        /// </summary>
        None = 1,

        /// <summary>
        /// Log to the standard output (<see cref="Console"/> or <see cref="Trace"/>).
        /// </summary>
        Output = 2,

        /// <summary>
        /// Log to file.
        /// </summary>
        File = 4,

        /// <summary>
        /// Log by displaying a message box
        /// </summary>
        Form = 8,

        /// <summary>
        /// Log to file, standard output, and form.
        /// </summary>
        All = File | Output | Form
    }
}