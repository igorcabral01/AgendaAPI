using AgendaAPI.Controllers;
using AgendaAPI.Data;
using AgendaAPI.Data.Dtos;
using AgendaAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgendaAPI.Tests
{
    public class ContatoControllerTests
    {
        private ContatosContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ContatoController _controller;

        public ContatoControllerTests()
        {

            var options = new DbContextOptionsBuilder<ContatosContext>()
                .UseInMemoryDatabase(databaseName: "AgendaAPIDatabase")
                .Options;

            _context = new ContatosContext(options);
            _mockMapper = new Mock<IMapper>();
            _controller = new ContatoController(_context, _mockMapper.Object);
        }


        private void ClearDatabase()
        {
            var entries = _context.Contato.ToList();
            _context.Contato.RemoveRange(entries);
            _context.SaveChanges();
        }

        [Fact]
        public void AdicionaContato()
        {
            ClearDatabase();

            var contatoDto = new CreatContatosDto { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _mockMapper.Setup(m => m.Map<Contatos>(contatoDto)).Returns(contato);
            var result = _controller.AdicionaContato(contatoDto);
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.BuscarContatoId), actionResult.ActionName);
        }
        [Fact]
        public void ContatosCadastrados()
        {
            ClearDatabase();
            var contatos = new List<Contatos>
    {
        new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" },
        new Contatos { Nome = "Maria", Telefone = 98765, Email = "maria@example.com" }
    };
            _context.Contato.AddRange(contatos);
            _context.SaveChanges();
            var expectedDtos = new List<ReadContatosDto>
    {
        new ReadContatosDto { Nome = "João", Telefone = 12345, Email = "joao@example.com" },
        new ReadContatosDto { Nome = "Maria", Telefone = 98765, Email = "maria@example.com" }
    };

            _mockMapper.Setup(m => m.Map<List<ReadContatosDto>>(contatos)).Returns(expectedDtos);
            var result = _controller.ContatosCadastrados();
            var okResult = Assert.IsAssignableFrom<IEnumerable<ReadContatosDto>>(result);
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }

        [Fact]
        public void BuscarContatoId()
        {
            ClearDatabase();

            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _context.Contato.Add(contato);
            _context.SaveChanges();
            _mockMapper.Setup(m => m.Map<ReadContatosDto>(contato))
                .Returns(new ReadContatosDto { Nome = contato.Nome, Telefone = contato.Telefone, Email = contato.Email });
            var result = _controller.BuscarContatoId(contato.Id);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReadContatosDto>(okResult.Value);
            Assert.Equal(contato.Nome, returnValue.Nome);
        }

        [Fact]
        public void AtualizarContato()
        {
            ClearDatabase();

            var contatoDto = new UpdateContatosDto { Nome = "João Atualizado", Telefone = 98765, Email = "joaoatualizado@example.com" };
            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _context.Contato.Add(contato);
            _context.SaveChanges();
            _mockMapper.Setup(m => m.Map(contatoDto, contato));
            var result = _controller.AtualizarContato(contato.Id, contatoDto);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void AtualizarContatoPatch()
        {
            ClearDatabase();

            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _context.Contato.Add(contato);
            _context.SaveChanges();
            var patchDoc = new JsonPatchDocument<UpdateContatosDto>();
            patchDoc.Replace(c => c.Nome, "João Atualizado");
            var updateDto = new UpdateContatosDto { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _mockMapper.Setup(m => m.Map<UpdateContatosDto>(contato)).Returns(updateDto);
            _mockMapper.Setup(m => m.Map(updateDto, contato)).Verifiable();
            var result = _controller.AtualizarContatoPatch(contato.Id, patchDoc);

            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify();
        }

        [Fact]
        public void DeletaContato()
        {
            ClearDatabase();

            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _context.Contato.Add(contato);
            _context.SaveChanges();
            var result = _controller.DeletaContato(contato.Id);

            Assert.IsType<NoContentResult>(result);
        }
    }
}