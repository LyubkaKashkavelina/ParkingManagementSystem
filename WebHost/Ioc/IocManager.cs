namespace WebHost.Ioc
{
    using Autofac;
    using AutoMapper;
    using System.Collections.Generic;
    using WebHost.Data;
    using WebHost.Mapping;

    public static class IocManager
    {
        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="T">
        /// T is used for Repository
        /// </typeparam>
        /// <typeparam name="M">
        /// M is used for Interface
        /// </typeparam>
        /// <returns></returns>
        public static M RegisterType<T, M>()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<T>().As<M>();
            containerBuilder.RegisterType<ParkingManagementSystemEntities>().AsSelf().InstancePerLifetimeScope();

            var container = containerBuilder.Build();

            return container.Resolve<M>();
        }

        public static IMapper RegisterMapper()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<MappingProfile>().As<Profile>();

            containerBuilder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            containerBuilder.Register(
                c => c.Resolve<MapperConfiguration>()
                .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            var container = containerBuilder.Build();

            return container.Resolve<IMapper>();
        }
    }
}
