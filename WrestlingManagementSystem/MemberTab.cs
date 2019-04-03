/*
 * Author: Shon Verch
 * File Name: MemberTab.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/02/2019
 * Modified Date: 04/02/2019
 * Description: A tab for a member-type display.
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// A tab for a member-type display
    /// </summary>
    public class MemberTab
    {
        /// <summary>
        /// Header text of this <see cref="MemberTab"/>.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// A collection of the data in this <see cref="MemberTab"/>.
        /// </summary>
        public ObservableCollection<Member> Data { get; }

        /// <summary>
        /// Initializes a new <see cref="MemberTab"/>.
        /// </summary>
        /// <param name="header">The header text of this <see cref="MemberTab"/>.</param>
        /// <param name="data">A collection of the data in this <see cref="MemberTab"/>.</param>
        public MemberTab(string header, ObservableCollection<Member> data)
        {
            Header = header;
            Data = data;
        }
    }
}
