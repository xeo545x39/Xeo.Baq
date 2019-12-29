using System;
using System.IO;
using Autofac;
using NCrontab;
using NLog;
using Xeo.Baq.Backups;
using Xeo.Baq.Filtering;
using Xeo.Baq.IO;

namespace Xeo.Baq.Configuration.DependencyInjection
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(LogManager.GetLogger("Default"))
                .As<ILogger>();

            builder.RegisterType<BackupManager>()
                .As<IBackupManager>();

            builder.RegisterInstance(new ApplicationSettings
                {
                    RegexTimeoutSeconds = 1,
                    ParallelOperationMaxItems = 50
                })
                .AsSelf();
            
            builder.RegisterInstance(new BackupSettings
                {
                    Id = Guid.NewGuid(),
                    Destination = @"F:\Xeo.Baq\Destination",
                    Schedule = CrontabSchedule.Parse("0 0 * * *"),
                    Filters = new[]
                    {
                        new FileSystemFilter
                        {
                            Attributes = new[] { FileAttributes.Archive, FileAttributes.Normal, FileAttributes.Directory },
                            Path = @"F:\Xeo.Baq\Source\*",
                            EntryType = FileSystemEntryType.Directory | FileSystemEntryType.File,
                            NameMask = "*"
                        }
                    },
                    BackupType = typeof(FullBackupPerformer)
                })
                .AsSelf();

            builder.RegisterType<ParallelFileSystemManipulator>()
                .As<IParallelFileSystemManipulator>();

            builder.RegisterType<DummyBackupOperationResultProvider>()
                .As<IBackupOperationResultProvider>();

            builder.RegisterType<ManagedFileCopier>()
                .As<IFileCopier>();

            builder.RegisterType<ManagedDirectoryCreator>()
                .As<IDirectoryCreator>();

            builder.RegisterType<BackupEntriesProvider>()
                .As<IBackupEntriesProvider>();

            builder.RegisterType<FileSystemActionExecutor>()
                .As<IFileSystemActionExecutor>();

            builder.RegisterType<FullBackupPerformer>()
                .AsSelf();

            builder.Register<Func<BackupSettings, FullBackupPerformer>>(contextRoot =>
                {
                    var context = contextRoot.Resolve<IComponentContext>();

                    return settings => context.Resolve<FullBackupPerformer>(new TypedParameter(typeof(BackupSettings), settings));
                })
                .As<Func<BackupSettings, FullBackupPerformer>>();
        }
    }
}