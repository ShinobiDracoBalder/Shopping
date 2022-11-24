using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shooping.Common.BrainTree;
using Shooping.Common.Enums;
using Shooping.Common.Utilities;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Interfaces;
using System;
using Vereyon.Web;

namespace Shopping.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFlashMessage _flashMessage;
        private readonly IOrdersHelper _ordersHelper;
        private readonly IBrainTreeGate _brain;

        public OrdersController(DataContext dataContext, IFlashMessage flashMessage, IOrdersHelper ordersHelper, IBrainTreeGate brain)
        {
            _dataContext = dataContext;
            _flashMessage = flashMessage;
            _ordersHelper = ordersHelper;
            _brain = brain;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _dataContext.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dispatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _dataContext.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.Nuevo)
            {
                _flashMessage.Danger("Solo se pueden despachar pedidos que estén en estado 'nuevo'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Despachado;
                _dataContext.Sales.Update(sale);
                await _dataContext.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'despachado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Send(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _dataContext.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus != OrderStatus.Despachado)
            {
                _flashMessage.Danger("Solo se pueden enviar pedidos que estén en estado 'despachado'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Enviado;
                _dataContext.Sales.Update(sale);
                await _dataContext.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'enviado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _dataContext.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.Enviado)
            {
                _flashMessage.Danger("Solo se pueden confirmar pedidos que estén en estado 'enviado'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Confirmado;
                _dataContext.Sales.Update(sale);
                await _dataContext.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'confirmado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _dataContext.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus == OrderStatus.Cancelado)
            {
                _flashMessage.Danger("No se puede cancelar un pedido que esté en estado 'cancelado'.");
            }
            else
            {
                await _ordersHelper.CancelOrderAsync(sale.Id);
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'cancelado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _dataContext.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Where(s => s.User.UserName == User.Identity.Name)
                .ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _dataContext.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return NotFound();
            }
            sale.Remarks = String.IsNullOrEmpty(sale.Remarks) ? "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout." : sale.Remarks;
            var gateway = _brain.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;

            return View(sale);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("MyDetails")]
        public async Task<IActionResult> MyDetails(IFormCollection collection) 
        {
            string nonceFromTheClient = collection["payment_method_nonce"];
            Random r = new Random();
            int genRandId = r.Next(100, 5000);
            Random random = new Random();
            decimal FinalVentaTotal = random.Next(100, 200000);

            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(String.Format("{0:0.##}", FinalVentaTotal)),
                PaymentMethodNonce = nonceFromTheClient,
                OrderId = genRandId.ToString(),
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var gateway = _brain.GetGateway();
            Result<Transaction> result = gateway.Transaction.Sale(request);

            //// Modificar la Venta
            if (result.Target.ProcessorResponseText == "Approved")
            {
                ViewBag.TransaccionId = result.Target.Id;
                ViewBag.EstadoVenta = WC.EstadoAprobado;
            }
            else
            {
                ViewBag.EstadoVenta = WC.EstadoCancelado;
            }
            return RedirectToAction(nameof(MyOrders), new {  });
        }
    }
}
