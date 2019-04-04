/*
 * Author: Shon Verch
 * File Name: WrestlerStatus.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 03/27/2019
 * Description: The status of a wrestler.
 */

namespace WrestlingManagementSystem
{
    /// <summary>
    /// The status of a <see cref="Wrestler"/>.
    /// </summary>
    public enum WrestlerStatus
    {
        Active,
        Injured,
        Quit
    }
}
