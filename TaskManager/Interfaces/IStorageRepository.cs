using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface IStorageRepository
    {
        DbTask FindTask(int id);
        User FindUser(string email, string password = null);
        User FindUserByUsername(string username);
        DbTask[] GetTasks();
        int InsertTask(DbTask task);
        int InsertUser(User user);
        int InsertTaskPerformers(TasksPerformersModel taskPerformers);
        void RemoveTask(int id);
        //Status ChangeStatus(int id, Status status);
        void UpdateTask(DbTask task);
    }
}
