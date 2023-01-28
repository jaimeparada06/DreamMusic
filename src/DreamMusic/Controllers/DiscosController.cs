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
using Microsoft.AspNetCore.Authorization;

namespace DreamMusic.Controllers
{
    [Authorize]

    public class DiscosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Discos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Discos.ToListAsync());
        }

        // GET: Discos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disco = await _context.Discos
                .FirstOrDefaultAsync(m => m.DiscoId == id);
            if (disco == null)
            {
                return NotFound();
            }

            return View(disco);
        }

        // GET: Discos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("DiscoId,PrecioDeCompra,Titulo,Artista,FechaLanzamiento")] Disco disco)

        {
            if (ModelState.IsValid)
            {
                _context.Add(disco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disco);
        }

        // GET: Discos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disco = await _context.Discos.FindAsync(id);
            if (disco == null)
            {
                return NotFound();
            }
            return View(disco);
        }

        // POST: Discos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]



        public async Task<IActionResult> Edit(int id, [Bind("DiscoId,PrecioDeCompra,Titulo,Artista,FechaLanzamiento")] Disco disco)

        {
            if (id != disco.DiscoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscoExists(disco.DiscoId))
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
            return View(disco);
        }

        // GET: Discos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disco = await _context.Discos
                .FirstOrDefaultAsync(m => m.DiscoId == id);
            if (disco == null)
            {
                return NotFound();
            }

            return View(disco);
        }

        // POST: Discos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disco = await _context.Discos.FindAsync(id);
            _context.Discos.Remove(disco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscoExists(int id)
        {
            return _context.Discos.Any(e => e.DiscoId == id);
        }

        // GET: Discos/SelectDiscosParaComprar
        [HttpGet]
        public IActionResult SelectDiscosParaComprar(string discoTitulo, string discoGeneroSeleccionado, string discoArtista)
        {
            SelectDiscosParaComprarViewModel selectDiscos = new SelectDiscosParaComprarViewModel();
            selectDiscos.Generos = new SelectList(_context.Generos.Select(g => g.Nombre).ToList());
            selectDiscos.Discos = _context.Discos
                .Include(d => d.Genero)
                .Where(disco => (disco.Titulo.Contains(discoTitulo) || discoTitulo == null)
                && (disco.Genero.Nombre.Contains(discoGeneroSeleccionado) || discoGeneroSeleccionado == null)
                && (disco.Artista.Contains(discoArtista) || discoArtista == null));

            selectDiscos.Discos = selectDiscos.Discos.ToList();
            return View(selectDiscos);
        }


        // GET: Discos/SelectDiscosParaDevolucion
        [HttpGet]
        public IActionResult SelectDiscosParaDevolucion(string discoTitulo, string discoGeneroSeleccionado, string discoArtista, int? discoAño, int? discoMes, int? discoDia)
        {
            SelectDiscosParaDevolucionViewModel selectDiscos = new SelectDiscosParaDevolucionViewModel();
            /*
                        selectDiscos.Discos = _context.Discos
                            .Include(m => m.ItemDevolucion)
                            .ThenInclude(rm => rm.Devolucion)
                            .Where(m => !(m.ItemDevolucion.Any(rm => rm.Devolucion.FechaDevolucion.CompareTo(rm.Devolucion.FechaEntrega) >= 0
                              && rm.Devolucion.FechaEntrega.CompareTo(rm.Devolucion.FechaDevolucion) <= 0))).ToList();
            */

            selectDiscos.Generos = new SelectList(_context.Generos.Select(g => g.Nombre).ToList());

            selectDiscos.Discos = _context.Discos.Include(d => d.Genero)
                .ThenInclude(d => d.Discos).ThenInclude(d => d.ItemCompra).ThenInclude(d => d.Comprar)
                .Where(disco => (disco.Titulo.Contains(discoTitulo) || discoTitulo == null)
                && (disco.Genero.Nombre.Contains(discoGeneroSeleccionado) || discoGeneroSeleccionado == null)
                && (disco.Artista.Contains(discoArtista) || discoArtista == null)
                && (disco.ItemCompra.Any(disco => disco.Comprar.FechaCompra.Year == discoAño || discoAño == null))
                && (disco.ItemCompra.Any(disco => disco.Comprar.FechaCompra.Month - discoMes == 0 || discoMes == null))
                && (disco.ItemCompra.Any(disco => disco.Comprar.FechaCompra.Day - discoDia == 0 || discoDia == null))).ToList();

            return View(selectDiscos);
        }

        


        // GET: Discos/SelectDiscosParaRestauracion
        [HttpGet]
        public IActionResult SelectDiscosParaRestauracion(string discoTitulo, int? discoAño, int? discoMes, int? discoDia, String nombreC, String apellidoC)
        {
            SelectDiscosParaRestauracionViewModel selectDiscos = new SelectDiscosParaRestauracionViewModel();
            /*
            selectDiscos.Discos = _context.Discos
                .Include(m => m.ItemDevolucion)
                .ThenInclude(rm => rm.Devolucion)
                .Where(m => !(m.ItemDevolucion.Any(rm => rm.Devolucion.FechaDevolucion.CompareTo(rm.Devolucion.FechaEntrega) >= 0
                  && rm.Devolucion.FechaEntrega.CompareTo(rm.Devolucion.FechaDevolucion) <= 0))).ToList();
            */
            selectDiscos.Discos = _context.Discos.Include(d => d.ItemRestauracion).ThenInclude(d => d.Restauracion).ThenInclude(d => d.Administrador)
                .Include(d => d.ItemCompra).ThenInclude(d => d.Comprar)
                .Where(d => (d.ItemRestauracion.Any(d => d.Restauracion.Administrador.Name.Contains(nombreC) && d.Restauracion.Administrador.Apellido1.Contains(apellidoC)) || nombreC == null || apellidoC == null)
                && (d.Titulo.Contains(discoTitulo) || discoTitulo == null)
                && (d.ItemCompra.Any(d => d.Comprar.FechaCompra.Year == discoAño || discoAño == null)
                && (d.ItemCompra.Any(d => d.Comprar.FechaCompra.Month - discoMes == 0 || discoMes == null)
                && (d.ItemCompra.Any(d => d.Comprar.FechaCompra.Day - discoDia == 0 || discoDia == null)))));
            return View(selectDiscos);
        }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SelectDiscosParaDevolucion(SelectedDiscosParaDevolucionViewModel selectedDiscos)
    {
        if (selectedDiscos.IdsToAdd != null)
        {

                return RedirectToAction("Create", "Devoluciones", selectedDiscos);
            }

        ModelState.AddModelError(string.Empty, "Tienes que seleccionar al menos un disco");

        return SelectDiscosParaDevolucion(selectedDiscos.discoTitulo, selectedDiscos.discoGeneroSeleccionado, selectedDiscos.discoArtista,
        selectedDiscos.discoAño, selectedDiscos.discoMes, selectedDiscos.discoDia);
    }



        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectDiscosParaComprar(SelectedDiscosParaComprarViewModel selectedDiscos)

        {
            if (selectedDiscos.IdsToAdd != null)
            {
                return RedirectToAction("Create", selectedDiscos);
            }



            ModelState.AddModelError(string.Empty, "Tienes que seleccionar al menos un disco");


            return SelectDiscosParaComprar(selectedDiscos.discoTitulo, selectedDiscos.discoGeneroSeleccionado, selectedDiscos.discoArtista);

        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectDiscosParaComprar(SelectedDiscosParaComprarViewModel selectedDiscos)
        {
            if (selectedDiscos.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Compras", selectedDiscos);
            }

            ModelState.AddModelError(string.Empty, "Tienes que seleccionar al menos un disco");


            return SelectDiscosParaComprar(selectedDiscos.discoTitulo, selectedDiscos.discoGeneroSeleccionado, selectedDiscos.discoArtista);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectDiscosParaRestauracion(SelectedDiscosParaRestauracionViewModel selectedDiscos)
            {
            if (selectedDiscos.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Restauraciones", selectedDiscos);
            }

            ModelState.AddModelError(string.Empty, "Tienes que seleccionar al menos un disco");

            return SelectDiscosParaRestauracion(selectedDiscos.discoTitulo, selectedDiscos.discoAño, 
                selectedDiscos.discoMes, selectedDiscos.discoDia, selectedDiscos.nombreC, selectedDiscos.apellidoC);
        }

 
    }

   }



        

