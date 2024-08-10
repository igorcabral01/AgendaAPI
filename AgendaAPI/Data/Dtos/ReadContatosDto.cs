using System.ComponentModel.DataAnnotations;

namespace AgendaAPI.Data.Dtos
{
    public class ReadContatosDto
    {
        public int Id { get; set; }
        public  required string Nome { get; set; }
        public required string Email { get; set; }
        public int Telefone { get; set; }
        public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
       
    }
}
