/*Name: Michal Drahorat
 * Description: The controller for the Driver table
 * 
 * Rev. History
 * October 28th, 2016 - Created by Michal Drahorat
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using A2BusService.Models;

namespace A1BusService.Controllers
{
    public class MDDriverController : Controller
    {
        private readonly BusServiceContext _context;

        public MDDriverController(BusServiceContext context)
        {
            _context = context;    
        }

        // GET: Driver
        public async Task<IActionResult> Index()
        {
            var busServiceContext = _context.Driver.Include(d => d.ProvinceCodeNavigation)
                .OrderBy(a => a.FullName);
            return View(await busServiceContext.ToListAsync());
        }

        //public async Task<IActionResult> AddDriver()
        //{
        //    returhn
        //}

        // GET: Driver/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver.SingleOrDefaultAsync(m => m.DriverId == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Driver/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Postback for driver create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverId,City,DateHired,FirstName,FullName,HomePhone,LastName,PostalCode,ProvinceCode,Street,WorkPhone")] Driver driver)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(driver);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "New driver created successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Exception thrown on create {ex.GetBaseException().Message}");
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", driver.ProvinceCode);
            return View(driver);
        }

        // GET: Driver/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver.SingleOrDefaultAsync(m => m.DriverId == id);
            if (driver == null)
            {
                return NotFound();
            }
            ViewData["name"] = driver.FullName;
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", driver.ProvinceCode)
                .OrderBy(p => driver.ProvinceCodeNavigation.Name);
            return View(driver);
        }

        // POST: Postback for driver edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DriverId,City,DateHired,FirstName,FullName,HomePhone,LastName,PostalCode,ProvinceCode,Street,WorkPhone")] Driver driver)
        {
            if (id != driver.DriverId)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(driver);
                        await _context.SaveChangesAsync();
                        TempData["message"] = "Driver information successfully updated.";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DriverExists(driver.DriverId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Exception thrown on edit {ex.GetBaseException().Message}");
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", driver.ProvinceCode);
            return View(driver);
        }

        // GET: Driver/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver.SingleOrDefaultAsync(m => m.DriverId == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Postback for deleting a driver
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Driver.SingleOrDefaultAsync(m => m.DriverId == id);
            _context.Driver.Remove(driver);
            await _context.SaveChangesAsync();
            TempData["message"] = "Driver deleted.";
            return RedirectToAction("Index");
        }

        private bool DriverExists(int id)
        {
            return _context.Driver.Any(e => e.DriverId == id);
        }
    }
}
