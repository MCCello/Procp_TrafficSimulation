using System;
using System.Diagnostics;
using System.IO;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;
using Queue = ProCPTestAppTiles.simulation.Queue;

namespace ProCPTestAppTiles.orm.dao
{
    public class QueueDao : IDao<Queue>
    {
        private PathDao _pathDao = (PathDao) DaoFactory.GetByType<Path>();
        
        public Queue Load(SimulationMap simulationMap, BinaryReader reader)
        {
            Queue queue = null;
            try
            {
                queue = new Queue();

                var queueCount = reader.ReadInt32();
                for (int i = 0; i < queueCount; i++)
                {
                    var startingPath = _pathDao.GetByObjectId(simulationMap, reader.ReadInt32());
                    var endingPath = _pathDao.GetByObjectId(simulationMap, reader.ReadInt32());
                    
                    queue.Add(startingPath, endingPath);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return queue;
        }
        
        public void Save(Queue o, BinaryWriter writer)
        {
            writer.Write(o.Count());
            foreach (var pathTuple in o.queue)
            {
                writer.Write(pathTuple.Item1.objectId); // starting Path
                writer.Write(pathTuple.Item2.objectId); // ending Path
            }
        }
    }
}