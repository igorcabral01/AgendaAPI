using AgendaAPI.Data;
using AgendaAPI.Data.Dtos;
using AgendaAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class ContatoController : ControllerBase
{
    private ContatosContext _context;
    private IMapper _mapper;



    public ContatoController(ContatosContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("/CadastrarContatos")]
    public IActionResult AdicionaContato([FromBody] CreatContatosDto contatoDto)
    {
        Contatos contato =  _mapper.Map<Contatos>(contatoDto);
        _context.Contato.Add(contato);
        _context.SaveChanges();
        Console.WriteLine(contato.Nome);
        return  CreatedAtAction(nameof(BuscarContatoId), new{id = contato.Id},contato);
 
    }

    [HttpGet("/Contatos")]
    public IEnumerable<ReadContatosDto> ContatosCadastrados([FromQuery] int skip = 0, [FromQuery ]int take = 50)
    {
        return _mapper.Map<List<ReadContatosDto>>(_context.Contato.Skip(skip).Take(take));
    }

    [HttpGet("/Buscar/{id}")]
    public IActionResult BuscarContatoId(int id)
    {
        var contato = _context.Contato.FirstOrDefault(contatos => contatos.Id == id);
        if(contato == null) return NotFound();
        var ContatosDto = _mapper.Map<ReadContatosDto>(contato);
        return Ok(ContatosDto);

    }

    [HttpPut("/AtualizarContato/{id}")]
    public IActionResult AtualizarContato(int id, [FromBody] UpdateContatosDto contatoDtos)
    {
        var contatos = _context.Contato.FirstOrDefault(contato => contato.Id == id);
        if (contatos == null) return NotFound();
        _mapper.Map(contatoDtos,contatos);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizarContatoPatch(int id,JsonPatchDocument<UpdateContatosDto> patch)
    {
        var contatos = _context.Contato.FirstOrDefault(contato => contato.Id == id);
        if (contatos == null) return NotFound();

        var ContatoParaAtualizar = _mapper.Map<UpdateContatosDto>(contatos);
        patch.ApplyTo(ContatoParaAtualizar, ModelState);
        if(!TryValidateModel(ContatoParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(ContatoParaAtualizar, contatos);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaContato(int id)
    {
        var contatos = _context.Contato.FirstOrDefault(contato => contato.Id == id);
        if (contatos == null) return NotFound();
        _context.Remove(contatos);
        _context.SaveChanges();
        return NoContent();
    }


}
