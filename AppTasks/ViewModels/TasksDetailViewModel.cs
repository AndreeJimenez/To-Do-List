using System;
using AppTasks.Models;
using AppTasks.Services;
using Xamarin.Forms;

namespace AppTasks.ViewModels
{
    public class TasksDetailViewModel : BaseViewModel
    {
        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));

        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));

        Command cancelCommand;
        public Command CancelCommand => cancelCommand ?? (cancelCommand = new Command(CancelAction));

        TaskModel taskSelected;
        public TaskModel TaskSelected
        {
            get => taskSelected;
            set => SetProperty(ref taskSelected, value);
        }

        ImageSource imageSource_;
        public ImageSource ImageSource_
        {
            get => imageSource_;
            set => SetProperty(ref imageSource_, value);
        }

        public TasksDetailViewModel()
        {
            TaskSelected = new TaskModel();
        }

        public TasksDetailViewModel(TaskModel taskSelected)
        {
            if(!string.IsNullOrEmpty(taskSelected.ImageBase64))
            {
                ImageSource_ = new ImageService().ConvertImageFromBase64ToImageSource(taskSelected.ImageBase64);
            }
            TaskSelected = taskSelected;
        }

        private async void SaveAction()
        {
            if (!string.IsNullOrEmpty(TaskSelected.ImageUrl))
            {
                TaskSelected.ImageBase64 = await new ImageService().DownloadImageAsBase64Async(TaskSelected.ImageUrl);
            }
            await App.TasksDatabase.SaveTaskAsync(TaskSelected);
            TasksListViewModel.GetInstance().LoadTasks();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            await App.TasksDatabase.DeleteTaskAsync(TaskSelected);
            TasksListViewModel.GetInstance().LoadTasks();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
