

namespace ForeignExchance.ViewModels
{
 
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Windows.Input;

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
        #endregion
        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        string _result;
        string _sourceRate;
        string _status;
        string _targetRate;
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
            LoadRates();
        }

        async void LoadRates()
        {
            IsRunning = true;
            Result = Lenguages.Loading;
            var response = await apiService.GetList<Rate>(
                                            "http://apiexchangerates.azurewebsites.net",
                                            "/api/Rates");
            if (!response.IsSucces)
            {
                IsRunning = false;
                Result = response.Message;
                return;
                
            }

            Rates = new ObservableCollection<Rate>((List<Rate>)response.Result);
            IsRunning = false;
            IsEnabled = true;
            Result = Lenguages.Ready;

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
                await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error,
                    Lenguages.AmountNumericValidation,
                    Lenguages.Accept);
                return;
            }

            decimal amount = 0;
            if(!decimal.TryParse(Amount, out amount))
            {
                await Application.Current.MainPage.DisplayAlert(
                              Lenguages.Error,
                              Lenguages.AmountNumericValidation,
                              Lenguages.Accept);
                return;

            }

            if (string.IsNullOrEmpty(SourceRate))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error,
                    Lenguages.SourceRateValidation,
                    Lenguages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(TargetRate))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error,
                    Lenguages.TargetRateValidation,
                    Lenguages.Accept);
                return;
            }

            var sourceRate = RateService.GetRateByName(SourceRate);
            var targetRate = RateService.GetRateByName(TargetRate);

            if(sourceRate == null || targetRate == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error,
                    "Rate not found",
                    Lenguages.Accept);
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
