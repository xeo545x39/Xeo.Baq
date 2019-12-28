using System;
using Autofac;

namespace Xeo.Baq.Configuration.DependencyInjection
{
    public static class Container
    {
        private static readonly Lazy<IContainer> ContainerLazy = new Lazy<IContainer>(Create);
        public static IContainer Instance => ContainerLazy.Value;

        private static IContainer Create()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<MainModule>();

            return containerBuilder.Build();
        }
    }
}