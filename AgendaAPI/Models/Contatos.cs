using System.ComponentModel.DataAnnotations;

namespace AgendaAPI.Models
{
    public class Contatos
    {
        [Key]
        [Required]
        public  int Id { get;  set; }

        [Required(ErrorMessage = "O seu nome é obrigatório! Preencher corretamente.")]
        [MaxLength(100)]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O seu email é obrigatório! Preencher corretamente.")]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O seu número de telefone é obrigatório! Preencher corretamente.")]
        public int Telefone { get; set; }
        
    }
}

