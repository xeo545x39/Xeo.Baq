using System;
using Autofac;
using Xeo.Baq.Configuration.DependencyInjection;

namespace Xeo.Baq
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (IContainer scope = Container.Instance)
            {
                var backupManger = scope.Resolve<IBackupManager>();

                backupManger.RunBackups();
            }

            Console.Read();
        }
    }
}