using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ProCPTestAppTiles.simulation;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;

namespace ProCPTestAppTiles.orm.dao
{
    public class SimulationDao : IDao<Simulation>
    {
        private static SimulationMapDao _simulationMapDao = (SimulationMapDao) DaoFactory.GetByType<SimulationMap>();
        private static QueueDao _queueDao = (QueueDao) DaoFactory.GetByType<Queue>();
        
        public Simulation Load(BinaryReader reader)
        {
            Simulation simulation = null;
            try
            {
                simulation = new Simulation
                {
                    Location = new Point(reader.ReadInt32(), reader.ReadInt32()),
                    Size = new Size(reader.ReadInt32(), reader.ReadInt32())
                };
                
                // simulationMap
                _simulationMapDao.Load(reader, simulation);


                simulation.Init();
                
                // Queue
                simulation.queue = _queueDao.Load(simulation.simulationMap, reader);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return simulation;
        }

        public void Save(Simulation o, BinaryWriter writer)
        {
            // Control
                // Location
            writer.Write(o.Location.X);
            writer.Write(o.Location.Y);
            
                // Size
            writer.Write(o.Size.Height);
            writer.Write(o.Size.Width);
            
            
            // Simulation
            o.simulationMap.Save(writer);
            
            // Queue
            o.queue.Save(writer);
        }
    }
}