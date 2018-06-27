using System;
using System.Collections.Generic;
using System.Text;

namespace Lands.ViewModels
{
    using Lands.Models;
    using Lands.Services;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    class LandsViewModel :  BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Land> lands;
        #endregion

        #region Properties
        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { SetValue(ref this.lands, value); }
        }
        #endregion

        #region Constructor
        public LandsViewModel()
        {
            this.apiService = new ApiService();
            LoadLands();
        }
        #endregion

        #region Methods
        private async void LoadLands()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await this.apiService.GetList<Land>(
                "https://restcountries.eu",
                "/rest",
                "/v2/all");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var list = (List<Land>) response.Result;
            this.lands = new ObservableCollection<Land>(list);
        }
        #endregion
    }
}
