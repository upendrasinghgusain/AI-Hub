using Microsoft.AspNetCore.Mvc;
using System;

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
            if (new Random().NextDouble() < 0.5)
            {
                // Simulate service unavailability like HttpClient
                throw new HttpRequestException("No connection could be made because the target machine actively refused it.");
            }

            _logger.LogInformation("Successfully created quote: {@Quote}", quote);

            return Ok(quote);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create quote for customer: {CustomerName}", quote.CustomerName);
            return StatusCode(500, new { error = "Quote creation failed", details = ex.Message });
        }
    }
}

public class Quote
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Premium { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
