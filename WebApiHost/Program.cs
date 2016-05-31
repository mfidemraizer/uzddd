using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace WebApiHost
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<Service>(s =>                        //2
                {
                    s.ConstructUsing(name => new Service());     //3
                    s.WhenStarted(service => service.OnStart());              //4
                    s.WhenStopped(service => service.OnStop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("UZ WebAPI");        //7
                x.SetDisplayName("UZ WebAPI");                       //8
                x.SetServiceName("UZ.WebAPI");                       //9
            });
        }
    }
}
