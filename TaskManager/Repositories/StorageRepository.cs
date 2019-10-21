using DapperExtensions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Models;
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

        public DbTask FindTask(int id)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                return connection.Get<DbTask>(id);
            }
        }

        public User FindUser(string email, string password = null)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                if (password != null)
                    return connection.Get<User>(new { email, password });
                else
                    return connection.Get<User>(email);
            }
        }

        public DbTask[] GetTasks()
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                return connection.GetList<DbTask>().OrderBy(t => t.ParentId)
                .ThenByDescending(t => t.RegistrationDate).ToArray();
            }
        }

        public int InsertTask(DbTask task)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                return connection.Insert<DbTask>(task);
            }
        }

        public int InsertTaskPerformers(TasksPerformersModel taskPerformers)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                return connection.Insert(taskPerformers);
            }
        }

        public int InsertUser(User user)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                return connection.Insert(user);
            }
        }

        public void RemoveTask(int id)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                connection.Delete<DbTask>(id);
            }
        }

        public void UpdateTask(DbTask task)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                connection.Update(task);
            }
        }
    }
}
