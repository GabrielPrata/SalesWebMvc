using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

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

        //Chamada síncrona
        //public IActionResult Index()
        //{
        //    var list = _sellerService.FindAll();
        //     return View(list);
        //}

        //Chamada assíncrona
        //Aqui, por se tratar de um controlador eu devo respeitar o padrão de nomes do framework
        //Portanto devo manter o nome como Index()
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
             return View(list);
        }


        //Por padrão todas as ações são do tipo IActionResult
        public async Task<IActionResult> Create()
        {
            //Pego os departamentos e passo para a tela que contém o formulário de cadastro
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }


        //Anotação para indicar o tipo da ação
        [HttpPost]

        //Anotação para previnir ataques do tipo CSRF
        [ValidateAntiForgeryToken]

        //Este objeto vendedor vem através da requisição
        public async Task<IActionResult> Create(Seller seller)
        {
            //Para realizar as validações do lado do servidor:
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);

            //Faço o redirecionamento para a tela Index
            //Posso fazer da forma abaixo
            //return RedirectToAction("Index");

            //Ou posso fazer assim:
            //Fazendo desta forma é melhor pois se futuramente o nome da string da ação Index() eu não preciso alterar nada no código
            return RedirectToAction(nameof(Index));
        }

        //A interrogação após o int significa que este parâmetro é opcional
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            //Uso id.Value pois id é nullable (int?)
            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller) 
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Mismatch" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            //Para simplificar o código eu poderia utlizar apenas o código comentado abaixo, pois ambas as exxcessões 
            //são do mesmo tipo, portanto podem ser resolvidas por meio de upcasting
            
            //catch (ApplicationException e)
            //{
            //  return RedirectToAction(nameof(Error), new { message = e.Message });
            //}

            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        //Como a ação de erro não possui acesso a dados, ela não precisa ser assíncrona
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,

                //Current é opcional por conta da interrogação "Current?"
                //Caso ele seja nulo eu irei usar o HttpContext.TraceIdentifier
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
