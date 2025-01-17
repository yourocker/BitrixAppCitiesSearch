using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BitrixAppCitiesSearch.Models;

namespace BitrixAppCitiesSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class BitrixController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public BitrixController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("update-territory")]
    public async Task<IActionResult> UpdateTerritory([FromBody] CityRequest request)
    {
        if (string.IsNullOrEmpty(request.Country) || string.IsNullOrEmpty(request.City))
            return BadRequest(new { Message = "City and Country are required!" });

        if (request.Country != "Россия") return Ok(new { Territory = "СНГ" });

        var territory = await GetFederalDistrictAsync(request.City);
        return Ok(new { Territory = territory });
    }

    private async Task<string> GetFederalDistrictAsync(string city)
    {
        var baseUrl = "https://nominatim.openstreetmap.org/search";
        var requestUrl = $"{baseUrl}?q={city},Россия&format=json&addressdetails=1";

        try
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BitrixApp");
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Парсим JSON-ответ
                using var doc = JsonDocument.Parse(responseBody);
                var elements = doc.RootElement.EnumerateArray();
                foreach (var element in elements)
                    if (element.TryGetProperty("display_name", out var displayName))
                    {
                        var name = displayName.GetString()!;

                        // Проверяем наличие федерального округа в "display_name"
                        if (name.Contains("Центральный федеральный округ"))
                            return "Центральный федеральный округ";
                        if (name.Contains("Северо-Западный федеральный округ"))
                            return "Северо-Западный федеральный округ";
                        if (name.Contains("Приволжский федеральный округ"))
                            return "Приволжский федеральный округ";
                        if (name.Contains("Сибирский федеральный округ"))
                            return "Сибирский федеральный округ";
                        if (name.Contains("Южный федеральный округ"))
                            return "Южный федеральный округ";
                        if (name.Contains("Уральский федеральный округ"))
                            return "Уральский федеральный округ";
                        if (name.Contains("Дальневосточный федеральный округ"))
                            return "Дальневосточный федеральный округ";
                        if (name.Contains("Северо-Кавказский федеральный округ"))
                            return "Северо-Кавказский федеральный округ";
                    }

                return "Неизвестный федеральный округ";
            }

            return "Ошибка API";
        }
        catch
        {
            return "Ошибка подключения";
        }
    }
}