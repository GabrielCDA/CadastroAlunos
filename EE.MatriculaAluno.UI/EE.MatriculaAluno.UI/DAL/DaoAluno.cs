using Bogus;
using EE.MatriculaAluno.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EE.MatriculaAluno.UI.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Cliente
    /// </summary>
    internal class DaoAluno : AcessoDados
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal int IncluirAluno(Aluno aluno)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", aluno.Nome));


            System.Data.DataSet ds = base.Consultar("EE_SP_IncAluno", parametros);
            int ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);

            return ret;
        }

        internal long IncluirNotasAluno(Aluno aluno)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            var faker = new Faker<Materia>("pt_BR").StrictMode(true)
                 .RuleFor(p => p.NotaMateria, f => f.Finance.Amount(1, 10))
                 .RuleFor(p => p.IdMateria, 0)
                 .RuleFor(p => p.NomeMateria, "");

            var nota = faker.Generate(1);

            parametros.Add(new System.Data.SqlClient.SqlParameter("IdMateria", aluno.IdMateria));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdAluno", aluno.IdAluno));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Nota", nota[0].NotaMateria));

            System.Data.DataSet ds = base.Consultar("EE_SP_IncNotasAluno", parametros);
            long ret = 0;

            return ret;
        }

        internal List<Materia> ConsultarMaterias()
        {
            List<Materia> materias = new List<Materia>();
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();
            parametros.Add(new System.Data.SqlClient.SqlParameter("MateriaID", "0"));
            System.Data.DataSet ds = base.Consultar("EE_SP_ConsMaterias", parametros);           
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Materia materia = new Materia();
                materia.IdMateria = row.Field<int>("MateriaID");
                materia.NomeMateria = row.Field<string>("NomeMateria");

                materias.Add(materia);
            } 

            return materias;
        }

        internal Usuario ConsultarUsuario(string Usuario)
        {
            Usuario usuario = new Usuario();
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();
            parametros.Add(new System.Data.SqlClient.SqlParameter("Usuario", Usuario));
            System.Data.DataSet ds = base.Consultar("EE_SP_ConsUsuario", parametros);
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                usuario.User = row.Field<string>("Usuario");
                usuario.Senha = row.Field<string>("Senha");

            }

            return usuario;
        }

        internal void ApagarDados()
        {            
            base.Executar("EE_SP_ApagarDados", null);           
        }


        internal List<Aluno> ConsultarNotasMaterias()
        {
            List<Aluno> notasAlunosMaterias = new List<Aluno>();
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();
            parametros.Add(new System.Data.SqlClient.SqlParameter("AlunoID", "0"));
            System.Data.DataSet ds = base.Consultar("EE_SP_ConsNotasMaterias", parametros);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Aluno aluno = new Aluno();
                aluno.IdMateria = row.Field<int>("MateriaID");
                aluno.NomeMateria = row.Field<string>("NomeMateria");
                aluno.Nota = row.Field<decimal>("Nota");
                aluno.IdAluno = row.Field<int>("AlunoID");
                aluno.Nome = row.Field<string>("NomeAluno");

                notasAlunosMaterias.Add(aluno);
            }

            return notasAlunosMaterias;
        }        

    }
}