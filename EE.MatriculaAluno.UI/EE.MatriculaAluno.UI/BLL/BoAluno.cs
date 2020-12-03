using EE.MatriculaAluno.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.MatriculaAluno.UI.BLL
{
    public class BoAluno
    {
        public int IncluirAluno(Aluno aluno)
        {
            DAL.DaoAluno Aluno = new DAL.DaoAluno();
            return Aluno.IncluirAluno(aluno);
        }

        public void IncluirNotasAluno(Aluno aluno)
        {
            DAL.DaoAluno Aluno = new DAL.DaoAluno();
            Aluno.IncluirNotasAluno(aluno);
        }

        public List<Materia> ListarMaterias()
        {
            DAL.DaoAluno Aluno = new DAL.DaoAluno();
            return Aluno.ConsultarMaterias();
        }

        public List<Aluno> ListarNotasMaterias()
        {
            DAL.DaoAluno Aluno = new DAL.DaoAluno();
            return Aluno.ConsultarNotasMaterias();
        }
        public Usuario ListarUsuario(string Usuario)
        {
            DAL.DaoAluno Aluno = new DAL.DaoAluno();
            return Aluno.ConsultarUsuario(Usuario);
        }
        
        public void ApagarDados()
        {
            DAL.DaoAluno Aluno = new DAL.DaoAluno();
            Aluno.ApagarDados();
        }

    }
}