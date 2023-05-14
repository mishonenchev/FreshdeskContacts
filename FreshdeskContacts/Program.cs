using System;
using AutoMapper;
using FreshdeskContacts.Clients;
using FreshdeskContacts.Database;
using FreshdeskContacts.Models;
using FreshdeskContacts.Repositories;
using FreshdeskContacts.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Channels;
using FreshdeskContacts.Models.Dtos;

namespace FreshdeskContacts
{
    internal class Program
    {
        private static async Task Main()
        {
            var services = new ServiceCollection();
            var serviceProvider = new Startup().ConfigureServices(services);

            var githubClient = serviceProvider.GetService<GithubClient>();
            var freshdeskClient = serviceProvider.GetService<FreshdeskClient>();
            var githubUserService = serviceProvider.GetService<IGithubUserService>();
            var mapper = serviceProvider.GetService<IMapper>();
            
            bool shouldQuit = false;

            while (!shouldQuit)
            {
                Console.WriteLine("[A] Add users to freshdesk, [L] List freshdesk contacts, [E] Edit contact [D] Delete contact, [Q] Quit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "A":
                        Console.Write("Input github username to save to contact: ");
                        var githubUserLogin = Console.ReadLine();
                        var githubUserDto = await githubClient.GetGithubUser(githubUserLogin);
                        if (githubUserDto == null)
                        {
                            break;
                        }

                        var githubUser = mapper.Map<GithubUser>(githubUserDto);
                        try
                        {
                            githubUserService.AddUser(githubUser);
                            Console.WriteLine("Saved github user to database.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        
                        var freshdeskUser = await freshdeskClient.SaveContact(githubUserDto);
                        if (freshdeskUser != null)
                        {
                            Console.WriteLine("Contact saved to freshdesk successfully!");
                        }
                        break;

                    case "L":
                        Console.WriteLine("Now we try to show freshdesk contacts...");
                        var contacts = await freshdeskClient.GetAllContacts();

                        contacts?.ForEach(x =>
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Id: {x.FreshdeskId}");
                            Console.WriteLine($"Name: {x.Name}");
                            Console.WriteLine($"Email: {x.Email}");
                            Console.WriteLine($"Description: {x.Description}");
                            Console.WriteLine($"GithubURL: {x.HtmlUrl}");
                            Console.WriteLine();
                        });
                        break;

                    case "E":
                        Console.Write("Enter Freshdesk ID of contact to edit: ");
                        var freshdeskId = Console.ReadLine();
                        
                        var contactToEdit = await freshdeskClient.GetContact(freshdeskId);
                        if (contactToEdit == null)
                        {
                            break;
                        }

                        var oldGithubUser = githubUserService.GetUserByLogin(contactToEdit.Login);

                        Console.WriteLine($"Editing contact {contactToEdit.Name} ({contactToEdit.Email})");
                        
                        Console.Write("Enter new name (or leave blank to keep current): ");
                        var newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                        {
                            contactToEdit.Name = newName;
                        }

                        Console.Write("Enter new email (or leave blank to keep current): ");
                        var newEmail = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newEmail))
                        {
                            contactToEdit.Email = newEmail;
                        }

                        Console.Write("Enter new description (or leave blank to keep current): ");
                        var newDescription = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newDescription))
                        {
                            contactToEdit.Description = newDescription;
                        }

                        Console.Write("Enter new GitHub URL (or leave blank to keep current): ");
                        var newGithubUrl = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newGithubUrl))
                        {
                            contactToEdit.HtmlUrl = newGithubUrl;
                        }
                        
                        await freshdeskClient.EditContact(contactToEdit);

                        var editedGithubUser = mapper.Map<GithubUser>(contactToEdit);
                        if (oldGithubUser != null)
                            githubUserService.UpdateUser(oldGithubUser.Id, editedGithubUser);

                        else
                            githubUserService.AddUser(editedGithubUser);
                        
                        break;

                    case "D":
                        Console.Write("Input Freshdesk ID of the contact to delete: ");
                        var contactId = Console.ReadLine();
                        await freshdeskClient.DeleteContact(contactId);
                        break;

                    case "Q":
                        shouldQuit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }
    }
}

