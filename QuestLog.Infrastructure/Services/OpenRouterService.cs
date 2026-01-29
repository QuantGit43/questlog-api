using System.ClientModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using QuestLog.Application.Interfaces;

namespace QuestLog.Infrastructure.Services
{
    public class OpenRouterService : IAiService
    {
        private readonly ChatClient _chatClient;
        private readonly ILogger<OpenRouterService> _logger;

        public OpenRouterService(IConfiguration config, ILogger<OpenRouterService> logger)
        {
            _logger = logger;
            
            var apiKey = config["AiSettings:ApiKey"];
            var model = config["AiSettings:Model"]; 
            
            _logger.LogInformation("[OpenRouterService] Initializing with Model: {Model}, ApiKey present: {HasKey}", 
                model, !string.IsNullOrWhiteSpace(apiKey));
            
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("AiSettings:ApiKey is missing in configuration!");
            
            if (string.IsNullOrWhiteSpace(model))
                throw new InvalidOperationException("AiSettings:Model is missing in configuration!");
            
            // OpenRouter вимагає специфічний Base URL
            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1/")
            };
            
            _chatClient = new ChatClient(model, new ApiKeyCredential(apiKey), options);
        }

        public async Task<string> GetAnswerAsync(string prompt)
        {
            _logger.LogInformation("[OpenRouterService] Sending prompt to AI...");
            
            var options = new ChatCompletionOptions
            {
                Temperature = 0.7f
            };

            try
            {
                ChatCompletion completion = await _chatClient.CompleteChatAsync(
                    [new UserChatMessage(prompt)], 
                    options);
                
                var response = completion.Content[0].Text;
                _logger.LogInformation("[OpenRouterService] Received response: {Response}", response);
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OpenRouterService] API call failed: {Message}", ex.Message);
                throw; // Перекидаємо далі, щоб AiTaskAnalyser міг обробити
            }
        }
    }
}