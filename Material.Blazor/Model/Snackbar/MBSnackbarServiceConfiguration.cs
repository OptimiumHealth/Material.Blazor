using System;
using System.Collections.Generic;

namespace Material.Blazor
{
    /// <summary>
    /// Configuration for <see cref="IMBSnackbarService"/>.
    /// </summary>
    public class MBSnackbarServiceConfiguration
    {
        public const int DefaultTimeout = 5000;


        private uint timeout = DefaultTimeout;
        /// <summary>
        /// Timeout in milliseconds until the Snackbar automatically closes. Defaults to 5000/>.
        /// </summary>
        public uint Timeout { get => timeout; set => Setter(ref timeout, value); }


        /// <summary>
        /// Used to notify the Snackbar service that a value has changed.
        /// </summary>
        internal event Action OnValueChanged;


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
