using System.Text.Json;
using System.Text;

namespace thrucommunity.Services
{
    public class ReplayParserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReplayParserService> _logger;
        private readonly string _parserUrl;

        public ReplayParserService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ReplayParserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _parserUrl = configuration["ReplayParser:Url"] ?? "http://localhost:5000";
        }

        public async Task<ReplayParseResult?> ParseReplayAsync(Stream fileStream, string fileName)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(fileStream);
                content.Add(streamContent, "replay_file", fileName);

                var response = await _httpClient.PostAsync(
                    $"{_parserUrl}/parse_replay",
                    content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        "Parser error (HTTP {StatusCode}): {Error}",
                        response.StatusCode,
                        responseContent);
                    return null;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<ReplayParseResult>(responseContent, options);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to connect to replay parser service");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during replay parsing");
                return null;
            }
        }
    }

    // DTO для десериализации ответа от микросервиса
    public class ReplayParseResult
    {
        public string? Game { get; set; }
        public string? Shot { get; set; }
        public long? Score { get; set; }
        public int? Difficulty { get; set; }
        public string? Name { get; set; }
        public DateTime? Timestamp { get; set; }
        public double? Slowdown { get; set; }
        public int? ReplayType { get; set; }
        public string? Route { get; set; }
        public int? SpellCardId { get; set; }
        public int? SceneGameLevel { get; set; }
        public int? SceneGameScene { get; set; }
        public List<string>? Equipment { get; set; }
        public List<ReplayStageData>? Stages { get; set; }
    }

    public class ReplayStageData
    {
        public int? Stage { get; set; }
        public long? Score { get; set; }
        public int? Piv { get; set; }
        public int? Graze { get; set; }
        public int? PointItems { get; set; }
        public int? Power { get; set; }
        public int? Lives { get; set; }
        public int? LifePieces { get; set; }
        public int? Bombs { get; set; }
        public int? BombPieces { get; set; }
        public int? Th06Rank { get; set; }
        public int? Th07Cherry { get; set; }
        public int? Th07Cherrymax { get; set; }
        public bool? Th09P1Cpu { get; set; }
        public bool? Th09P2Cpu { get; set; }
        public string? Th09P2Shot { get; set; }
        public int? Th09P2Score { get; set; }
        public int? Th128Motivation { get; set; }
        public int? Th128PerfectFreeze { get; set; }
        public double? Th128FrozenArea { get; set; }
        public int? Th13Trance { get; set; }
        public int? Extends { get; set; }
        public int? Th16SeasonPower { get; set; }
    }
}
