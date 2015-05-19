using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using MMPlus.Service.Interface;
using MMPlus.Service.Model;
using MMPlus.Shared.Interface;

namespace MMPlus.Service.Services
{
    [ServiceContract(Namespace = "https://carterjk.com/mmplus/2015/05")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class GuildService : IGuildService
    {
        /// <summary>
        ///     The storage data store for all Guild tables.
        /// </summary>
        private readonly IStorageRepository _repository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GuildService" /> class.
        /// </summary>
        /// <param name="repository">The storage data store for all Guild tables.</param>
        public GuildService(IStorageRepository repository)
        {
            _repository = repository;
        }

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
            var results = new List<IEsoGuild>();
            foreach (string guildName in guildNames)
            {
                EsoGuild guild = _repository.Find<EsoGuild>(string.Empty, guildName).FirstOrDefault();
                if (guild != null)
                {
                    results.Add(guild);
                }
            }
            return results;
        }
    }
}