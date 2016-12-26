using System;
using System.ComponentModel.DataAnnotations;

namespace Sarwa.Host.WebApi.Entities
{
    public partial class Expense
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public int ExpenseGroupId { get; set; }

        public virtual ExpenseGroup ExpenseGroup { get; set; }

    }
}
