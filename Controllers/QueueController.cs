using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class QueueController : Controller
{
    private readonly QueueService _queueService;

    public QueueController(QueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return BadRequest("Message cannot be empty.");
        }

        await _queueService.SendMessageAsync(message);

        return Ok("Message sent successfully.");
    }
}
