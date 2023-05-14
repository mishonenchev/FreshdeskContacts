using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FreshdeskContacts.Models.Dtos;
using AutoMapper;
using System.Net;
using FreshdeskContacts.Models;

namespace FreshdeskContacts.Clients
{
    public class FreshdeskClient
    {
        private readonly IMapper _mapper;
        private readonly string _domain;
        private readonly HttpClient _httpClient;
        private readonly string? _freshdeskToken = ConfigurationManager.AppSettings["FreshdeskToken"];

        public FreshdeskClient(IMapper mapper, string domain)
        {
            _domain = domain;
            _mapper = mapper;
            _httpClient = new HttpClient();
            if (string.IsNullOrEmpty(_freshdeskToken))
            { 
                throw new ArgumentException("Freshdesk token is null or empty");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_freshdeskToken}:X")));
        }

        public async Task<FreshdeskContactDTO?> GetContact(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://{_domain}.freshdesk.com/api/v2/contacts/{id}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception($"Account with id {id} not found.");
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to get contact with ID {id} from Freshdesk API. Status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<FreshdeskContactDTO>(content);
                return contact;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting contact: {e.Message}");
                return null;
            }
        }

        public async Task<FreshdeskContactDTO?> SaveContact(GithubUserDTO githubUser)
        {
            try
            {
                var contact = _mapper.Map<FreshdeskContactDTO>(githubUser);
                var json = JsonConvert.SerializeObject(contact);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://{_domain}.freshdesk.com/api/v2/contacts", content);

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Contact already exists.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to save contact to Freshdesk API. Status code: {response.StatusCode}");
                }

                var savedContactJson = await response.Content.ReadAsStringAsync();
                var savedContact = JsonConvert.DeserializeObject<FreshdeskContactDTO>(savedContactJson);
                return savedContact;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving contact: {e.Message}");
                return null;
            }
        }

        public async Task EditContact(FreshdeskContactDTO contactToEdit)
        {
            try
            {
                var json = JsonConvert.SerializeObject(contactToEdit);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"https://{_domain}.freshdesk.com/api/v2/contacts/{contactToEdit.FreshdeskId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to save contact to Freshdesk API. Status code: {response.StatusCode}");
                }

                Console.WriteLine($"Contact with id {contactToEdit.FreshdeskId} has been edited.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error editing contact: {e.Message}");
            }
        }

        public async Task DeleteContact(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://{_domain}.freshdesk.com/api/v2/contacts/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to delete contact with ID {id} from Freshdesk API. Status code: {response.StatusCode}");
                }

                Console.WriteLine($"Contact with id {id} has been deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error deleting contact: {e.Message}");
            }
        }

        public async Task<List<FreshdeskContactDTO>?> GetAllContacts()
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://{_domain}.freshdesk.com/api/v2/contacts/");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to get contacts from Freshdesk API. Status code: {response.StatusCode}");
                }
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<FreshdeskContactDTO>>(content);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting contacts: {e.Message}");
                return null;
            }
        }

        
    }
}
