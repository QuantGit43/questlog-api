using System.ClientModel;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using QuestLog.Application.Interfaces; // Потрібно для ApiKeyCredential

namespace QuestLog.Infrastructure.Services
{
    public class OpenRouterService : IAiService
    {
        private readonly ChatClient _chatClient;

        public OpenRouterService(IConfiguration config)
        {
            var apiKey = config["AiSettings:ApiKey"];
            var model = config["AiSettings:Model"]; 
            
            // OpenRouter вимагає специфічний Base URL
            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            };
            
            _chatClient = new ChatClient(model, new ApiKeyCredential(apiKey), options);
        }

        public async Task<string> GetAnswerAsync(string prompt)
        {
            var options = new ChatCompletionOptions
            {
                Temperature = 0.7f // Можна налаштувати "креативність модельки" - Чим менше значення, тим менше креативність
            };

            ChatCompletion completion = await _chatClient.CompleteChatAsync(
                [new UserChatMessage(prompt)], 
                options);
            
            return completion.Content[0].Text;
        }
    }
    
    
}