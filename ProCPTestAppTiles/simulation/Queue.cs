using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.simulation
{
    public class Queue : ISaveable
    {
        private static QueueDao _queueDao = (QueueDao) DaoFactory.GetByType<Queue>();
        
        public List<Tuple<Path, Path>> queue { get; set; }

        public Queue()
        {
            queue = new List<Tuple<Path, Path>>();
        }

        public void Add(Path startingPath, Path endingPath)
        {
            queue.Add(Tuple.Create(startingPath, endingPath));
        }

        public void Remove(Tuple<Path, Path> tuple)
        {
            queue.Remove(tuple);
        }
        
        public void Remove(Path startingPath, Path endingPath)
        {
            var path = queue.First(t => t.Item1 == startingPath && t.Item2 == endingPath);
            if (path == null)
            {
                return;
            }
            queue.Remove(path);
        }

        public int Count()
        {
            return queue.Count;
        }

        public Tuple<Path, Path> GetFirst()
        {
            if (queue == null || queue.Count == 0)
            {
                return null;
            }

            return queue[0];
        }

        public void Save(BinaryWriter writer)
        {
            _queueDao.Save(this, writer);
        }
    }
}