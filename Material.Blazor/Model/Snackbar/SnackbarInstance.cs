using System;

namespace Material.Blazor.Internal
{
    /// <summary>
    /// The current status of a Snackbar, expressed through it's CSS class.
    /// </summary>
    internal enum SnackbarStatus { Show, FadeOut, Hide }


    /// <summary>
    /// An instance of a snackbar message.
    /// </summary>
    internal class SnackbarInstance
    {
        /// <summary>
        /// The snackbar's unique id.
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// It's timestamp for when it was raised.
        /// </summary>
        public DateTime TimeStamp { get; set; }


        /// <summary>
        /// The settings containing all data determining the snackbar's style and behaviour.
        /// </summary>
        public MBSnackbarSettings Settings { get; set; }
    }
}
