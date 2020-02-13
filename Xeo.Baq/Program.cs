using System;
using Autofac;
using Xeo.Baq.Backups;
using Xeo.Baq.Configuration.DependencyInjection;

namespace Xeo.Baq
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (ILifetimeScope scope = Container.Instance.BeginLifetimeScope())
            {
                var backupManger = scope.Resolve<IBackupManager>();

                backupManger.RunAllBackups();
            }

            Console.Read();
        }
    }
}