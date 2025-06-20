// Services/PortfolioParserService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using Microsoft.SemanticKernel;

namespace CVisionary.Services
{
    /// <summary>
    /// Uses Semantic Kernel + Azure OpenAI to rewrite a portfolio bio.
    /// </summary>
    public class PortfolioParserService : IPortfolioParser
    {
        private readonly Kernel _kernel;

        public PortfolioParserService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<PortfolioInfoResult> ParsePortfolioPersonalInfoAsync(string personalInfoText)
        {
            var prompt = @"
            You are a smart portfolio assistant.
            Given the following freeform personal information (which may include name, email, phone, address, social links, and a personal bio), do the following:
            - Extract each field: FirstName, LastName, Email, PhoneNumber, LinkedInLink, GithubLink, FacebookLink, InstagramLink, Address, DateOfBirth.
            - Professionally rewrite the personal bio as EnhancedSummary (one paragraph, engaging, concise, and in the first person).
            - Return your result as valid JSON in this format:
            {
              ""FirstName"": ""..."",
              ""LastName"": ""..."",
              ""Email"": ""..."",
              ""PhoneNumber"": ""..."",
              ""LinkedInLink"": ""..."",
              ""GithubLink"": ""..."",
              ""FacebookLink"": ""..."",
              ""InstagramLink"": ""..."",
              ""Address"": ""..."",
              ""DateOfBirth"": ""..."",
              ""EnhancedSummary"": ""...""
            }

            Freeform Input:
            {{$personalInfoText}}

            ---
            JSON Output:
            ";
            var function = _kernel.CreateFunctionFromPrompt(prompt);

            var result = await _kernel.InvokeAsync(function, new()
            {
                ["personalInfoText"] = personalInfoText
            });

            var json = result.GetValue<string>();

            // Use System.Text.Json or Newtonsoft.Json to parse the result
            var info = System.Text.Json.JsonSerializer.Deserialize<PortfolioInfoResult>(json);
            return info;
        }

    }
}
