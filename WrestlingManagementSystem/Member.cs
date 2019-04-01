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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

        public string School { get; set; }
        public int YearsOfExperience { get; set; } = 0;
    }
}
