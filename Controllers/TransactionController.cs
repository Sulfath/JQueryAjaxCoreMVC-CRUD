﻿using JQueryAjaxCoreMVC_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JQueryAjaxCoreMVC_CRUD.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionDbContext _context;

        public TransactionController(TransactionDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transactions.ToListAsync());
        }



        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount,Date")] TransactionModel transactionModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(transactionModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(transactionModel);
        //}

        // GET: Transaction/AddOrEdit/5
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new TransactionModel());
            else
            {
                var transactionModel = await _context.Transactions.FindAsync(id);
                if (transactionModel == null)
                {
                    return NotFound();
                }
                return View(transactionModel);
            }
        }

        // POST: Transaction/AddOrEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount,Date")] TransactionModel transactionModel)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _context.Add(transactionModel);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    try
                    {
                        _context.Update(transactionModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(transactionModel.TransactionId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return Json(new { IsValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()) });
            }
            return Json(new { IsValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", transactionModel) });
        }


        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionModel = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transactionModel);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()) });
        }

        private bool TransactionModelExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
