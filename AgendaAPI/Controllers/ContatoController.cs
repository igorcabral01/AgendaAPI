using AgendaAPI.Data.Dtos;
using AgendaAPI.Models;
using AgendaAPI.Services; 
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;

        public ContatoController(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        [HttpPost("/CadastrarContatos")]
        public IActionResult AdicionaContato([FromBody] CreatContatosDto contatoDto)
        {
            return _contatoService.AdicionaContato(contatoDto);
        }

        [HttpGet("/listarcontatos")]
        public IEnumerable<ReadContatosDto> ContatosCadastrados([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _contatoService.ContatosCadastrados(skip, take);
        }

        [HttpGet("/Buscar/{id}")]
        public IActionResult BuscarContatoId(int id)
        {
            return _contatoService.BuscarContatoId(id);
        }

        [HttpPut("/AtualizarContato/{id}")]
        public IActionResult AtualizarContato(int id, [FromBody] UpdateContatosDto contatoDtos)
        {
            return _contatoService.AtualizarContato(id, contatoDtos);
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarContatoPatch(int id, JsonPatchDocument<UpdateContatosDto> patch)
        {
            return _contatoService.AtualizarContatoPatch(id, patch);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaContato(int id)
        {
            return _contatoService.DeletaContato(id);
        }
    }
}
