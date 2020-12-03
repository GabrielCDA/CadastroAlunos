using EE.MatriculaAluno.UI.Models;
using System.Web.Mvc;

namespace EE.MatriculaAluno.UI.Controllers
{
    public class MatriculaController : Controller
    {
        [HttpPost]
        public ActionResult Matricula()
        {
            return View();
        }
    }
}