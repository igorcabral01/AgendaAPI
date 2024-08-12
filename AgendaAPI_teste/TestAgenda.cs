using AgendaAPI.Controllers;
using AgendaAPI.Data.Dtos;
using AgendaAPI.Models;
using AgendaAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AgendaAPI.Tests
{
    public class ContatoControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IContatoService> _mockContatoService;
        private readonly ContatoController _controller;

        public ContatoControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockContatoService = new Mock<IContatoService>();

            
            _controller = new ContatoController(_mockContatoService.Object);
        }

        [Fact]
        public void AdicionaContato()
        {
            var contatoDto = new CreatContatosDto { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _mockMapper.Setup(m => m.Map<Contatos>(contatoDto)).Returns(contato);
            _mockContatoService.Setup(s => s.AdicionaContato(contatoDto)).Returns(new CreatedAtActionResult(nameof(_controller.BuscarContatoId), "Contato", contato, contato.Id)); 

            var result = _controller.AdicionaContato(contatoDto);
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.BuscarContatoId), actionResult.ActionName);
        }

        [Fact]
        public void ContatosCadastrados()
        {
            var contatos = new List<Contatos>
            {
                new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" },
                new Contatos { Nome = "Maria", Telefone = 98765, Email = "maria@example.com" }
            };
            var expectedDtos = new List<ReadContatosDto>
            {
                new ReadContatosDto { Nome = "João", Telefone = 12345, Email = "joao@example.com" },
                new ReadContatosDto { Nome = "Maria", Telefone = 98765, Email = "maria@example.com" }
            };

            _mockMapper.Setup(m => m.Map<List<ReadContatosDto>>(contatos)).Returns(expectedDtos);
            _mockContatoService.Setup(s => s.ContatosCadastrados(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedDtos); 

            var result = _controller.ContatosCadastrados();
            var okResult = Assert.IsAssignableFrom<IEnumerable<ReadContatosDto>>(result);
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }

        [Fact]
        public void BuscarContatoId()
        {
            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _mockMapper.Setup(m => m.Map<ReadContatosDto>(contato))
                .Returns(new ReadContatosDto { Nome = contato.Nome, Telefone = contato.Telefone, Email = contato.Email });
            _mockContatoService.Setup(s => s.BuscarContatoId(contato.Id)).Returns(new OkObjectResult(new ReadContatosDto { Nome = contato.Nome, Telefone = contato.Telefone, Email = contato.Email })); 

            var result = _controller.BuscarContatoId(contato.Id);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReadContatosDto>(okResult.Value);
            Assert.Equal(contato.Nome, returnValue.Nome);
        }

        [Fact]
        public void AtualizarContato()
        {
            var contatoDto = new UpdateContatosDto { Nome = "João Atualizado", Telefone = 98765, Email = "joaoatualizado@example.com" };
            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _mockMapper.Setup(m => m.Map(contatoDto, contato));
            _mockContatoService.Setup(s => s.AtualizarContato(contato.Id, contatoDto)).Returns(new NoContentResult()); 

            var result = _controller.AtualizarContato(contato.Id, contatoDto);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void AtualizarContatoPatch()
        {
            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            var patchDoc = new JsonPatchDocument<UpdateContatosDto>();
            patchDoc.Replace(c => c.Nome, "João Atualizado");

            _mockMapper.Setup(m => m.Map<UpdateContatosDto>(contato))
                .Returns(new UpdateContatosDto { Nome = "João", Telefone = 12345, Email = "joao@example.com" });
            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateContatosDto>(), It.IsAny<Contatos>())).Verifiable();
            _mockContatoService.Setup(s => s.AtualizarContatoPatch(contato.Id, patchDoc)).Returns(new NoContentResult()); 

            var result = _controller.AtualizarContatoPatch(contato.Id, patchDoc);

            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify();
        }

        [Fact]
        public void DeletaContato()
        {
            var contato = new Contatos { Nome = "João", Telefone = 12345, Email = "joao@example.com" };
            _mockContatoService.Setup(s => s.DeletaContato(contato.Id)).Returns(new NoContentResult()); 

            var result = _controller.DeletaContato(contato.Id);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
