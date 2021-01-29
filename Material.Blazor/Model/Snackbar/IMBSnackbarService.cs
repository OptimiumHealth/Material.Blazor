using System;

namespace Material.Blazor
{
    /// <summary>
    /// Interface for the Material.Blazor snackbar service, developed from the code base of Blazored Snackbar by Chris Sainty.
    /// Works in conjunction with a <see cref="MBSnackbarAnchor"/> that must be placed in either App.razor or
    /// MainLayout.razor to avoid an exception being thrown when you first attempt to show a snackbar notification.
    /// 
    /// <para>
    /// Throws a <see cref="System.InvalidOperationException"/> if
    /// <see cref="ShowSnackbar(MBSnackbarLevel, string, string, MBSnackbarCloseMethod?, string, string, IMBIconFoundry?, bool?, uint?, bool)"/>
    /// is called without an <see cref="MBSnackbarAnchor"/> component used in the app.
    /// </para>
    /// <example>
    /// <para>You can optionally add configuration when you add this to the service collection:</para>
    /// <code>
    /// services.AddMBSnackbarService(new MBSnackbarServiceConfiguration()
    /// {
    ///     Postion = MBSnackbarPosition.TopRight,
    ///     CloseMethod = MBSnackbarCloseMethod.Timeout,
    ///     ... etc
    /// });
    /// </code>
    /// </example>
    /// </summary>
    public interface IMBSnackbarService
    {
        /// <summary>
        /// Snackbar service configuration
        /// </summary>
        public MBSnackbarServiceConfiguration Configuration { get; set; }



        /// <summary>
        /// A event that will be invoked when showing a snackbar
        /// </summary>
        internal event Action<MBSnackbarLevel, MBSnackbarSettings> OnAdd;



        /// <summary>
        /// A event that will be invoked when snackbars should call StateHasChanged. This will
        /// be when the configuration is updated.
        /// </summary>
        internal event Action OnTriggerStateHasChanged;



        /// <summary>
        /// Shows a snackbar using the supplied settings. Only the level and message parameters are required, with
        /// the remainder haveing defaults specified by the <see cref="MBSnackbarServiceConfiguration"/> that you can supply
        /// when registering services. Failing that Material.Blazor provides defaults.
        /// </summary>
        /// <param name="level">Severity of the snackbar (info, error, etc)</param>
        /// <param name="message">Body text in the snackbar</param>
        /// <param name="heading">Text used in the heading of the snackbar</param>
        /// <param name="closeMethod">close method</param>
        /// <param name="cssClass">additional css applied to snackbar</param>
        /// <param name="iconName">Icon name</param>
        /// <param name="iconFoundry">The icon's foundry</param>
        /// <param name="showIcon">Show or hide icon</param>
        /// <param name="timeout">Length of time before autodismiss</param>
        /// <param name="debug">If true only shows snackbars when compiling in DEBUG mode</param>
#nullable enable annotations
        void ShowSnackbar(
            MBSnackbarLevel level,
            string message,
            string heading = null,
            MBSnackbarCloseMethod? closeMethod = null,
            string cssClass = null,
            string iconName = null,
            IMBIconFoundry? iconFoundry = null,
            bool? showIcon = null,
            uint? timeout = null,
            bool debug = false);
#nullable restore annotations
    }
}
