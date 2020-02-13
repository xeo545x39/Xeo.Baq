using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NLog;
using Xeo.Baq.Backups.Factories;
using Xeo.Baq.Backups.Performers;
using Xeo.Baq.Backups.Types;
using Xeo.Baq.Configuration;
using Xeo.Baq.Diagnostics;
using Xeo.Baq.Extensions;

namespace Xeo.Baq.Backups
{
    public interface IBackupManager
    {
        void RunAllBackups();
        void RunBackup(Guid id);
    }

    public class BackupManager : IBackupManager
    {
        private readonly IBackupPerformerFactory _backupPerformerFactory;
        private readonly IEnumerable<BackupSettings> _backupSettingsList;
        private readonly ILogger _logger;
        private readonly ILifetimeScope _parentScope;

        public BackupManager(IBackupSettingsProvider backupSettingsProvider,
            ILogger logger,
            IBackupPerformerFactory backupPerformerFactory,
            ILifetimeScope parentScope)
        {
            _backupSettingsList = backupSettingsProvider.Load();
            _logger = logger;
            _backupPerformerFactory = backupPerformerFactory;
            _parentScope = parentScope;
        }

        public void RunAllBackups()
        {
            foreach (BackupSettings settings in _backupSettingsList)
            {
                RunInternal(settings);
            }
        }

        public void RunBackup(Guid id)
        {
            BackupSettings settings = _backupSettingsList.SingleOrDefault(x => x.Id == id);
            if (settings == null)
            {
                throw new InvalidOperationException($"There is no backup with Id = \"{id}\".");
            }

            RunInternal(settings);
        }

        private void RunInternal(BackupSettings settings)
        {
            var type = settings.BackupType.Instantiate<IBackupType>();
            Func<BackupSettings, IBackupPerformer> factory = _backupPerformerFactory.GetFactoryByBackupType(type);
            IBackupPerformer performer = factory(settings);
            TimeSpan duration = default;
            BackupOperationResult result;

            _logger.Info($"Started performing backup with Id = \"{settings.Id}\".");

            using (ILifetimeScope scope = _parentScope.BeginLifetimeScope())
            {
                IBackupOperationResultBuilder resultBuilder = scope.Resolve<IBackupOperationResultBuilder>()
                    .SetStartTime(DateTime.Now);

                try

                {
                    Measured.Run(() =>
                            performer.Perform(),
                        out duration);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Fatal error occurred while performing backup with Id = \"{settings.Id}\".");

                    throw;
                }
                finally
                {
                    result = resultBuilder.SetEndTime(DateTime.Now)
                        .SetDuration(duration)
                        .Build();

                    _logger.Info($"Finished performing backup with Id = \"{settings.Id}\". Duration: {result.Duration:g} (ms: {result.Duration.TotalMilliseconds}), start time: {result.TimeStarted}, end time: {result.TimeEnded}, errors: {result.Exceptions.Count()}");
                }
            }
        }
    }
}