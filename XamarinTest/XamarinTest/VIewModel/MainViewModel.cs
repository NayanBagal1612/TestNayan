using DataLayer.Services;
using GalaSoft.MvvmLight;

namespace XamarinTest.VIewModel
{
    public class MainViewModel : ViewModelBase
    {
        EmployeeService _employeeService;

        #region LABELTEXT
        public const string LABELTEXTPropertyName = "LABELTEXT";

        private string _LABELTEXT="Hello World";

        public string LABELTEXT
        {
            get
            {
                return _LABELTEXT;
            }

            set
            {
                if (_LABELTEXT == value)
                {
                    return;
                }

                _LABELTEXT = value;
                RaisePropertyChanged(LABELTEXTPropertyName);
            }
        } 
        #endregion

        public MainViewModel()
        {
            _employeeService = new EmployeeService();
            var res = _employeeService.GetEmployee();
            LABELTEXT ="Hiii";
        }
    }
}
