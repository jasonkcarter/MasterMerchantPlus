using System.Collections.Generic;

namespace MMPlus.Shared.Interface
{
    /// <summary>
    ///     Supports server-side operations related to Elder Scrolls Online guilds.
    /// </summary>
    public interface IGuildService
    {
        /// <summary>
        ///     Gets a list of guild meta data records for a given list of guild names.
        /// </summary>
        /// <param name="guildNames">The guild names to retreive meta data records for.</param>
        /// <returns>A list of guild entities.</returns>
        IEnumerable<IEsoGuild> Get(IEnumerable<string> guildNames);
    }
}