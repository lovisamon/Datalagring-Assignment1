using App.Models;
using App.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            this.InitializeComponent();
        }

        private void btnNavigateToMainPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();

            // Convert user input to a person object
            try
            {
                person.FirstName = this.tbxFirstName.Text;
                person.LastName = this.tbxLastName.Text;
                person.Age = Int32.Parse(this.tbxAge.Text);
                person.City = this.tbxCity.Text;
            }
            catch
            {
                var messageDialog = new MessageDialog("Invalid data, try again.");
                await messageDialog.ShowAsync();
                return;
            }
            

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add(".json", new List<string>() { ".json" });
            savePicker.FileTypeChoices.Add(".csv", new List<string>() { ".csv" });
            savePicker.FileTypeChoices.Add(".xml", new List<string>() { ".xml" });
            savePicker.FileTypeChoices.Add(".txt", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                WriteService.WriteToFile(file, person);

                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    this.tbStatus.Text = "File " + file.Name + " was saved.";
                }
                else
                {
                    this.tbStatus.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                this.tbStatus.Text = "Operation cancelled.";
            }
        }
    }

}
