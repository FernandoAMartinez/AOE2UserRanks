using AOE2DEStats.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AOE2DEStats.Services
{
    public interface IStringServices
    {
        Task<GeneralData> GetStringsAsync(string? lang);
    }
    public class StringServices : IStringServices
    {
        private readonly string game = "aoe2de";

        private readonly IConfiguration _config;
        public StringServices(IConfiguration config)
        {
            _config = config;
        }

        public async Task<GeneralData> GetStringsAsync(string? lang)
        {
            /*
             game (Required) Game (Age of Empires 2:HD=aoe2hd, Age of Empires 2:Definitive Edition=aoe2de)
             language (Optional, defaults to en) Language (en, de, el, es, es-MX, fr, hi, it, ja, ko, ms, nl, pt, ru, tr, vi, zh, zh-TW)
            */
            //string api_string = configuration.GetSection("Endpoints").GetSection("Strings").GetSection("Strings").Value;
            string jsonResult;
            string endpoint = $"{_config["StringAPI"]}?game={game}";
            if (lang != null) { endpoint += string.Format($"&language={lang}"); };

            using (var Http = new HttpClient())
            {
                jsonResult = await Http.GetStringAsync(endpoint);
                return JsonConvert.DeserializeObject<GeneralData>(jsonResult);
            }
        }
    }
}
