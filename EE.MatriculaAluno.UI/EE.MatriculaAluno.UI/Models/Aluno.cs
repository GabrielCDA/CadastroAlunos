using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EE.MatriculaAluno.UI.Models;

namespace EE.MatriculaAluno.UI.Models
{

    public class Aluno
    {
        public int IdMateria { get; set; }
        public int IdAluno { get; set; }
        public string Nome { get; set; }
        public decimal Nota { get; set; }
        public string NomeMateria { get; set; }
        
    }


}