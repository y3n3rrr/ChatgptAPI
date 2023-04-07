using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;

namespace ChatgptAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public WeatherForecastController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;

            //optional
            _openAIService.SetDefaultModelId(Models.TextDavinciV3);
        }

        [HttpGet]
        [Route("/api/say")]
        public async Task<IActionResult> SaySomething(string message)
        {
            CompletionCreateRequest request = new CompletionCreateRequest();
            request.Prompt = message;
            request.MaxTokens = 256;
            request.N = 2;
            request.Model = Models.TextDavinciV3;

            var complations = await _openAIService.Completions.CreateCompletion(request);

            string response = String.Join("\n", complations.Choices.Select(s => s.Text).ToList());
            return Ok(response);
        }

        [HttpGet]
        [Route("/api/getimage")]
        public async Task<IActionResult> getimage(string image)
        {
            var imageResult = await _openAIService.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = image,
                N = 10,
                Size = StaticValues.ImageStatics.Size.Size256,
                ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
                User = "TestUser"
            });

            if (imageResult.Successful)
            {
                return Ok(string.Join("\n", imageResult.Results.Select(r => r.Url)));
            }

            return Ok();
        }
    }
}