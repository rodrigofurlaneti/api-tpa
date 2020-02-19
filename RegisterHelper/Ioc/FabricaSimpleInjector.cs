using Aplicacao;
using CommonServiceLocator.SimpleInjectorAdapter;
using Dominio;
using Entidade.Base;
using Microsoft.Practices.ServiceLocation;
using Repositorio;
using Repositorio.Base;
using System.Linq;
using System.Reflection;
using Container = SimpleInjector.Container;

namespace InitializerHelper.Ioc
{
    public class FabricaSimpleInjector
    {
        public static void Registrar(Container container)
        {
            RegistrarNhibernate(container);
            RegistrarRepositorios(container);
            RegistrarServicos(container);
            RegistrarAplicacao(container);
            
            ServiceLocator.SetLocatorProvider(() => new SimpleInjectorServiceLocatorAdapter(container));
        }

        private static void RegistrarNhibernate(Container container)
        {
            container.Register<NHibContext, FluentNHibContext>();
        }

        private static void RegistrarAplicacao(Container container)
        {
            var repositoryAssembly = typeof(CidadeAplicacao).Assembly;

            //container.Register(typeof(Aplicacao.Base.IBaseAplicacao<>), typeof(Aplicacao.Base.BaseAplicacao<,>));

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "Aplicacao"
                where type.Name.Contains("Aplicacao")
                where type.IsClass
                where type.GetInterfaces().Length > 0
                select new
                {
                    Service = type.GetInterfaces().First(x => x.Name.Equals("I" + type.Name)),
                    Implementation = type
                };

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation);
            }
        }

        private static void RegistrarServicos(Container container)
        {
            var repositoryAssembly = typeof(CidadeServico).Assembly;

            //container.Register(typeof(Dominio.Base.IBaseServico<>), typeof(Dominio.Base.BaseServico<,>));
            
            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "Dominio"
                where type.Name.Contains("Servico")
                where type.IsClass
                where type.GetInterfaces().Length > 0
                select new
                {
                    Service = type.GetInterfaces().First(x => x.Name.Equals("I" + type.Name)),
                    Implementation = type
                };

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation);
            }
        }

        private static void RegistrarRepositorios(Container container)
        {
            var repositoryAssembly = typeof(CidadeRepositorio).Assembly;

            //container.Register(typeof(Dominio.IRepositorio.Base.IRepository<>), typeof(NHibRepository<>));

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "Repositorio"
                where type.Name.Contains("Repositorio")
                where type.IsClass
                where type.GetInterfaces().Length > 0
                select new
                {
                    Service = type.GetInterfaces().First(x => x.Name.Equals("I" + type.Name)),
                    Implementation = type
                };

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation);
            }
        }

        //private static void RegisterGenerics(Container container)
        //{
        //    var repositoryAssembly = Assembly.Load("Entidade");

        //    var registrations =
        //        from type in repositoryAssembly.GetExportedTypes()
        //        where type.Namespace == "Entidade"
        //        where type.IsClass
        //        where type.GetInterfaces().Length > 0
        //        where type.IsAssignableFrom(typeof(IEntity))
        //        where !type.Name.Contains("Base") && !type.Name.Contains("Audit")
        //        select new
        //        {
        //            Entity = type
        //        };

        //    foreach (var reg in registrations)
        //    {
        //        var repositoryType = typeof(NHibRepository<>).MakeGenericType(reg.Entity);
        //        var serviceType = typeof(Dominio.Base.IBaseServico<>).MakeGenericType(reg.Entity);
        //        var concreteType = typeof(Dominio.Base.BaseServico<,>).MakeGenericType(reg.Entity, ) 
        //        container.Register(serviceType, 
        //    }
        //}
    }
}