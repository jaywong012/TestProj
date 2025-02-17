using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace NewProject.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly IRedisCacheService _redis;

        public MessageController(IRedisCacheService redis)
        {
            _redis = redis;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateOtp([FromBody] GenerateOtpRequest request)
        {
            // Generate a random 6-digit OTP
            var otp = GenerateRandomOtp();

            // Create a key using a prefix and the person's identifier
            string key = $"OTP_{request.PersonIdentifier}";

            // Store the OTP in Redis with a TTL (for example, 5 minutes)
            await _redis.SetAsync(key, otp, TimeSpan.FromMinutes(15));

            // In a real-world application, send the OTP via SMS or email here

            return Ok(new { Message = $"OTP generated and sent.{otp}" });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            string key = $"OTP_{request.PersonIdentifier}";

            // Retrieve the OTP from Redis
            var storedOtp = await _redis.GetAsync<string>(key);

            if (storedOtp == null)
            {
                return BadRequest("Person not found");
            }

            if (storedOtp != request.Otp)
            {
                return BadRequest("Invalid OTP.");
            }

            // Optionally: Remove the OTP after successful verification if your cache service supports removal.
            // await _cacheService.RemoveAsync(key);

            return Ok("OTP verified successfully.");
        }

        // Helper method to generate a 6-digit OTP
        private string GenerateRandomOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }

    public class GenerateOtpRequest
    {
        // This could be an email, phone number, or user ID that uniquely identifies the person.
        public string PersonIdentifier { get; set; }
    }

    public class VerifyOtpRequest
    {
        public string PersonIdentifier { get; set; }
        public string Otp { get; set; }
    }
}
