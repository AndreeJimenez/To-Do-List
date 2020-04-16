using System;
using AppTasks.Models;
using AppTasks.Services;
using Plugin.Media;
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

        Command _TakePictureCommand;
        public Command TakePictureCommand => _TakePictureCommand ?? (_TakePictureCommand = new Command(TakePictureAction));

        Command _SelectPictureCommand;
        public Command SelectPictureCommand => _SelectPictureCommand ?? (_SelectPictureCommand = new Command(SelectPictureAction));

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

        string _ImageUrl;
        public string ImageUrl
        {
            get => _ImageUrl;
            set => SetProperty(ref _ImageUrl, value);
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

        private async void TakePictureAction()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            ImageUrl = file.Path;
            await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

            ImageSource_ = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private async void SelectPictureAction()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Not supported", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (file == null)
                return;

            ImageUrl = file.Path;

            ImageSource_ = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }
    }
}
