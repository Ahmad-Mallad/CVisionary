 using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using Microsoft.SemanticKernel;
using NuGet.Protocol;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CVisionary.Services
{
    public class CvParserService : ICVParser
    {
        private readonly Kernel _Kernel;

        public CvParserService(Kernel kernel)
        {
            _Kernel = kernel;
        }

        public async Task<Resume> ParseCvAsync(string rawText)
        {
            string prompt = @"
You are an intelligent resume parser and enhancer.
Your job is to take a user's raw, unstructured CV or resume input in free text (in any language) and return a clean, structured, and enhanced version as a JSON object.
Translate input to English if needed. Output must be English.
if user enters simple summary make it proffessional 
Split mixed content into individual items (e.g., jobs, skills, certificates).
Enhance Summary & Experiences with keywords and clear, result-driven bullet points.
if user mentions skills in his summary and did not mention it in his skills add them to skills section 
Classify each skill as Technical, Soft, or Language.
Languages format: ""English"": ""Native"", others as ""Language"" - Level: ""Proficient""/""Native""/etc.
Certificates: If dates are missing, set StartDate and EndDate to null. Do not default to ""Present"" unless explicitly stated.
Dates: Format as ""yyyy-MM-dd"" or ""yyyy"" if full date not available.
Ongoing roles: If user says “present” or “currently”, set IsCurrent = true and EndDate = null.
 **For any required field (e.g., FirstName, LastName, Email,):**
- If not found in input, set its value to ""Not Provided"".
 For optional fields (e.g., GPA, SecondName, ThirdName, LinkedInLink, GithubLink, FacebookLink, InstagramLink, Address, DateOfBirth), set to `null` if not found.
 Do NOT hallucinate or invent information. Only extract what is clearly stated or strongly implied.
 Output *every required field*, even if set to ""Not Provided"".


Your output must strictly follow this structure (return only valid JSON):

{
  ""PersonalInfo"": {
    ""FirstName"": ""string"",      // Required
    ""SecondName"": null,         // Optional
    ""ThirdName"": null,          // Optional
    ""LastName"": ""string"",       // Required
    ""Email"": ""string"",          // Optional
    ""PhoneNumber"": ""string"",    // Optional
    ""LinkedInLink"": null,
    ""GithubLink"": null,
    ""FacebookLink"": null,
    ""InstagramLink"": null,
    ""Address"": null,
    ""DateOfBirth"": null,
    ""Title"": ""string"",          // Optional
    ""Summary"": ""string""         // Optional
  },
  ""Educations"": [
    {
      ""CollegeName"": ""string"",
      ""DegreeType"": ""string"",
      ""MajorName"": ""string"",
      ""StartDate"": ""2015"",
      ""EndDate"": ""2019"",
      ""GPA"": 3.8    // Optional, may be null
    }
  ],
  ""Experiences"": [
    {
      ""Title"": ""string"",
      ""CompanyName"": ""string"",
      ""StartDate"": ""2019"",
      ""EndDate"": ""2023"",
      ""IsCurrent"": false,
      ""Description"": ""string""
    }
  ],
  ""Skills"": [
    {
      ""SkillName"": ""string"",
      ""SkillType"": ""Technical""
    }
  ],
  ""Languages"": [
    {
      ""LanguageName"": ""string"",
      ""Level"": ""string""
    }
  ],
  ""Certificates"": [
    {
      ""ProviderName"": ""string"",
      ""TopicName"": ""string"",
      ""StartDate"": null,  // Optional, may be null
      ""EndDate"": null,  // Optional, may be null
      ""GPA"": null
    }
  ],
  ""Errors"": []
}

---

Now extract structured JSON from the following text:

CV Text:
{{$input}}

---

JSON:
";



            var extractFunction = _Kernel.CreateFunctionFromPrompt(prompt);

            var result = await _Kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = rawText
            });

            var json = result.ToString();

            // Parse JSON manually
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var resume = new Resume();

            // 🧠 Manually bind base class (PersonalInfo) properties
            if (root.TryGetProperty("PersonalInfo", out var personalInfo))
            {
                resume.FirstName = personalInfo.GetProperty("FirstName").GetString();
                resume.SecondName = personalInfo.GetProperty("SecondName").GetString();
                resume.ThirdName = personalInfo.GetProperty("ThirdName").GetString();
                resume.LastName = personalInfo.GetProperty("LastName").GetString();
                resume.Email = personalInfo.GetProperty("Email").GetString();
                resume.PhoneNumber = personalInfo.GetProperty("PhoneNumber").GetString();
                resume.LinkedInLink = personalInfo.GetProperty("LinkedInLink").GetString();
                resume.GithubLink = personalInfo.GetProperty("GithubLink").GetString();
                resume.FacebookLink = personalInfo.GetProperty("FacebookLink").GetString();
                resume.InstagramLink = personalInfo.GetProperty("InstagramLink").GetString();
                resume.Address = personalInfo.GetProperty("Address").GetString();
                resume.DateOfBirth = personalInfo.GetProperty("DateOfBirth").GetString();
                resume.Title = personalInfo.GetProperty("Title").GetString();
                resume.Summary = personalInfo.GetProperty("Summary").GetString();
            }

            // ✅ Deserialize the rest normally (lists)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            if (root.TryGetProperty("Educations", out var educationsElement))
            {
                resume.Educations = JsonSerializer.Deserialize<List<Education>>(educationsElement.GetRawText(), options);
            }

            if (root.TryGetProperty("Experiences", out var experiencesElement))
            {
                resume.Experiences = JsonSerializer.Deserialize<List<Experience>>(experiencesElement.GetRawText(), options);
            }

            if (root.TryGetProperty("Skills", out var skillsElement))
            {
                resume.Skills = JsonSerializer.Deserialize<List<Skill>>(skillsElement.GetRawText(), options);
            }

            if (root.TryGetProperty("Languages", out var languagesElement))
            {
                resume.Languages = JsonSerializer.Deserialize<List<Language>>(languagesElement.GetRawText(), options);
            }

            if (root.TryGetProperty("Certificates", out var certificatesElement))
            {
                resume.Certificates = JsonSerializer.Deserialize<List<Certificate>>(certificatesElement.GetRawText(), options);
            }



            return resume;
        }

    }
}
