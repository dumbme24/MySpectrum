using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySpectrum.DataModel;
using Android;
using MySpectrum.DataAccessLayer;

namespace MySpectrum.Droid
{
    [Activity(Label = "Users List", MainLauncher = true, Icon = "@drawable/spectrum")]
    public class UserDetailsActivity : Activity
    {
        List<User> users;
        Toolbar toolbarUserDetails;
        ListView listviewUserDetails;
        Button clearListButton;

        static string database_Name = "UserDB.sqlite";
        static string folder_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        string database_Path = System.IO.Path.Combine(folder_Path, database_Name);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserDetailList);

            toolbarUserDetails = FindViewById<Toolbar>(Resource.Id.toolbarUserDetails);
            listviewUserDetails = FindViewById<ListView>(Resource.Id.listViewUserDetails);
            clearListButton = FindViewById<Button>(Resource.Id.ButtonClearList);

            //SetActionBar(toolbarUserDetails);
            //ActionBar.Title = "User Details";            

            clearListButton.Click += ClearButton_Click;
         
            // Create your application here
            users = new List<User>();
            users = MySpectrum.DataAccessLayer.LocalDatabaseHelper.ReadData(database_Path);

            var arrayAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, users);
            //this.ListAdapter = arrayAdapter;
            listviewUserDetails.Adapter = arrayAdapter;


        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            if(LocalDatabaseHelper.DeletData(database_Path))
            {
                Toast.MakeText(this, "Users list has been cleared", ToastLength.Short).Show();
            }

            PopulateUsersList();
        }


        protected void PopulateUsersList()
        {
            users = MySpectrum.DataAccessLayer.LocalDatabaseHelper.ReadData(database_Path);
            var arrayAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, users);
            listviewUserDetails.Adapter = arrayAdapter;
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            PopulateUsersList();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //Resource.
            //MenuInflater.Inflate(Resource.Menu.userMenu, menu);
            MenuInflater.Inflate(Resource.Menu.usersMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.TitleFormatted.ToString() == "New User")
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}