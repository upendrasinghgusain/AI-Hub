using System.Net.Http;

namespace Quoting
{
    public class PricingService
    {

        public async Task<HttpResponseMessage> GetPriceAsync()
        {
            // This is a timeout
            throw new HttpRequestException("Pricing service request timed out.");
        }
    }
}
