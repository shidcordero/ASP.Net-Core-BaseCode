using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.Entities
{
    public class EmailTemplate
    {
        [Key]
        public int TemplateId { get; set; }

        [Column(TypeName = "NVARCHAR(50)"), Required]
        public string TemplateName { get; set; }

        [Column(TypeName = "NVARCHAR(50)"), Required]
        public string Subject { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)"), Required]
        public string Body { get; set; }
    }
}