using bike.Views.Settings;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingViewModel : BindableBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public SettingViewModel()
        {
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command is executed when the favourite button is clicked.
        /// </summary>
        public DelegateCommand BackButtonCommand => new DelegateCommand(()=>BackButtonClicked());

        /// <summary>
        /// Gets or sets the command is executed when the edit profile option is clicked.
        /// </summary>
        public DelegateCommand EditProfileCommand => new DelegateCommand(() => EditProfileClicked());

        /// <summary>
        /// Gets or sets the command is executed when the change password option is clicked.
        /// </summary>
        public DelegateCommand ChangePasswordCommand => new DelegateCommand(() => ChangePasswordClicked());

        /// <summary>
        /// Gets or sets the command is executed when the account link option is clicked.
        /// </summary>
        public DelegateCommand LinkAccountCommand => new DelegateCommand(() => LinkAccountClicked());

        /// <summary>
        /// Gets or sets the command is executed when the help option is clicked.
        /// </summary>
        public DelegateCommand HelpCommand => new DelegateCommand(() => HelpClicked());
        /// <summary>
        /// Gets or sets the command is executed when the terms of service option is clicked.
        /// </summary>
        public DelegateCommand TermsCommand => new DelegateCommand(() => TermsServiceClicked());

        /// <summary>
        /// Gets or sets the command is executed when the privacy policy option is clicked.
        /// </summary>
        public DelegateCommand PolicyCommand => new DelegateCommand(() => PrivacyPolicyClicked());

        /// <summary>
        /// Gets or sets the command is executed when the FAQ option is clicked.
        /// </summary>
        public DelegateCommand FAQCommand => new DelegateCommand(() => FAQClicked());

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the back button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void BackButtonClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the edit profile option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void EditProfileClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the change password clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void ChangePasswordClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the account link clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void LinkAccountClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the terms of service clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void TermsServiceClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the privacy and policy clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void PrivacyPolicyClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the FAQ clicked
        /// </summary>
        /// <param name="obj">The object</param>
        /// 

        private void FAQClicked()
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the help option is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void HelpClicked()
        {
            // Do something
        }

        #endregion
    }
}
