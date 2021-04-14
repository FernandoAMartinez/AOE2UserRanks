using AOE2DEStats.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AOE2DEStats.Services
{
    public interface ILeaderboardService
    {
        Task<LeaderboardResult> GetLeaderboardAsync(int leaderboard_id, int? start, int? count, string? search, string? steam_id, string? profile_id);

    }
    public class LeaderboardServices : ILeaderboardService
    {
        private string Game = "aoe2de";

        private readonly IConfiguration _config;

        public LeaderboardServices(IConfiguration config)
        {
            _config = config;
        }
        public async Task<LeaderboardResult> GetLeaderboardAsync(int leaderboard_id, int? start, int? count, string? search, string? steam_id, string? profile_id)
        {
            /*
             game (Required) Game (Age of Empires 2:Definitive Edition=aoe2de)
             leaderboard_id (Required) Leaderboard ID (Unranked=0, 1v1 Deathmatch=1, Team Deathmatch=2, 1v1 Random Map=3, Team Random Map=4)
             start (Required) Starting rank (Ignored if search, steam_id, or profile_id are defined)
             count (Required) Number of leaderboard entries to get (Must be 10000 or less))
             search (Optional) Name Search
             steam_id (Optional) steamID64 (ex: 76561199003184910)
             profile_id (Optional) Profile ID (ex: 459658)
            */
            string jsonResult;
            string endpoint = $"{_config["LeaderboardAPI"]}?game={Game}&leaderboard_id={leaderboard_id}&start={start}&count={count}";

            if (search != null) { endpoint += string.Format($"&search={search}"); }
            if (steam_id != null) { endpoint += string.Format($"&steam_id={steam_id}"); }
            if (profile_id != null) { endpoint  += string.Format($"&profile_id={profile_id}"); }

            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<LeaderboardResult>(jsonResult);
            }
        }
    }
}
