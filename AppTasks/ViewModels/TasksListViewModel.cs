using System;
using System.Collections.Generic;
using AppTasks.Models;
using AppTasks.Views;
using Xamarin.Forms;

namespace AppTasks.ViewModels
{
    public class TasksListViewModel : BaseViewModel
    {
        static TasksListViewModel instance;

        Command newTaskCommand;
        public Command NewTaskCommand => newTaskCommand ?? (newTaskCommand = new Command(NewTaskAction));

        List<TaskModel> tasks;
        public List<TaskModel> Tasks
        {
            get => tasks;
            set => SetProperty(ref tasks, value);
        }

        TaskModel taskSelected;
        public TaskModel TaskSelected
        {
            get => taskSelected;
            set
            {
                if (SetProperty(ref taskSelected, value))
                {
                    SelectTaskAction();
                }
            }
        }

        public TasksListViewModel()
        {
            instance = this;

            LoadTasks();
        }

        public static TasksListViewModel GetInstance()
        {
            if (instance == null) instance = new TasksListViewModel();
            return instance;
        }

        public async void LoadTasks()
        {
            Tasks = await App.TasksDatabase.GetAllTasksAsync();
        }

        private void NewTaskAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TasksDetailPage());
        }

        private void SelectTaskAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TasksDetailPage(TaskSelected));
        }
    }
}
