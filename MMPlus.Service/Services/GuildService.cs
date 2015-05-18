using System;
using System.Collections.Generic;
using System.ServiceModel;
using MMPlus.Shared.Interface;

namespace MMPlus.Service.Services
{
    [ServiceContract(Namespace = "https://carterjk.com/mmplus/2015/05")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class GuildService : IGuildService
    {
        /// <summary>
        ///     Gets a list of guild meta data records for a given list of guild names.
        /// </summary>
        /// <param name="guildNames">The guild names to retreive meta data records for.</param>
        /// <returns>
        ///     A list of guild entities.
        /// </returns>
        [OperationContract(AsyncPattern = true)]
        public IEnumerable<IEsoGuild> Get(IEnumerable<string> guildNames)
        {
            throw new NotImplementedException();
        }
    }
}