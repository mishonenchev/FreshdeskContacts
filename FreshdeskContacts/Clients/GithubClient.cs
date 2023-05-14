using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using AutoMapper;
using FreshdeskContacts.Models;
using FreshdeskContacts.Models.Dtos;
using FreshdeskContacts.Services;
using Newtonsoft.Json;

namespace FreshdeskContacts.Clients
{
    public class GithubClient
    {
        private readonly IMapper _mapper;
        private readonly IGithubUserService _githubUserService;
        private readonly string? _githubToken = ConfigurationManager.AppSettings["GithubToken"];
        private readonly HttpClient _httpClient;

        public GithubClient(IGithubUserService githubUserService, IMapper mapper)
        {
            _githubUserService = githubUserService;
            _mapper = mapper;

            if (string.IsNullOrEmpty(_githubToken))
            {
                throw new ArgumentException("GitHub token is null or empty");
            }
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _githubToken);
        }

        public async Task<GithubUserDTO?> GetGithubUser(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.github.com/users/{username}");
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"User {username} does not exist on GitHub.");
                    return null;
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to get user from GitHub API. Status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var userDto = JsonConvert.DeserializeObject<GithubUserDTO>(content);
                return userDto;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting user: {e.Message}");
                throw;
            }
        }
    }
}
