using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAdmin.Model
{
    public class MyDataRow
    {
        public Int64 Id;
        public string Texto;

        public MyDataRow(Int64 id,string texto){
            Id = id;
            Texto = texto;
        }
    }
}
