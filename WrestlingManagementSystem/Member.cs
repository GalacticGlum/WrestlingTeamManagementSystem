/*
 * Author: Shon Verch
 * File Name: Member.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/27/2019
 * Modified Date: 03/27/2019
 * Description: DESCRIPTION
 */

namespace WrestlingManagementSystem
{
    public abstract class Member
    {
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public Gender Gender { get; protected set; }

        public string School { get; protected set; }
        public int YearsOfExperience { get; protected set; } = 0;
    }
}
