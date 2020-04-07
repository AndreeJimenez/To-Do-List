using System;
using AppTasks.Data;
using AppTasks.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTasks
{
    public partial class App : Application
    {
        static TasksDatabase tasksDatabase;
        public static TasksDatabase TasksDatabase
        {
            get
            {
                if (tasksDatabase == null) tasksDatabase = new TasksDatabase();
                return tasksDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            var nav = new NavigationPage(new TasksListPage());
            nav.BarBackgroundColor = (Color)App.Current.Resources["primaryDarkGreen"];
            nav.BarTextColor = Color.White;
            MainPage = nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
