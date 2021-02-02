﻿using Material.Blazor.Internal;
using System;
using System.Threading.Tasks;

namespace Material.Blazor
{
    /// <summary>
    /// Settings for an individual snackbar notification determining all aspects controlling
    /// it's markup and behaviour. All parameters are optional with defaults defined in
    /// the <see cref="MBSnackbarServiceConfiguration"/> that you define when creating the snackbar service.
    /// </summary>
    public class MBSnackbarSettings
    {
        /// <summary>
        /// TODO documentation
        /// </summary>
        public Action Action { get; set; }
        /// <summary>
        /// TODO documentation
        /// </summary>
        public string ActionText { get; set; }
        /// <summary>
        /// TODO documentation
        /// </summary>
        public bool Leading { get; set; }
        /// <summary>
        /// TODO documentation
        /// </summary>
        public bool Stacked { get; set; }
        /// <summary>
        /// TODO documentation
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// TODO documentation
        /// </summary>
        public bool DismissIcon { get; set; }


        private int? timeout;
        /// <summary>
        /// The snackbar's timeout in milliseconds.
        /// Minimum value is 4000 (4 seconds), maximum value is 10000 (10s).
        /// Use -1 to disable.
        /// </summary>
        public int? Timeout
        {
            get => timeout;
            set
            {
                if (value == null)
                {
                    timeout = null;
                }
                else if (value == -1)
                {
                    timeout = -1;
                }
                else
                {
                    timeout = Math.Max(4000, Math.Min(value.Value, 10000));
                }
            }
        }

        /// <summary>
        /// The snackbar service's configuration.
        /// </summary>
        internal MBSnackbarServiceConfiguration Configuration { get; set; }

        internal int AppliedTimeout => (Timeout is null) ? Configuration?.Timeout ?? MBSnackbarServiceConfiguration.DefaultTimeout : (int)Timeout;

        internal Func<SnackbarInstance, Task> OnClose { get; set; }
        internal bool Closed { get; set; }
    }
}
