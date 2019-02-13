

namespace Sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    using Common.Models;
    using Helpers;
    using Services;
    using Views;
    public class ProductsViewModel: BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private bool isRefreshing;
        #endregion

        #region Properties
        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion



        #region Constructors
        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        #endregion

        #region Singleton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if(instance== null)
            {
                return new ProductsViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.isSucess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;

            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Getlist<Product>(url, prefix, controller);
            if (!response.isSucess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
            var list = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(list);

            this.IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            { return new RelayCommand(LoadProducts); }

        } 
        #endregion
    }
}
