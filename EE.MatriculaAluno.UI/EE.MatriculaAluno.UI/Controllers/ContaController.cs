using Bogus;
using EE.MatriculaAluno.UI.BLL;
using EE.MatriculaAluno.UI.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace EE.MatriculaAluno.UI.Controllers
{
    public class ContaController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            BoAluno aluno = new BoAluno();
            Usuario usuario = aluno.ListarUsuario(login.Usuario);
            var achou = (login.Usuario == usuario.User && login.Senha == usuario.Senha);
            //var achou = (login.Usuario == "candidato-evolucional" && login.Senha == "123456");
            ViewBag.Autenticado = achou;
            if (achou)
            {
                FormsAuthentication.SetAuthCookie(login.Usuario, login.LembrarMe);
 
                RedirectToAction("Index", "Home");

              
            }
            else
            {
                ModelState.AddModelError("", "Login inválido.");
            }

            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult InserirAlunos()
        {
            try
            {
                BoAluno aluno = new BoAluno();

                List<Materia> materias = aluno.ListarMaterias();

                List<Aluno> alunosDistintos = GerarListaAlunos();
                aluno.ApagarDados();

                foreach (var item in alunosDistintos)
                {
                    item.IdAluno = aluno.IncluirAluno(item);

                    foreach (var materia in materias)
                    {
                        item.IdMateria = materia.IdMateria;
                        aluno.IncluirNotasAluno(item);
                    }

                }

                ViewBag.MensagemSucesso = "Alunos Inseridos com Sucesso";
            }
            catch (Exception ex)
            {
                ViewBag.MensagemSucesso = "Erro ao insetir Alunos: " + ex.Message;
            }
            
            return View("Login");

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GerarExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                BoAluno aluno = new BoAluno();
                List<Materia> materias = aluno.ListarMaterias();
                var titulos = new String[materias.Count + 2];


                List<Aluno> listaAlunos = aluno.ListarNotasMaterias();

                titulos[0] = "Aluno";
                for (int i = 1; i < materias.Count + 1; i++)
                {
                    titulos[i] = materias[i - 1].NomeMateria;
                }
                titulos[materias.Count + 1] = "Média";

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "Evolucional";
                    excelPackage.Workbook.Properties.Title = "Alunos";

                    var sheet = excelPackage.Workbook.Worksheets.Add("Planilha 1");
                    sheet.Name = "Planilha 1";

                    // Títulos
                    var i = 1;
                    foreach (var titulo in titulos)
                    {
                        sheet.Cells[1, i++].Value = titulo;
                    }

                    List<Aluno> listaDistintosID = GetDiferentesAlunos(listaAlunos);
                    // Valores
                    for (int j = 2; j < listaDistintosID.Count + 2; j++)
                    {
                        i = 1;
                        var alunoIndividual = listaAlunos.Where(x => x.IdAluno == listaDistintosID[j - 2].IdAluno);
                        var valores = new String[titulos.Count()];
                        valores[0] = alunoIndividual.Where(x => x.IdAluno == listaDistintosID[j - 2].IdAluno).First().Nome;
                        int a = 1;
                        decimal media = 0;
                        foreach (var item in alunoIndividual)
                        {
                            media = media + item.Nota;
                            valores[a] = item.Nota.ToString();
                            a++;
                        }
                        valores[titulos.Count() -1] = (media / alunoIndividual.Count()).ToString("0.00");
                        foreach (var valor in valores)
                        {
                            sheet.Cells[j, i++].Value = valor;
                        }
                    }

                    string nomeDiretorio = @"C:\ControleAlunos";

                    if (!Directory.Exists(nomeDiretorio))
                    {
                        Directory.CreateDirectory(nomeDiretorio);
                    }
                    string path = @"C:\ControleAlunos\Alunos.xlsx";
                    System.IO.File.WriteAllBytes(path, excelPackage.GetAsByteArray());
                    ViewBag.MensagemSucesso = "Excel Gerado com Sucesso - Diretorio: " + path;
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.MensagemSucesso = "Erro ao gerar planilha excel: " + ex.Message ;
            }
            return View("Login");
        }

        private List<Aluno> GerarListaAlunos()
        {
            var faker = new Faker<Aluno>("pt_BR").StrictMode(true)
               .RuleFor(p => p.Nome, f => f.Person.FullName)
               .RuleFor(p => p.IdAluno, 0)
               .RuleFor(p => p.IdMateria, 0)
               .RuleFor(p => p.NomeMateria, "")
               .RuleFor(p => p.Nota, 0);

            var pessoas = faker.Generate(1000);

            var distincAlunos = pessoas.GroupBy(i => i.Nome).Select(g => g.First()).ToList();

            if (distincAlunos.Count < 1000)
            {
                while (distincAlunos.Count < 1000)
                {
                    var alunosRestantes = faker.Generate(1000 - distincAlunos.Count);
                    foreach (var item in alunosRestantes)
                    {
                        distincAlunos.Add(item);
                    }

                    distincAlunos = distincAlunos.GroupBy(i => i.Nome).Select(g => g.First()).ToList();
                }
            }
            return distincAlunos;
        }

        private List<Aluno> GetDiferentesAlunos(List<Aluno> listaAlunos)
        {
         
            var distincAlunos = listaAlunos.GroupBy(i => i.IdAluno).Select(g => g.First()).ToList();
                       
            return distincAlunos;
        }
    }
}