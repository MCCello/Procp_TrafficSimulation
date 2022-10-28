using System.IO;

namespace ProCPTestAppTiles.orm.dao
{
    public interface IDao<T> where T : ISaveable
    {
        void Save(T o, BinaryWriter writer);
    }
}