
namespace Theblueway.Core.Editor.SerializedInstanceMigrationTools
{
    public interface IMigratable
    {
        int Version { get; set; }
        int LatestVersion { get; }
        void Migrate(int fromVersion);
    }

    public class ExampleIMigrateable : IMigratable
    {
        public int Version { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public int LatestVersion => throw new System.NotImplementedException();

        public void Migrate(int fromVersion)
        {
            if(fromVersion < 1) MigrateV1();
            if(fromVersion < 2) MigrateV2();
        }

        public void MigrateV1()
        {

        }

        public void MigrateV2()
        {

        }
    }
}
