using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agenda.Domain;
using AutoFixture;
using NUnit.Framework;

namespace Agenda.DAL.Test
{
    [TestFixture]
    public class Contatos2Test : BaseTest
    {
        Contatos _contatos;
        Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _contatos = new Contatos();
            _fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            _contatos = null;
            _fixture = null;
        }

        [Test]
        public void ObterTodosOsContatosTest()
        {
            //Monta
            var contato1 = _fixture.Create<Contato>();
            var contato2 = _fixture.Create<Contato>();

            //Executa
            _contatos.Adicionar(contato1);
            _contatos.Adicionar(contato2);

            var lstContato = _contatos.ObterTodos();
            var contatoResultado = lstContato.Where(x => x.Id == contato1.Id).FirstOrDefault();

            //Verifica
            Assert.AreEqual(2, lstContato.Count());
            Assert.AreEqual(contato1.Id, contatoResultado.Id);
            Assert.AreEqual(contato1.Nome, contatoResultado.Nome);
        }
    }
}
