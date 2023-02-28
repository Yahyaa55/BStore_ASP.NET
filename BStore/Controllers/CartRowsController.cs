using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BStore.Data;
using BStore.Models;
using Microsoft.AspNetCore.Identity;

namespace BStore.Controllers
{
    public class CartRowsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CartRowsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int productID, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var cart = await _context.Cart
                .Include(c => c.User)
                .Where(c => c.User.Id == user.Id)
                .Include(c => c.CartRows)
                .ThenInclude(cl => cl.Product)
                .FirstAsync();
            var product = await _context.Product.FindAsync(productID);
            if (product == null) return NotFound();
            var cartRow = cart.CartRows.FirstOrDefault(cl => cl.Product.Id == productID);
            if (cartRow == null)
            {
                cartRow = new CartRow
                {
                    Cart = cart,
                    Product = product,
                    Quantity = quantity
                };
                _context.CartRow.Add(cartRow);
            }
            else
            {
                cartRow.Quantity += quantity;
                _context.CartRow.Add(cartRow);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCart", controllerName: "Carts");
        }

        // GET: CartRows
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CartRow.Include(c => c.Cart).Include(c => c.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CartRows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CartRow == null)
            {
                return NotFound();
            }

            var cartRow = await _context.CartRow
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartRow == null)
            {
                return NotFound();
            }

            return View(cartRow);
        }

        // GET: CartRows/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Set<Cart>(), "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

        // POST: CartRows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,TotalRow,CartId,ProductId")] CartRow cartRow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartRow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Set<Cart>(), "Id", "Id", cartRow.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cartRow.ProductId);
            return View(cartRow);
        }

        // GET: CartRows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CartRow == null)
            {
                return NotFound();
            }

            var cartRow = await _context.CartRow.FindAsync(id);
            if (cartRow == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Set<Cart>(), "Id", "Id", cartRow.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cartRow.ProductId);
            return View(cartRow);
        }

        // POST: CartRows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,TotalRow,CartId,ProductId")] CartRow cartRow)
        {
            if (id != cartRow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartRow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartRowExists(cartRow.Id))
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
            ViewData["CartId"] = new SelectList(_context.Set<Cart>(), "Id", "Id", cartRow.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cartRow.ProductId);
            return View(cartRow);
        }

        // GET: CartRows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CartRow == null)
            {
                return NotFound();
            }

            var cartRow = await _context.CartRow
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartRow == null)
            {
                return NotFound();
            }

            return View(cartRow);
        }

        // POST: CartRows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CartRow == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CartRow'  is null.");
            }
            var cartRow = await _context.CartRow.FindAsync(id);
            if (cartRow != null)
            {
                _context.CartRow.Remove(cartRow);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartRowExists(int id)
        {
          return _context.CartRow.Any(e => e.Id == id);
        }
    }
}
