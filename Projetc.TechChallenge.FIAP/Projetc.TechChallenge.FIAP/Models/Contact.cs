using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Projetc.TechChallenge.FIAP.Models
{
    public class Contact
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(9, 99)]
        public string DDD { get; set; }
    }

    public enum DDDRegion 
    {
        [Display(Name = "São Paulo")]
        SP = 11,
        [Display(Name = "Rio de Janeiro")]
        RJ = 21,
        [Display(Name = "Minas Gerais")]
        MG = 31
        // Adicione mais regiões conforme necessário
    }

}
