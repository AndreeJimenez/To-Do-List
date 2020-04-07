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
            // De la imagen guardada en SQLite en formato base 64, hacemos la conversión para visualizarla en imagen
            if(!string.IsNullOrEmpty(taskSelected.ImageBase64))
            {
                ImageSource_ = new ImageService().ConvertImageFromBase64ToImageSource(taskSelected.ImageBase64);
            }

            TaskSelected = taskSelected;
        }

        private async void SaveAction()
        {
            // Descargamos la imagen con la URL tecleada y la convertimos a base 64 para que se guarde en SQLite
            if (!string.IsNullOrEmpty(TaskSelected.ImageUrl))
            {
                TaskSelected.ImageBase64 = await new ImageService().DownloadImageAsBase64Async(TaskSelected.ImageUrl);
            }
            // Guardamos la tarea en SQLite
            await App.TasksDatabase.SaveTaskAsync(TaskSelected);
            // Refescamos las tareas en el listado
            TasksListViewModel.GetInstance().LoadTasks();
            // Cerramos la ventana
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            // Borramos la tarea en SQLite
            await App.TasksDatabase.DeleteTaskAsync(TaskSelected);
            // Refescamos las tareas en el listado
            TasksListViewModel.GetInstance().LoadTasks();
            // Cerramos la ventana
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
