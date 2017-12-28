using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MySpectrum.DataAccessLayer;
using MySpectrum.DataModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace MySpectrum.Droid
{
	[Activity (Label = "MySpectrum", MainLauncher = false, Icon = "@drawable/spectrum")]
	public class MainActivity : Activity
	{
        EditText firstNameEditText, lastNameEditText, genderEditText, ageEditText, userNameEditText, userPasswordEditText;
        Button saveButton;

        static string database_Name = "UserDB.sqlite";
        static string folder_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        string database_Path = System.IO.Path.Combine(folder_Path, database_Name);
        List<User> users;

        protected override void OnCreate (Bundle bundle)
		{
            //SetTheme(Resource.Style.AppThemeLaunch);
            base.OnCreate (bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            firstNameEditText = FindViewById<EditText>(Resource.Id.EditTextFirstName);
            lastNameEditText = FindViewById<EditText>(Resource.Id.EditTextLastName);
            genderEditText = FindViewById<EditText>(Resource.Id.EditTextGender);
            ageEditText = FindViewById<EditText>(Resource.Id.EditTextAge);
            userNameEditText = FindViewById<EditText>(Resource.Id.EditTextUserName);
            userPasswordEditText = FindViewById<EditText>(Resource.Id.EditTextUserPassworrd);

            saveButton = FindViewById<Button>(Resource.Id.ButtonSave);
            saveButton.Click += SaveButton_Click;
        }
       
        private void SaveButton_Click(object sender, EventArgs e)
        {
            User newUser = new User(firstNameEditText.Text, lastNameEditText.Text,
                                    genderEditText.Text, ageEditText.Text,
                                    userNameEditText.Text, userPasswordEditText.Text);


            if(FieldValidation(newUser))
            { 
                if (LocalDatabaseHelper.InsertData(ref newUser, database_Path))
                {
                    Toast.MakeText(this, "New user has been created", ToastLength.Short).Show();
                    this.StartActivity(typeof(UserDetailsActivity));
                }
                else
                {
                    Toast.MakeText(this, "Unable to create a new user", ToastLength.Short).Show();
                }
            }
        }

        private bool FieldValidation(User newUser)
        {
            if(!isValidInput(newUser.FirstName, "First Name"))
            {
                return false;
            }

            if(!isValidInput(newUser.LastName, "Last Name"))
            {
                return false;
            }
           
            if (!isValidInput(newUser.Gender, "Gender"))
            {
                return false;
            }

            if (!isValidInput(newUser.Age, "Age"))
            {
                return false;
            }

            if (!isValidInput(newUser.UserName, "UserName"))
            {
                return false;
            }
            else if (UserNameExist(newUser.UserName))
            {
                return false;
            }

            if (!ValidatePasswordString(newUser.UserPassworrd, "Password"))
            {
                Toast.MakeText(this, "Please enter the valid Password ", ToastLength.Short).Show();
                return false;
            }

            else
            { return true; }
            
        }

        private bool isValidInput(string input, string messgaField)
        {
            if (input.Length > 0)
            {
                return true;
            }
            else
            {
                Toast.MakeText(this, "Please enter the valid " + messgaField, ToastLength.Short).Show();
                return false;
            }
        }

        protected bool ValidatePasswordString(string userPassword, string messageField)
        {
            if (userPassword.Length > 0)
            {
                //string validationPattern = @"^[a-zA-Z][a-zA-Z0-9]*$";
                string validationPattern = @"^(?=.*\d)(?=.*[a-zA-Z]).{5,12}$";

                var regex = new Regex(validationPattern);
                bool regexValidation = regex.IsMatch(userPassword);

                //if above regexValidation validation is true then check for the consecutive character sequence
                if (regexValidation)
                {
                    if (StringHasConsecutiveCharacters(userPassword, 2))
                    {
                        ToastMessage("Password has a consecutive sequence of characters.");
                        return false;
                    }
                    else
                    {
                        return regexValidation; //this should be true
                    }
                }
                else
                {
                    return false;
                }

            }
          
            else
                return false;
        }

        protected bool UserNameExist(string userName)
        {
            users = new List<User>();
            users = MySpectrum.DataAccessLayer.LocalDatabaseHelper.ReadData(database_Path);

            foreach (var user in users)
            {
                if(user.UserName == userName)
                {
                    //UserName already exist.
                    ToastMessage("Username " + userName + " already exist.");
                    return true;
                }
            }

            return false;
        }

        protected void ToastMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        protected bool StringHasConsecutiveCharacters(string source , int sequenceLength)
        {
            return Regex.IsMatch(source, "([a-zA-Z0-9])\\1{" + (sequenceLength - 1) + "}");
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.userDetailsMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //if (item.TitleFormatted.ToString() == "Users List")
            {
                StartActivity(typeof(MySpectrum.Droid.UserDetailsActivity));
            }
            return base.OnOptionsItemSelected(item);
        }


    }
}


