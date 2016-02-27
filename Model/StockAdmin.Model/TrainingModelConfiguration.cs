using StockAdmin.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAdmin.Model
{
    public class TrainingModelConfiguration : DbConfiguration
    {
#if INTERCEPTOR_ON
        public TrainingModelConfiguration()
        {
            this.AddInterceptor(new NLogCommandInterceptor());
        }
#endif
    }
}
