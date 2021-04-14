using AOE2DEStats.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AOE2DEStats.Services
{
    public interface IMatchServices
    {
        Task<Match[]> GetMatchesSinceAsync(int count, int? since);
        Task<Match> GetMatchDetailAsync(string uuid, string match_id);

    }
    public class MatchServices : IMatchServices
    {
        private readonly string game = "aoe2de";

        private readonly IConfiguration _config;

        public MatchServices(IConfiguration config)
        {
            _config = config;
        }
        public async Task<Match[]> GetMatchesSinceAsync(int count, int? since)
        {
            /*
             game Game (Age of Empires 2:Definitive Edition=aoe2de)
             count (Required) Number of matches to get (Must be 1000 or less))
             since (Optional) Only show matches starting after timestamp (epoch)
            */
            string jsonResult;
            string endpoint = $"{_config["MatchesAPI"]}?game={game}&count={count}";

            if (since != null) { endpoint += string.Format($"&since={ since }"); }
            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<Match[]>(jsonResult);
            }
        }

        public async Task<Match> GetMatchDetailAsync(string uuid, string match_id)
        {
            /*
             game (Required) Game (Age of Empires 2:Definitive Edition=aoe2de)
             uuid (uuid or match_id Required) Match UUID
             match_id (uuid or match_id Required) Match ID
            */

            string jsonResult;
            string endpoint = $"{_config["MatchAPI"]}?game={game}&uuid={uuid}&match_id={match_id}";

            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<Match>(jsonResult);
            }
        }
    }
}
