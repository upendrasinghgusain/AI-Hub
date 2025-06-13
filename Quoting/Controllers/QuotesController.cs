using Microsoft.AspNetCore.Mvc;
using Quoting;
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
    public async Task<IActionResult> CreateQuote([FromBody] Quote quote)
    {
        _logger.LogInformation("Received quote request for customer: {CustomerName}", quote.CustomerName);

        try
        {
            await new PricingService().GetPriceAsync();
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
