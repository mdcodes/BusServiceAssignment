using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2BusService.Models
{
    public class BusServiceContext_Singleton
    {
        private static BusServiceContext context;
        private static object locker = new object();

        public static BusServiceContext Context()
        {
            if(context == null)
            {
                lock (locker)
                {
                    if(context == null)
                    {
                        var optionBuilder = new DbContextOptionsBuilder<BusServiceContext>();

                        optionBuilder.UseSqlServer(
                            @"server=.\sqlexpress;database=BusService;Trusted_Connection=true");
                        context = new BusServiceContext(optionBuilder.Options);
                    }
                }
            }
            return context;
        }
    }
}
