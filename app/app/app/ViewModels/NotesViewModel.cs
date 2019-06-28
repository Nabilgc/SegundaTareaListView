using System;
using System.Collections.Generic;
using System.Text;

namespace app.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class NotesViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Notes> notas;
        private bool isRefreshing;
        private string filter;
        private List<Notes> notasList;
        #endregion

        #region Properties
        public ObservableCollection<Notes> Notas
        {
            get { return this.notas; }
            set { SetValue(ref this.notas, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }
        #endregion

        #region Constructors
        public NotesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadNotes();
        }
        #endregion

        #region Methods
        private async void LoadNotes()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await this.apiService.GetList<Notes>(
                "https://notesplc.azurewebsites.net",
                "/api",
                "/notes");

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            this.notasList = (List<Notes>)response.Result;
            this.Notas = new ObservableCollection<Notes>(this.notasList);
            this.IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadNotes);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Notas = new ObservableCollection<Notes>(
                    this.notasList);
            }
            else
            {
                this.Notas = new ObservableCollection<Notes>(
                    this.notasList.Where(
                        l => l.Content.ToLower().Contains(this.Filter.ToLower())));
            }
        }
        #endregion
    }
}
