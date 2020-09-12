using JQueryAjaxCoreMVC_CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JQueryAjaxCoreMVC_CRUD
{
    public class TransactionDbContext: DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options): base(options)
        { }
        public DbSet<TransactionModel> Transactions { get; set; }
    }
}
