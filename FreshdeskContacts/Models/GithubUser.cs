using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshdeskContacts.Models
{
    public class GithubUser
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? HtmlUrl { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }
    }
}
