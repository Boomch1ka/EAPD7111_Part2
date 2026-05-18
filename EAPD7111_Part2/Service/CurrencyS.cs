using System.Text.Json;

namespace EAPD7111_Part2.Service
{
    // ✅ This should be a plain service class, not a Controller
    public class CurrencyResult
    {
        public decimal Value { get; set; }
        public bool UsedFallback { get; set; }
    }

    public class CurrencyS
    {
        private readonly HttpClient _httpClient;

        public CurrencyS(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyResult> ConvertUsdToZar(decimal usdAmount)
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
                var data = JsonDocument.Parse(response);

                var rate = data.RootElement.GetProperty("rates").GetProperty("ZAR").GetDecimal();

                return new CurrencyResult
                {
                    Value = usdAmount * rate,
                    UsedFallback = false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Currency API failed: {ex.Message}");

                decimal fallbackRate = 18m;
                return new CurrencyResult
                {
                    Value = usdAmount * fallbackRate,
                    UsedFallback = true
                };
            }
        }
    }

}
