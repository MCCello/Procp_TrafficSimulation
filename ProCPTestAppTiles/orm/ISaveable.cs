using System.IO;

namespace ProCPTestAppTiles.orm
{
    public interface ISaveable
    {
        void Save(BinaryWriter writer);
    }
}