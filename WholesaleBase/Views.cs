using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WholesaleBase
{
    static class Views
    {
        public static CollectionViewSource ProductsView { get; set; }
        public static CollectionViewSource CategoryView { get; set; }
        public static CollectionViewSource UnitsView { get; set; }
        public static CollectionViewSource ManagersView { get; set; }
        public static CollectionViewSource BuyersView { get; set; }
    }
}
