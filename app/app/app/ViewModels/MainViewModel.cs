using System;
using System.Collections.Generic;
using System.Text;

namespace app.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels

        public NotesViewModel Notes
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Notes = new NotesViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
