using AppTasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTasks.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksListPage : ContentPage
    {
        public TasksListPage()
        {
            InitializeComponent();

            BindingContext = new TasksListViewModel();
        }
    }
}