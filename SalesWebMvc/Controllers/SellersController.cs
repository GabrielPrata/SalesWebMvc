using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        //Por padrão todas as ações são do tipo IActionResult
        public IActionResult Create()
        {
            //Pego os departamentos e passo para a tela que contém o formulário de cadastro
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }


        //Anotação para indicar o tipo da ação
        [HttpPost]

        //Anotação para previnir ataques do tipo CSRF
        [ValidateAntiForgeryToken]

        //Este objeto vendedor vem através da requisição
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);

            //Faço o redirecionamento para a tela Index
            //Posso fazer da forma abaixo
            //return RedirectToAction("Index");

            //Ou posso fazer assim:
            //Fazendo desta forma é melhor pois se futuramente o nome da string da ação Index() eu não preciso alterar nada no código
            return RedirectToAction(nameof(Index));
        }

        //A interrogação após o int significa que este parâmetro é opcional
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Uso id.Value pois id é nullable (int?)
            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id) 
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }
}
