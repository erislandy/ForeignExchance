

namespace ForeignExchance.ViewModels
{
 
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Windows.Input;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Newtonsoft.Json;
    using Models;
    using Services;
    using Helpers;
   

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        string _result;
        string _sourceRate;
        string _status;
        string _targetRate;
        List<Rate> rates;
        #endregion

        #region Properties

        public string Amount
        {
            get;
            set;
        }

        public string SourceRate
        {
            get
            {
                return _sourceRate;
            }
            set
            {
                if(_sourceRate != value)
                {
                    _sourceRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(SourceRate))
                        );
                }
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Status))
                        );
                }
            }
        }
        public string TargetRate
        {
            get
            {
                return _targetRate;
            }
            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TargetRate))
                        );
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if(_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning))
                        );
                }
            }
        }
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if(_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled))
                        );
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if(_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Result))
                        );
                }
            }
        }

        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
            LoadRates();
            SourceRate = "CUC";
            TargetRate = "CUP";
        }

        async void LoadRates()
        {
            IsRunning = true;
            Result = Lenguages.Loading;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSucces)
            {
                LoadLocalData();
            }
            else
            {
                await LoadFromApi();
            }
            if(rates.Count == 0)
            {
                IsRunning = false;
                IsEnabled = false;
                Result = "There are not internet connection and not load previosly rates." +
                    "Please try again width internet connection";
                Status = "Not rate loaded";
                return;
            }

            Rates = new ObservableCollection<Rate>(rates);
            IsRunning = false;
            IsEnabled = true;
            Result = Lenguages.Ready;

        }

        private void LoadLocalData()
        {
            rates = dataService.Get<Rate>(false);
            Status = Lenguages.StatusLocalConnection;
        }

        private async Task LoadFromApi()
        {
            var url = "http://apiexchangerates.azurewebsites.net"; //Application.Current.Resources["URLAPI"].ToString();
            var response = await apiService.GetList<Rate>(
                                            url,
                                            "/api/Rates");
            if (!response.IsSucces)
            {
                LoadLocalData();
                return;

            }
            rates = (List<Rate>)response.Result;

            //Storage data Local
            dataService.DeleteAll<Rate>();
            dataService.Save(rates);

            Status = Lenguages.StatusInternetConnection;
        }
        #endregion

        #region Commands
        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }
        async void Convert()
        {
            if (string.IsNullOrEmpty(Amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmountNumericValidation);
                return;
            }

            decimal amount = 0;
            if(!decimal.TryParse(Amount, out amount))
            {
                await dialogService.ShowMessage(
                              Lenguages.Error,
                              Lenguages.AmountNumericValidation);
                return;

            }

            if (string.IsNullOrEmpty(SourceRate))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.SourceRateValidation);
                return;
            }

            if (string.IsNullOrEmpty(TargetRate))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.TargetRateValidation);
                return;
            }

            var sourceRate = RateService.GetRateByName(SourceRate);
            var targetRate = RateService.GetRateByName(TargetRate);

            if(sourceRate == null || targetRate == null)
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    "Rate not found");
                return;
            }

            var amountConverted = amount /
                                   (decimal)sourceRate.TaxtRate * 
                                   (decimal)targetRate.TaxtRate;
            Result = string.Format("{0} {1:C2} = {2} {3:C2}",
                                    sourceRate.Code,
                                    Amount,
                                    targetRate.Code,
                                    amountConverted);

        }

        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(SwitchMethod);
            }
        }

        private void SwitchMethod()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
            Convert();

        }

        #endregion
    }
}
