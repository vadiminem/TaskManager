using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Settings;

namespace TaskManager.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private PostgresSettings settings;

        public StorageRepository(PostgresSettings settings)
        {
            this.settings = settings;
        }
    }
}
