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

        public User FindUser(string email, string password)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                if (password == null)
                {
                    var predicate = Predicates.Field<User>(u => u.Email, Operator.Eq, email);
                    var users = connection.GetList<User>(predicate).ToList();
                    return users.Count == 0 ? null : users.First();
                }
                else
                {
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<User>(u => u.Email, Operator.Eq, email));
                    pg.Predicates.Add(Predicates.Field<User>(u => u.Password, Operator.Eq, password));
                    return connection.GetList<User>(pg).First();
                }
            }
        }

        public User FindUserByUsername(string username)
        {
            using (var connection = new NpgsqlConnection(settings.ConnectionString))
            {
                if (username != null)
                {
                    return connection.GetList<User>().Where(u => u.Username == username).First() ?? null;
                }
                else return null;
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
                return connection.Insert(task);
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
