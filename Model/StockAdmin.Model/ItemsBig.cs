//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockAdmin.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemsBig
    {
        public int ID_Order { get; set; }
        public int ID_Product { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual OrdersBig OrdersBig { get; set; }
    }
}
