namespace Xeo.Baq.Backups
{
    public interface IBackupPerformer
    {
        void Perform(bool force = false);
    }
}