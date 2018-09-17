using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.Entities
{
    public class EmailTemplate
    {
        public EmailTemplate()
        {
        }

        public EmailTemplate(int id, string subject, string body)
        {
            Id = id;
            Body = body;
            Subject = subject;
        }

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR(50)"), Required]
        public string TemplateName { get; set; }

        [Column(TypeName = "NVARCHAR(50)"), Required]
        public string Subject { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)"), Required]
        public string Body { get; set; }
    }
}