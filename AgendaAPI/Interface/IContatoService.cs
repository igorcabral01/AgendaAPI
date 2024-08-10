using AgendaAPI.Data.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AgendaAPI.Services
{
    public interface IContatoService
    {
        IActionResult AdicionaContato(CreatContatosDto contatoDto);
        IEnumerable<ReadContatosDto> ContatosCadastrados(int skip = 0, int take = 50);
        IActionResult BuscarContatoId(int id);
        IActionResult AtualizarContato(int id, UpdateContatosDto contatoDtos);
        IActionResult AtualizarContatoPatch(int id, JsonPatchDocument<UpdateContatosDto> patch);
        IActionResult DeletaContato(int id);
    }
}
