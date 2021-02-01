using Material.Blazor.Internal;

using System;

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



#nullable enable annotations
#nullable restore annotations


        /// <summary>
        /// The snackbar service's configuration.
        /// </summary>
        internal MBSnackbarServiceConfiguration Configuration { get; set; }

        internal uint AppliedTimeout => (Timeout is null) ? Configuration?.Timeout ?? MBSnackbarServiceConfiguration.DefaultTimeout : (uint)Timeout;


        /// <summary>
        /// The current display status of the snackbar message driven by its CSS class.
        /// </summary>
        internal SnackbarStatus Status { get; set; }
    }
}
