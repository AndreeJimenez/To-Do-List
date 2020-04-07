using AppTasks.Extensions;
using AppTasks.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTasks.Data
{
    public class TasksDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() => {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;

        static bool initialized = false;

        public TasksDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }


        // Método para inicializar la table de tasks, si no existe la crea
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(TaskModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(TaskModel)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<TaskModel>> GetAllTasksAsync()
        {
            return Database.Table<TaskModel>().ToListAsync();
        }

        public Task<List<TaskModel>> GetTasksNotDoneAsync()
        {
            return Database.QueryAsync<TaskModel>($"SELECT * FROM [{typeof(TaskModel).Name}] WHERE [Done] = 0");
        }

        public Task<TaskModel> GetTaskAsync(int id)
        {
            return Database.Table<TaskModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveTaskAsync(TaskModel item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteTaskAsync(TaskModel item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
