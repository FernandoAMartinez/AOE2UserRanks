using AOE2DEStats.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AOE2DEStats.Services
{
    public interface IPlayerServices
    {
        Task<Match> GetPlayersLastMatchAsync(string? steam_id, string? profile_id);
        Task<Match[]> GetMatchHistoryAsync(int start, int count, string? steam_id, string? profile_id, string[]? steam_ids, string[]? profile_ids);
        Task<RatingHistory> GetRatingHistoryAsync(int leaderboard_id, int start, int count, string? steam_id, string? profile_id);
        Task<PlayersOnline> GetPlayersOnlineAsync();
    }
    public class PlayerServices : IPlayerServices
    {
        private readonly string game = "aoe2de";

        private readonly IConfiguration _config;
        public PlayerServices(IConfiguration config)
        {
            _config = config;
        }
        public async Task<Match> GetPlayersLastMatchAsync(string? steam_id, string? profile_id)
        {
            /*
                game (Required) Game (Age of Empires 2:Definitive Edition=aoe2de)
                steam_id (steam_id or profile_id required) steamID64 (ex: 76561199003184910)
                profile_id (steam_id or profile_id required) Profile ID (ex: 459658) 
            */
            string jsonResult;

            string endpoint = $"{_config["PlayerLastMatchesAPI"]}?game={game}";
            if (steam_id == null && profile_id != null) { endpoint += string.Format($"&profile_id={ profile_id }"); }
            else if (profile_id == null && steam_id != null) { endpoint += string.Format($"&steam_id={ steam_id }"); }
            else if (steam_id != null && profile_id != null) { endpoint += string.Format($"&profile_id={ profile_id }"); }
            else return null;

            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<Match>(jsonResult);
            }
        }

        public async Task<Match[]> GetMatchHistoryAsync(int start, int count, string? steam_id, string? profile_id, string[]? steam_ids, string[]? profile_ids)
        {
            /*
                game (Required) Game (Age of Empires 2:Definitive Edition=aoe2de)
                start (Required) Starting match (0 is the most recent match)
                count (Required) Number of matches to get (Must be 1000 or less))
                steam_id (steam_id or profile_id required) steamID64 (ex: 76561199003184910)
                profile_id (steam_id or profile_id required) Profile ID (ex: 459658)
                steam_ids (steam_id or profile_id required) steamID64 (ex: 76561199003184910,76561198449406083)
                profile_ids (steam_id or profile_id required) Profile ID (ex: 459658,199325) 
            */

            string jsonResult;
            string endpoint = $"{_config["PlayerMatchesAPI"]}?game={game}&start={start}&count={count}";

            if (steam_id != null) { endpoint += string.Format($"&steam_id={ steam_id }"); }
            if (profile_id != null) { endpoint += string.Format($"&profile_id={ profile_id }"); }
            if (steam_ids != null) { endpoint += string.Format($"&profile_ids={ steam_ids }"); }
            if (profile_ids != null) { endpoint += string.Format($"&profile_ids={ profile_ids }"); }

            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<Match[]>(jsonResult);
            }
        }

        public async Task<RatingHistory> GetRatingHistoryAsync(int leaderboard_id, int start, int count, string? steam_id, string? profile_id)
        {
            /*
                game (Required) Game (Age of Empires 2:Definitive Edition=aoe2de)
                leaderboard_id (Required) Leaderboard ID (Unranked=0, 1v1 Deathmatch=1, Team Deathmatch=2, 1v1 Random Map=3, Team Random Map=4)
                start (Required) Starting match (0 is the most recent match)
                count (Required) Number of matches to get (Must be 10000 or less))
                steam_id (steam_id or profile_id required) steamID64 (ex: 76561199003184910)
                profile_id (steam_id or profile_id required) Profile ID (ex: 459658)
            */

            string jsonResult;
            string endpoint = $"{_config["PlayerRatingHistoryAPI"]}?game={game}&leaderboard_id={leaderboard_id}&start={start}&count={count}";

            if (steam_id == null && profile_id != null) { endpoint += string.Format($"&profile_id={ profile_id }"); }
            else if (profile_id == null && steam_id != null) { endpoint += string.Format($"&steam_id={ steam_id }"); }
            else if (steam_id != null && profile_id != null) { endpoint += string.Format($"&profile_id={ profile_id }"); }

            else return null;

            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<RatingHistory>(jsonResult);
            }
        }

        public async Task<PlayersOnline> GetPlayersOnlineAsync()
        {
            string jsonResult;
            string endpoint = $"{_config["PlayersOnlineAPI"]}?game={game}";

            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<PlayersOnline>(jsonResult);
            }
        }
    }
}
