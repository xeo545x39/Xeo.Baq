namespace Xeo.Baq.Backups.Performers
{
    public interface IBackupPerformer
    {
        void Perform(bool force = false);
    }
}