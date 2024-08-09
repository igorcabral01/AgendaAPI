using System.ComponentModel.DataAnnotations;

namespace AgendaAPI.Data.Dtos;

public class UpdateContatosDto
{
    [Required(ErrorMessage = "O seu nome é obrigatório! Preencher corretamente.")]
    [StringLength(100)]
    public  string Nome { get; set; }

    [Required(ErrorMessage = "O seu email é obrigatório! Preencher corretamente.")]
    [StringLength(100)]
    public  string Email { get; set; }

    [Required(ErrorMessage = "O seu número de telefone é obrigatório! Preencher corretamente.")]
    public  int Telefone { get; set; }
}
