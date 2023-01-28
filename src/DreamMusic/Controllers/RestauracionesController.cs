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
using DreamMusic.Models.RestauracionViewModels;
using static DreamMusic.Models.RestauracionViewModels.RestauracionCreateViewModel;

namespace DreamMusic.Controllers
{
    public class RestauracionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestauracionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restauraciones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.restauracion.Include(r => r.Administrador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Restauraciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restauracion = await _context.restauracion
                .Include(r => r.Administrador)
                .Include(d => d.ItemRestauracion).ThenInclude(d => d.Disco)
                .FirstOrDefaultAsync(m => m.RestauracionId == id);
            if (restauracion == null)
            {
                return NotFound();
            }

            return View(restauracion);
        }

        // GET: Restauraciones/Create
        public IActionResult Create(SelectedDiscosParaRestauracionViewModel selectedDiscos)
        {
            RestauracionCreateViewModel restauracion = new();
            restauracion.RestauracionItems = new List<RestauracionItemViewModel>();

            if (selectedDiscos.IdsToAdd == null)
            {
                ModelState.AddModelError("DiscoNoSeleccionado", "Tienes que seleccionar algún disco para que sea restaurado, por favor");
            }
            else
                restauracion.RestauracionItems = _context.Discos.Include(disco => disco.Genero)
                    .Select(disco => new RestauracionItemViewModel()
                    {
                        DiscoID = disco.DiscoId,
                        Genre = disco.Genero.Nombre,
                        PriceDeRestauracion = disco.PrecioDeRestauracion,
                        Title = disco.Titulo
                    })
                    .Where(disco => selectedDiscos.IdsToAdd.Contains(disco.DiscoID.ToString())).ToList();

            Administrador Customer = _context.Users.OfType<Administrador>().FirstOrDefault<Administrador>(u => u.UserName.Equals(User.Identity.Name));
            restauracion.Name = Customer.Name;
            restauracion.FirstSurname = Customer.Apellido1;
            restauracion.SecondSurname = Customer.Apellido2;

            return View(restauracion);

        }


        // POST: Restauraciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(RestauracionCreateViewModel restauracionViewModel)
        {
            Disco disco;
            ItemRestauracion restauracionItem;
            Administrador customer;
            Restauracion restauracion = new();
            restauracion.PrecioTotal = 0;
            restauracion.ItemRestauracion = new List<ItemRestauracion>();
            customer = await _context.Users.OfType<Administrador>().FirstOrDefaultAsync<Administrador>(u => u.UserName.Equals(User.Identity.Name));


            if (ModelState.IsValid)
            {
                foreach (RestauracionItemViewModel item in restauracionViewModel.RestauracionItems)
                {
                    disco = await _context.Discos.FirstOrDefaultAsync<Disco>(m => m.DiscoId == item.DiscoID);
                    if (disco.CantidadRestauracion < item.Quantity)
                    {
                        ModelState.AddModelError("", $"No hay suficientes discos titulados {disco.Titulo}");

                    }
                    else
                    {
                        if (item.Quantity > 0)
                        {
                            disco.CantidadRestauracion -= item.Quantity;
                            restauracionItem = new ItemRestauracion
                            {
                                Disco = disco,
                                Restauracion = restauracion,
                                CantidadRestauracion = item.Quantity
                            };
                            restauracion.PrecioTotal += item.Quantity * disco.PrecioDeRestauracion;
                            restauracion.ItemRestauracion.Add(restauracionItem);
                        }
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                restauracionViewModel.Name = customer.Name;
                restauracionViewModel.FirstSurname = customer.Apellido1;
                restauracionViewModel.SecondSurname = customer.Apellido2;
                return View(restauracionViewModel);
            }


            restauracion.Administrador = customer;
            restauracion.FechaRestauracion = DateTime.Now.Date;
            if (restauracionViewModel.PaymentMethod == "PayPal")
                restauracion.MetodoDePago = new PayPal()
                {
                    CorreoElectronico = restauracionViewModel.Email,
                    Prefijo = restauracionViewModel.Prefix,
                    NumTelefono = restauracionViewModel.Phone
                };
            else
                restauracion.MetodoDePago = new TarjetaDeCredito()
                {
                    NumeroTarjeta = restauracionViewModel.CreditCardNumber,
                    CCV = restauracionViewModel.CCV,
                    FechaCaducidad = (DateTime)restauracionViewModel.ExpirationDate
                };
            restauracion.Direccion = restauracionViewModel.DeliveryAddress;
            _context.Add(restauracion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = restauracion.RestauracionId });
        }

        // GET: Restauraciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restauracion = await _context.restauracion.FindAsync(id);
            if (restauracion == null)
            {
                return NotFound();
            }
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "Id", "Id", restauracion.AdministradorId);
            return View(restauracion);
        }

        // POST: Restauraciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestauracionId,PrecioTotal,FechaRestauracion,Direccion,AdministradorId")] Restauracion restauracion)
        {
            if (id != restauracion.RestauracionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restauracion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestauracionExists(restauracion.RestauracionId))
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
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "Id", "Id", restauracion.AdministradorId);
            return View(restauracion);
        }

        // GET: Restauraciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restauracion = await _context.restauracion
                .Include(r => r.Administrador)
                .FirstOrDefaultAsync(m => m.RestauracionId == id);
            if (restauracion == null)
            {
                return NotFound();
            }

            return View(restauracion);
        }

        // POST: Restauraciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restauracion = await _context.restauracion.FindAsync(id);
            _context.restauracion.Remove(restauracion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestauracionExists(int id)
        {
            return _context.restauracion.Any(e => e.RestauracionId == id);
        }
    }
}
