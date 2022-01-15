using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agenda.Domain;
using Dapper;

namespace Agenda.DAL
{
    public class Contatos
    {
        string _strCon;

        //SqlConnection _con;

        public Contatos()
        {
            _strCon = @"Data Source=.\sqlexpress;Initial Catalog=AgendaTest;Integrated Security=True;";
            //_con = new SqlConnection(_strCon);
        }
        public void Adicionar(Contato contato)
        {
            using (var con = new SqlConnection(_strCon))
            {
                con.Open();
                con.Execute("insert into Contato(Id, Nome) values(@Id, @Nome)", contato);
                //con.Open();
                //var sql = string.Format("insert into Contato (Id, Nome) values ('{0}', '{1}')", contato.Id, contato.Nome);

                //var cmd = new SqlCommand(sql, con);

                //cmd.ExecuteNonQuery();
            }
        }

        public Contato Obter(Guid id)
        {
            Contato contato;
            using (var con = new SqlConnection(_strCon))
            {
                con.Open();
                contato = con.QueryFirst<Contato>("select Id, Nome from Contato where id = @Id", new { Id = id });
                //con.Open();
                //var sql = string.Format("select Id, Nome from Contato where id = '{0}'", id);
                //var cmd = new SqlCommand(sql, con);

                //var sqlDataReader = cmd.ExecuteReader();
                //sqlDataReader.Read();

                //contato = new Contato()
                //{
                //    Id = Guid.Parse(sqlDataReader["Id"].ToString()),
                //    Nome = sqlDataReader["Nome"].ToString()
                //};
            }

            return contato;
        }


        public List<Contato> ObterTodos()
        {
            var contatos = new List<Contato>();
            using (var con = new SqlConnection(_strCon))
            {
                con.Open();
                contatos = con.Query<Contato>("select Id, Nome from Contato").ToList();
            }

            return contatos;
        }
    }
}
