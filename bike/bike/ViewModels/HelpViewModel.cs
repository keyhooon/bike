using Prism.Commands;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Help page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class HelpViewModel : BindableBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpViewModel" /> class
        /// </summary>
        public HelpViewModel()
        {

        }
        #endregion

        #region Method

        /// <summary>
        /// Invoked when the other queries option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void OtherQueriesClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the account queries option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void AccountQueriesClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the offers queries option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void OffersQueriesClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the payment queries option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void PaymentQueriesClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the refund option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void RefundClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the issue previous order option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void IssuePreviousOrderClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the back button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void BackButtonClicked()
        {
            // Do something
        }
        #endregion

        #region Command

        private DelegateCommand _backButtonCommand;
        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public DelegateCommand BackButtonCommand => _backButtonCommand ?? (_backButtonCommand = new DelegateCommand(()=>BackButtonClicked()));


        private DelegateCommand _issuePreviousOrderCommand;
        /// <summary>
        /// Gets or sets the command is executed when the issue previous order option is clicked.
        /// </summary>
        public DelegateCommand IssuePreviousOrderCommand => _issuePreviousOrderCommand ?? (_issuePreviousOrderCommand = new DelegateCommand(() => IssuePreviousOrderClicked()));


        private DelegateCommand _return_RefundCommand;
        /// <summary>
        /// Gets or sets the command is executed when the return refund option is clicked.
        /// </summary>
        public DelegateCommand Return_RefundCommand => _return_RefundCommand ?? (_return_RefundCommand = new DelegateCommand(() => RefundClicked()));

        private DelegateCommand _paymentQueriesCommand;
        /// <summary>
        /// Gets or sets the command is executed when the payment queries option is clicked.
        /// </summary>
        public DelegateCommand PaymentQueriesCommand => _paymentQueriesCommand ?? (_paymentQueriesCommand = new DelegateCommand(() => PaymentQueriesClicked()));


        private DelegateCommand _offersQueriesCommand;
        /// <summary>
        /// Gets or sets the command is executed when the offer queries  option is clicked.
        /// </summary>
        public DelegateCommand OffersQueriesCommand => _offersQueriesCommand ?? (_offersQueriesCommand = new DelegateCommand(() => OffersQueriesClicked()));


        private DelegateCommand _accountQueriesCommand;
        /// <summary>
        /// Gets or sets the command is executed when the account queries option is clicked.
        /// </summary>
        public DelegateCommand AccountQueriesCommand => _accountQueriesCommand ?? (_accountQueriesCommand = new DelegateCommand(() => AccountQueriesClicked()));



        private DelegateCommand _otherQueriesCommand;
        /// <summary>
        /// Gets or sets the command is executed when the other queries option is clicked.
        /// </summary>
        public DelegateCommand OtherQueriesCommand => _otherQueriesCommand ?? (_otherQueriesCommand = new DelegateCommand(() => OtherQueriesClicked()));

        #endregion
    }


}
