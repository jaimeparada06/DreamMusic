using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DreamMusic.Data;
using DreamMusic.Models;
using DreamMusic.Models.DiscoViewModels;
using DreamMusic.Models.CompraViewModels;


namespace DreamMusic.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            /* ANTES
            var applicationDbContext = _context.Compras.Include(c => c.Cliente);
            return View(await applicationDbContext.ToListAsync());
            */
            var applicationDbContext = _context.Compras
                .Include(p => p.Cliente)
                .Where(p => p.Cliente.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprar = await _context.Compras
                .Include(c => c.Cliente)
                .Include(p => p.ComprarItem).ThenInclude(p => p.Disco)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (comprar == null)
            {
                return NotFound();
            }

            return View(comprar);
        }

        // GET: Compras/Create
        public IActionResult Create(SelectedDiscosParaComprarViewModel selectedDiscos)
        {
            CompraCreateViewModel purchase = new();
            purchase.CompraItems = new List<CompraItemViewModel>();

            if (selectedDiscos.IdsToAdd == null)
            {
                ModelState.AddModelError("DiscoNoSeleccionado", "Tienes que seleccionar algún disco para que sea comprado, por favor");
            }
            else
                purchase.CompraItems = _context.Discos.Include(disco => disco.Genero)
                    .Select(disco => new CompraItemViewModel()
                    {
                        DiscoID = disco.DiscoId,
                        Genre = disco.Genero.Nombre,
                        PriceForCompra = disco.PrecioDeCompra,
                        Title = disco.Titulo
                    })
                    .Where(disco => selectedDiscos.IdsToAdd.Contains(disco.DiscoID.ToString())).ToList();

            Cliente Customer = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            purchase.Name = Customer.Name;
            purchase.FirstSurname = Customer.Apellido1;
            purchase.SecondSurname = Customer.Apellido2;

            return View(purchase);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CompraCreateViewModel purchaseViewModel)
        {
            Disco disco; ItemDeCompra purchaseItem;
            Cliente customer;
            Comprar purchase = new ();
            purchase.PrecioTotal = 0;
            purchase.ComprarItem = new List<ItemDeCompra>();
            customer = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));

            if (ModelState.IsValid)
            {
                foreach (CompraItemViewModel item in purchaseViewModel.CompraItems)
                {
                    disco = await _context.Discos.FirstOrDefaultAsync<Disco>(m => m.DiscoId == item.DiscoID);
                    if (disco.CantidadCompra < item.Quantity)
                    {
                        ModelState.AddModelError("", $"No hay disponibles los discos titulados {disco.Titulo}");
                    }
                    else
                    {
                        if (item.Quantity > 0)
                        {
                            disco.CantidadCompra -= item.Quantity;
                            purchaseItem = new ItemDeCompra
                            {
                                Disco = disco,
                                Comprar = purchase,
                                CantidadCompra = item.Quantity
                            };
                            purchase.PrecioTotal += item.Quantity * disco.PrecioDeCompra;
                            purchase.ComprarItem.Add(purchaseItem);
                        }
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                purchaseViewModel.Name = customer.Name;
                purchaseViewModel.FirstSurname = customer.Apellido1;
                purchaseViewModel.SecondSurname = customer.Apellido2;
                return View(purchaseViewModel);
            }


            purchase.Cliente = customer;
            purchase.FechaCompra = DateTime.Now;
            if (purchaseViewModel.PaymentMethod == "PayPal")
                purchase.MetodoDePago = new PayPal()
                {
                    CorreoElectronico = purchaseViewModel.Email,
                    Prefijo = purchaseViewModel.Prefix,
                    NumTelefono = purchaseViewModel.Phone
                };
            else
                purchase.MetodoDePago = new TarjetaDeCredito()
                {
                    NumeroTarjeta = purchaseViewModel.CreditCardNumber,
                    CCV = purchaseViewModel.CCV,
                    FechaCaducidad = (DateTime)purchaseViewModel.ExpirationDate
                };
            purchase.Direccion = purchaseViewModel.DeliveryAddress;
            _context.Add(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = purchase.CompraId });
        }
    

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprar = await _context.Compras.FindAsync(id);
            if (comprar == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", comprar.ClienteId);
            return View(comprar);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompraId,PrecioTotal,FechaCompra,Direccion,ClienteId")] Comprar comprar)
        {
            if (id != comprar.CompraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comprar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprarExists(comprar.CompraId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", comprar.ClienteId);
            return View(comprar);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprar = await _context.Compras
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (comprar == null)
            {
                return NotFound();
            }

            return View(comprar);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comprar = await _context.Compras.FindAsync(id);
            _context.Compras.Remove(comprar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComprarExists(int id)
        {
            return _context.Compras.Any(e => e.CompraId == id);
        }
    }
}
