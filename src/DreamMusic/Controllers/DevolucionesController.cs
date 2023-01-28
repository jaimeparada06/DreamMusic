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
using DreamMusic.Models.DevolucionViewModels;

namespace DreamMusic.Controllers
{
    public class DevolucionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevolucionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        // GET: Devoluciones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Devoluciones.Include(d => d.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Devoluciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devoluciones
                .Include(d => d.Cliente)
                .Include(d => d.ItemDevolucions).ThenInclude(d => d.Disco)
                .FirstOrDefaultAsync(m => m.DevolucionId == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // GET: Devoluciones/Create
        public IActionResult Create(SelectedDiscosParaDevolucionViewModel selectedDiscos)
        {
            DevolucionCreateViewModel devolucion = new ();
            devolucion.DevolucionItems = new List<DevolucionItemViewModel>();

            if (selectedDiscos.IdsToAdd == null)
            {
                ModelState.AddModelError("DiscoNoSeleccionado", "Tienes que seleccionar algún disco para que sea devuelto, por favor");
            }
            else
                devolucion.DevolucionItems = _context.Discos.Include(disco => disco.Genero)
                    .Select(disco => new DevolucionItemViewModel()
                    {
                        DiscoID = disco.DiscoId,
                        Genre = disco.Genero.Nombre,
                        PriceDeDevolucion = disco.PrecioDeDevolucion,
                        Title = disco.Titulo
                    })
                    .Where(disco => selectedDiscos.IdsToAdd.Contains(disco.DiscoID.ToString())).ToList();
            
            Cliente Customer = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            devolucion.Name = Customer.Name;
            devolucion.FirstSurname = Customer.Apellido1;
            devolucion.SecondSurname = Customer.Apellido2;
            
            return View(devolucion);

        }

        // POST: Devoluciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(DevolucionCreateViewModel devolucionViewModel)
        {
            Disco disco; 
            ItemDevolucion devolucionItem;
            Cliente customer;
            Devolucion devolucion = new();
            devolucion.PrecioTotal = 0;
            devolucion.ItemDevolucions = new List<ItemDevolucion>();
            customer = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));

            
            if (ModelState.IsValid)
            {
                foreach (DevolucionItemViewModel item in devolucionViewModel.DevolucionItems)
                {
                    disco = await _context.Discos.FirstOrDefaultAsync<Disco>(m => m.DiscoId == item.DiscoID);
                    if (disco.CantidadDevolucion < item.Quantity)
                    {
                        ModelState.AddModelError("", $"No hay suficientes discos titulados {disco.Titulo}");
                        
                    }
                    else
                    {
                        if (item.Quantity > 0)
                        {
                            disco.CantidadDevolucion -= item.Quantity;
                            devolucionItem = new ItemDevolucion
                            {
                                Disco = disco,
                                Devolucion = devolucion,
                                CantidadDevolucion = item.Quantity
                            };
                            devolucion.PrecioTotal += item.Quantity * disco.PrecioDeDevolucion;
                            devolucion.ItemDevolucions.Add(devolucionItem);
                        }
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                devolucionViewModel.Name = customer.Name;
                devolucionViewModel.FirstSurname = customer.Apellido1;
                devolucionViewModel.SecondSurname = customer.Apellido2;
                return View(devolucionViewModel);
            }


            devolucion.Cliente = customer;
            devolucion.FechaDevolucion = DateTime.Now.Date;
            if (devolucionViewModel.PaymentMethod == "PayPal")
                devolucion.MetodoDePago = new PayPal()
                {
                    CorreoElectronico = devolucionViewModel.Email,
                    Prefijo = devolucionViewModel.Prefix,
                    NumTelefono = devolucionViewModel.Phone
                };
            else
                devolucion.MetodoDePago = new TarjetaDeCredito()
                {
                    NumeroTarjeta = devolucionViewModel.CreditCardNumber,
                    CCV = devolucionViewModel.CCV,
                    FechaCaducidad = (DateTime)devolucionViewModel.ExpirationDate
                };
            devolucion.Direccion = devolucionViewModel.DeliveryAddress;
            _context.Add(devolucion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = devolucion.DevolucionId });
        }

        // GET: Devoluciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devoluciones.FindAsync(id);
            if (devolucion == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", devolucion.ClienteId);
            return View(devolucion);
        }

        // POST: Devoluciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DevolucionId,FechaEntrega,PrecioTotal,FechaDevolucion,Direccion,ClienteId")] Devolucion devolucion)
        {
            if (id != devolucion.DevolucionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devolucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucionExists(devolucion.DevolucionId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", devolucion.ClienteId);
            return View(devolucion);
        }

        // GET: Devoluciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devoluciones
                .Include(d => d.Cliente)
                .FirstOrDefaultAsync(m => m.DevolucionId == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // POST: Devoluciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devolucion = await _context.Devoluciones.FindAsync(id);
            _context.Devoluciones.Remove(devolucion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucionExists(int id)
        {
            return _context.Devoluciones.Any(e => e.DevolucionId == id);
        }
    }
}
