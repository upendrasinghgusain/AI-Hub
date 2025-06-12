using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly ILogger<QuotesController> _logger;

    public QuotesController(ILogger<QuotesController> logger)
    {
        _logger = logger;
    }

    [HttpPost("create")]
    public IActionResult CreateQuote([FromBody] Quote quote)
    {
        _logger.LogInformation("Received quote request for customer: {CustomerName}", quote.CustomerName);

        try
        {
            // Simulate a random failure
            if (quote.CustomerName.ToLower().Contains("fail"))
            {
                throw new InvalidOperationException("Simulated quote failure due to invalid customer name.");
            }

            quote.Id = new Random().Next(1000, 9999);
            quote.Premium = CalculatePremium(quote.CustomerName);

            _logger.LogInformation("Successfully created quote: {@Quote}", quote);

            return Ok(quote);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create quote for customer: {CustomerName}", quote.CustomerName);
            return StatusCode(500, new { error = "Quote creation failed", details = ex.Message });
        }
    }

    private decimal CalculatePremium(string customerName)
    {
        // Simulate logic
        return customerName.Length * 1.5m;
    }
}

public class Quote
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Premium { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
