namespace BitrixAppCitiesSearch.Models
{
    public class CityRequest
    {
        public string City { get; set; } = string.Empty; // Предотвращаем null
        public string Country { get; set; } = string.Empty; // Предотвращаем null
    }
}