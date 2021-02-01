using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Material.Blazor
{
    /// <summary>
    /// Configuration for <see cref="IMBSnackbarService"/>.
    /// </summary>
    public class MBSnackbarServiceConfiguration
    {
        public const string DefaultCloseButtonIcon = "close";
        public const bool DefaultShowIcons = true;
        public const int DefaultTimeout = 3000;
        public const string DefaultInfoIconName = "notifications";
        public const string DefaultSuccessIconName = "done";
        public const string DefaultWarningIconName = "warning";
        public const string DefaultErrorIconName = "error_outline";



        private string closeButtonIcon = DefaultCloseButtonIcon;
        /// <summary>
        /// The close icon. Defaults to <see cref="MBSnackbarServiceConfiguration.DefaultCloseButtonIcon"/> ("close").
        /// </summary>
        public string CloseButtonIcon { get => closeButtonIcon; set => Setter(ref closeButtonIcon, value); }


        private bool showIcons = DefaultShowIcons;
        /// <summary>
        /// Determines whether default Snackbars show an icon.
        /// </summary>
        public bool ShowIcons { get => showIcons; set => Setter(ref showIcons, value); }


        private uint timeout = DefaultTimeout;
        /// <summary>
        /// Timeout in milliseconds until the Snackbar automatically closes. Defaults to 3000 and ignored if <see cref="MBSnackbarServiceConfiguration.CloseMethod"/> is <see cref="MBSnackbarCloseMethod.CloseButton"/>.
        /// </summary>
        public uint Timeout { get => timeout; set => Setter(ref timeout, value); }


        private int maxSnackbarsShowing = 0;
        /// <summary>
        /// The maximum number of Snackbars that can be simultaneously shown. Further Snackbars are queued until others have been closed. Zero or negative means there is no limit.
        /// </summary>
        public int MaxSnackbarsShowing { get => maxSnackbarsShowing; set => Setter(ref maxSnackbarsShowing, value); }


        private string infoDefaultHeading = "";
        /// <summary>
        /// Default heading for an Info Snackbar.
        /// </summary>
        public string InfoDefaultHeading { get => infoDefaultHeading; set => Setter(ref infoDefaultHeading, value); }


        private string successDefaultHeading = "";
        /// <summary>
        /// Default heading for an Success Snackbar.
        /// </summary>
        public string SuccessDefaultHeading { get => successDefaultHeading; set => Setter(ref successDefaultHeading, value); }


        private string warningDefaultHeading = "";
        /// <summary>
        /// Default heading for an waWrning Snackbar.
        /// </summary>
        public string WarningDefaultHeading { get => warningDefaultHeading; set => Setter(ref warningDefaultHeading, value); }


        private string errorDefaultHeading = "";
        /// <summary>
        /// Default heading for an Error Snackbar.
        /// </summary>
        public string ErrorDefaultHeading { get => errorDefaultHeading; set => Setter(ref errorDefaultHeading, value); }


        private string infoIconName;
        /// <summary>
        /// Icon for an Info Snackbar.
        /// </summary>
        public string InfoIconName { get => infoIconName; set => Setter(ref infoIconName, value); }


        private string successIconName;
        /// <summary>
        /// Icon for an Success Snackbar.
        /// </summary>
        public string SuccessIconName { get => successIconName; set => Setter(ref successIconName, value); }


        private string warningIconName;
        /// <summary>
        /// Icon for an warning Snackbar.
        /// </summary>
        public string WarningIconName { get => warningIconName; set => Setter(ref warningIconName, value); }


        private string errorIconName;
        /// <summary>
        /// Icon for an Error Snackbar.
        /// </summary>
        public string ErrorIconName { get => errorIconName; set => Setter(ref errorIconName, value); }


        /// <summary>
        /// Snackbar icon foundry.
        /// </summary>
        public IMBIconFoundry IconFoundry { get; set; }


        /// <summary>
        /// Used to notify the Snackbar service that a value has changed.
        /// </summary>
        internal event Action OnValueChanged;

        
        public MBSnackbarServiceConfiguration()
        {
            InfoIconName = DefaultInfoIconName;
            SuccessIconName = DefaultSuccessIconName;
            WarningIconName = DefaultWarningIconName;
            ErrorIconName = DefaultErrorIconName;
        }


        private bool Setter<T>(ref T property, T value)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                OnValueChanged?.Invoke();
                return true;
            }

            return false;
        }
    }
}
