//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WholesaleBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class sales_invoice
    {
        public int ID { get; set; }
        public int OrderNum { get; set; }
        public System.DateTime Date { get; set; }
        public string Buyer { get; set; }
        public int Manager { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal ProductAmount { get; set; }
        public decimal ProductCost { get; set; }
        public decimal TotalCost { get; set; }
    
        public virtual manager manager1 { get; set; }
        public virtual order order { get; set; }
    }
}
