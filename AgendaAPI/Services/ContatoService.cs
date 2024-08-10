using AgendaAPI.Data;
using AgendaAPI.Data.Dtos;
using AgendaAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AgendaAPI.Services
{
    public class ContatoService : IContatoService
    {
        private readonly ContatosContext _context;
        private readonly IMapper _mapper;

        public ContatoService(ContatosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult AdicionaContato(CreatContatosDto contatoDto)
        {

            var contatoExistente = _context.Contato
                .FirstOrDefault(c =>
                    c.Nome == contatoDto.Nome &&
                    c.Telefone == contatoDto.Telefone &&
                    c.Email == contatoDto.Email);

            if (contatoExistente != null)
            {

                return new ConflictObjectResult("Já existe um contato com o mesmo nome, número e e-mail.");
            }


            var contato = _mapper.Map<Contatos>(contatoDto);

            _context.Contato.Add(contato);
            _context.SaveChanges();

            return new CreatedAtActionResult(nameof(BuscarContatoId), "Contato", new { id = contato.Id }, contato);
        }


        public IEnumerable<ReadContatosDto> ContatosCadastrados(int skip = 0, int take = 50)
        {
            return _mapper.Map<List<ReadContatosDto>>(
                _context.Contato.Skip(skip).Take(take).ToList()
            );
        }

        public IActionResult BuscarContatoId(int id)
        {
            var contato = _context.Contato.FirstOrDefault(c => c.Id == id);
            if (contato == null) return new NotFoundResult();

            var contatoDto = _mapper.Map<ReadContatosDto>(contato);
            return new OkObjectResult(contatoDto);
        }

        public IActionResult AtualizarContato(int id, UpdateContatosDto contatoDtos)
        {
            var contato = _context.Contato.FirstOrDefault(c => c.Id == id);
            if (contato == null) return new NotFoundResult();

            _mapper.Map(contatoDtos, contato);
            _context.SaveChanges();
            return new NoContentResult();
        }

        public IActionResult AtualizarContatoPatch(int id, JsonPatchDocument<UpdateContatosDto> patch)
        {
            var contato = _context.Contato.FirstOrDefault(c => c.Id == id);
            if (contato == null) return new NotFoundResult();

            var contatoParaAtualizar = _mapper.Map<UpdateContatosDto>(contato);
            patch.ApplyTo(contatoParaAtualizar);

            if (!TryValidateModel(contatoParaAtualizar))
            {
                return new BadRequestObjectResult(ModelState);
            }

            _mapper.Map(contatoParaAtualizar, contato);
            _context.SaveChanges();
            return new NoContentResult();
        }

        public IActionResult DeletaContato(int id)
        {
            var contato = _context.Contato.FirstOrDefault(c => c.Id == id);
            if (contato == null) return new NotFoundResult();

            _context.Contato.Remove(contato);
            _context.SaveChanges();
            return new NoContentResult();
        }

        private bool TryValidateModel(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(model, validationContext, validationResults, true);
        }
    }
}
