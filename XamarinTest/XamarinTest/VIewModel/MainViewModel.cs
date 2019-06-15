

using GalaSoft.MvvmLight;

namespace XamarinTest.VIewModel
{
    public class MainViewModel : ViewModelBase
    {

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
            LABELTEXT="fdjfjyhtgfkuy";
        }
    }
}
