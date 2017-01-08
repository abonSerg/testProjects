using Ninject.Extensions.Conventions;
using IoCExample.Core;
using Ninject.Modules;

namespace IoCExample.Ninject
{
    public class CustomNinjectModule : NinjectModule
    {
        public override void Load()
        {
            //Using Ninject extension - in case 1 interface to 1 type
            Kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());

            Bind<IPrinter>().To<LocalPrinter>();

            Kernel.Bind<IPrinter>().To<LocalPrinter>().WhenInjectedInto<PrivatePrinterProvider>();
            Kernel.Bind<IPrinter>().To<PublicPrinter>().WhenInjectedInto<PublicPrinterProvider>();
        }
    }
}
