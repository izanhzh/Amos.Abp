using System.Collections.Generic;
using System.Reflection;

namespace Amos.Abp.TempTable
{
    /// <summary>
    /// Provides entity members to work with.
    /// </summary>
    public interface IEntityMembersProvider
    {
        /// <summary>
        /// Gets members to work with.
        /// </summary>
        /// <returns>A collection <see cref="MemberInfo"/>.</returns>
        IReadOnlyList<MemberInfo> GetMembers();
    }
}
