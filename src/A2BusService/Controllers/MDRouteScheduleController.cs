/* Name: Michal Drahorat
 * Project: Bus Service Assignment 3
 * Revision History
 * 10/24/2016 - Finalized and submitted
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using A2BusService.Models;
using Microsoft.AspNetCore.Http;

namespace A1BusService.Controllers
{
    public class MDRouteScheduleController : Controller
    {
        private readonly BusServiceContext _context;

        public MDRouteScheduleController(BusServiceContext context)
        {
            _context = context;    
        }

        // GET: MDRouteSchedule
        public async Task<IActionResult> Index(string BusRouteCode, string routeName)
        {
            if(BusRouteCode != null)
            {
                HttpContext.Session.SetString(nameof(BusRouteCode), BusRouteCode);
                HttpContext.Session.SetString(nameof(routeName), routeName);

            }
            else if(HttpContext.Session.GetString(nameof(BusRouteCode)) != null)
            {
                BusRouteCode = HttpContext.Session.GetString("BusRouteCode");
                routeName = HttpContext.Session.GetString("routeName");
            }
            else
            {
                TempData["message"] = "You must select a bus route to view its schedule.";
                RedirectToAction(actionName: "Index", controllerName: "MDBusRoute");
            }

            ViewBag.BusRouteCode = BusRouteCode;
            ViewBag.routeName = routeName;

            var busServiceContext = _context.RouteSchedule.Include(r => r.BusRouteCodeNavigation)
                .Where(a => a.BusRouteCode == BusRouteCode)
                .OrderBy(a => a.IsWeekDay)
                .OrderBy(a => !a.IsWeekDay) ;
            return View(await busServiceContext.ToListAsync());
        }

        public async Task<IActionResult> RouteStopSchedule(int routeStopId, string BusStopNumber, string location, string offsetMinutes, string busRouteCode)
        {
            if (routeStopId == 0 || !ModelState.IsValid)
            {
                TempData["message"] = "Please select a stop to see its schedule.";
                RedirectToAction(actionName: "Index", controllerName: "MDRouteStop");
            }
            var selectedRoute = _context.RouteSchedule
                .Include(r => r.BusRouteCodeNavigation)
                .Where(a => a.RouteScheduleId == routeStopId);
                

            if (selectedRoute.Any())
            {
                HttpContext.Session.SetString("BusStopNumber", BusStopNumber);
                HttpContext.Session.SetString("location", location);
                HttpContext.Session.SetString("offsetMinutes", offsetMinutes);
            }
            else
            {
                HttpContext.Session.GetString("BusStopNumber");
                HttpContext.Session.GetString("location");
                HttpContext.Session.GetString("offsetMinutes");

                TempData["message"] = "No schedule records for the selected route.";
            }

            return View(await selectedRoute.ToListAsync());
        }

        // GET: MDRouteSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeSchedule = await _context.RouteSchedule.SingleOrDefaultAsync(m => m.RouteScheduleId == id);
            if (routeSchedule == null)
            {
                return NotFound();
            }

            return View(routeSchedule);
        }

        // GET: MDRouteSchedule/Create
        public IActionResult Create()
        {
            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode");
            return View();
        }

        // POST: MDRouteSchedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteScheduleId,BusRouteCode,Comments,IsWeekDay,StartTime")] RouteSchedule routeSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(routeSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode", routeSchedule.BusRouteCode);
            return View(routeSchedule);
        }

        // GET: MDRouteSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeSchedule = await _context.RouteSchedule.SingleOrDefaultAsync(m => m.RouteScheduleId == id);
            if (routeSchedule == null)
            {
                return NotFound();
            }
            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode", routeSchedule.BusRouteCode);
            return View(routeSchedule);
        }

        // POST: MDRouteSchedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteScheduleId,BusRouteCode,Comments,IsWeekDay,StartTime")] RouteSchedule routeSchedule)
        {
            if (id != routeSchedule.RouteScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(routeSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteScheduleExists(routeSchedule.RouteScheduleId))
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
            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode", routeSchedule.BusRouteCode);
            return View(routeSchedule);
        }

        // GET: MDRouteSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeSchedule = await _context.RouteSchedule.SingleOrDefaultAsync(m => m.RouteScheduleId == id);
            if (routeSchedule == null)
            {
                return NotFound();
            }

            return View(routeSchedule);
        }

        // POST: MDRouteSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routeSchedule = await _context.RouteSchedule.SingleOrDefaultAsync(m => m.RouteScheduleId == id);
            _context.RouteSchedule.Remove(routeSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RouteScheduleExists(int id)
        {
            return _context.RouteSchedule.Any(e => e.RouteScheduleId == id);
        }
    }
}
