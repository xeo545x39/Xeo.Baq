using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using NLog;
using Xeo.Baq.Backups;
using Xeo.Baq.Backups.Actions;
using Xeo.Baq.Backups.Factories;
using Xeo.Baq.Backups.Performers;
using Xeo.Baq.Configuration.Extensions;
using Xeo.Baq.Diagnostics.Computing;
using Xeo.Baq.Exceptions;
using Xeo.Baq.Filtering;
using Xeo.Baq.IO;

namespace Xeo.Baq.Configuration.DependencyInjection
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                    new ConfigurationBuilder()
                        .AddJsonFile(context.Resolve<IConfigurationPathProvider>()
                            .Get())
                        .Build())
                .As<IConfiguration>()
                .SingleInstance();

            builder.RegisterInstance(LogManager.GetLogger("Default"))
                .As<ILogger>()
                .SingleInstance();

            builder.RegisterType<BackupManager>()
                .As<IBackupManager>();

            builder.Register(context =>
                    context.Resolve<IConfiguration>()
                        .GetSettings<ApplicationSettings>())
                .As<ApplicationSettings>()
                .SingleInstance();

            builder.RegisterType<BackupSettingsProvider>()
                .As<IBackupSettingsProvider>()
                .SingleInstance();

            builder.RegisterType<ConfigurationPathProvider>()
                .As<IConfigurationPathProvider>()
                .SingleInstance();

            builder.Register(context => new BaqConfiguration(
                    context.Resolve<IConfiguration>(),
                    context.Resolve<IConfigurationPathProvider>()
                        .Get()))
                .As<IBaqConfiguration>()
                .SingleInstance();

            builder.RegisterType<Crc32CHashGenerator>()
                .As<IHashGenerator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BackupOperationResultBuilder>()
                .As<IBackupOperationResultBuilder>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ParallelFileSystemManipulator>()
                .As<IParallelFileSystemManipulator>();

            builder.RegisterType<DummyBackupOperationResultProvider>()
                .As<IBackupOperationResultProvider>();

            builder.RegisterType<ManagedFileCopier>()
                .As<IFileCopier>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ManagedDirectoryCreator>()
                .As<IDirectoryCreator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BackupEntriesProvider>()
                .As<IBackupEntriesProvider>()
                .SingleInstance();

            builder.RegisterType<FileSystemActionExecutor>()
                .As<IFileSystemActionExecutor>();

            builder.RegisterType<FullBackupPerformer>()
                .AsSelf();

            builder.RegisterType<GenericExceptionHandler>()
                .As<IGenericExceptionHandler>()
                .InstancePerDependency();

            builder.RegisterType<BackupPerformerFactory>()
                .As<IBackupPerformerFactory>()
                .SingleInstance();

            builder.RegisterType<CreateDirectoryAction>()
                .As<ICreateDirectoryAction>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CopyFileAction>()
                .As<ICopyFileAction>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CheckIntegrityAction>()
                .As<ICheckIntegrityAction>()
                .InstancePerLifetimeScope();

            builder.Register<Func<IGenericExceptionHandler>>(contextRoot =>
                {
                    var context = contextRoot.Resolve<IComponentContext>();

                    return () => context.Resolve<IGenericExceptionHandler>();
                })
                .As<Func<IGenericExceptionHandler>>();

            builder.RegisterType<ExceptionHandlerChainBuilder>()
                .As<IExceptionHandlerChainBuilder>()
                .InstancePerDependency();

            builder.Register<Func<IExceptionHandler, IExceptionHandlerChainBuilder>>(contextRoot =>
                {
                    var context = contextRoot.Resolve<IComponentContext>();

                    return exceptionHandler
                        => context.Resolve<IExceptionHandlerChainBuilder>(new TypedParameter(typeof(IExceptionHandler), exceptionHandler));
                })
                .As<Func<IExceptionHandler, IExceptionHandlerChainBuilder>>();

            builder.Register<Func<BackupSettings, IFullBackupPerformer>>(contextRoot =>
                {
                    var context = contextRoot.Resolve<IComponentContext>();

                    return settings
                        => context.Resolve<FullBackupPerformer>(new TypedParameter(typeof(BackupSettings), settings));
                })
                .As<Func<BackupSettings, IFullBackupPerformer>>();
        }
    }
}