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
#nullable enable annotations
        /// <summary>
        /// The heading - first line.
        /// </summary>
        public string Heading { get; set; }


        /// <summary>
        /// The message - second line.
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// CSS class to be applied to the snackbar message.
        /// </summary>
        public string CssClass { get; set; }


        /// <summary>
        /// Determines whether to show the icon to the left.
        /// </summary>
        public bool? ShowIcon { get; set; }


        /// <summary>
        /// The icon's name.
        /// </summary>
        public string IconName { get; set; }


        /// <summary>
        /// The foundry to use for both leading and trailing icons.
        /// <para><c>IconFoundry="IconHelper.MIIcon()"</c></para>
        /// <para><c>IconFoundry="IconHelper.FAIcon()"</c></para>
        /// <para><c>IconFoundry="IconHelper.OIIcon()"</c></para>
        /// </summary>
        public IMBIconFoundry? IconFoundry { get; set; }

        
        /// <summary>
        /// How the snackbar message gets closed. See <see cref="MBSnackbarCloseMethod"/>.
        /// </summary>
        public MBSnackbarCloseMethod? CloseMethod { get; set; }


        /// <summary>
        /// The snackbar's timeout in milliseconds.
        /// </summary>
        public uint? Timeout { get; set; }
#nullable restore annotations


        /// <summary>
        /// The snackbar service's configuration.
        /// </summary>
        internal MBSnackbarServiceConfiguration Configuration { get; set; }

        internal string AppliedHeading => Heading ?? ConfigHeading;

        internal string AppliedMessage => Message ?? "";

        internal string AppliedCssClass => CssClass ?? "";

        internal bool AppliedShowIcon => (AppliedIconName != null) && ((ShowIcon is null) ? Configuration?.ShowIcons ?? MBSnackbarServiceConfiguration.DefaultShowIcons : (bool)ShowIcon);

        internal string AppliedIconName => string.IsNullOrWhiteSpace(IconName) ? ConfigIconName : IconName;
        
        internal IMBIconFoundry AppliedIconFoundry => (IconFoundry is null) ? Configuration?.IconFoundry ?? new IconFoundryMI() : IconFoundry;

        internal MBSnackbarCloseMethod AppliedCloseMethod => (CloseMethod is null) ? Configuration?.CloseMethod ?? MBSnackbarServiceConfiguration.DefaultCloseMethod : (MBSnackbarCloseMethod)CloseMethod;

        internal uint AppliedTimeout => (Timeout is null) ? Configuration?.Timeout ?? MBSnackbarServiceConfiguration.DefaultTimeout : (uint)Timeout;


        /// <summary>
        /// The level of the snackbar. See <see cref="MBSnackbarLevel"/>.
        /// </summary>
        internal MBSnackbarLevel Level { get; set; }


        /// <summary>
        /// The current display status of the snackbar message driven by its CSS class.
        /// </summary>
        internal SnackbarStatus Status { get; set; }


        /// <summary>
        /// Default heading from the configuration, dependent upon <see cref="Level/>.
        /// </summary>
        internal string ConfigHeading => Level switch
        {
            MBSnackbarLevel.Error => Configuration?.ErrorDefaultHeading ?? "",
            MBSnackbarLevel.Info => Configuration?.InfoDefaultHeading ?? "",
            MBSnackbarLevel.Success => Configuration?.SuccessDefaultHeading ?? "",
            MBSnackbarLevel.Warning => Configuration?.WarningDefaultHeading ?? "",
            _ => throw new InvalidOperationException(),
        };


        /// <summary>
        /// Default icon from the configuration, dependent upon <see cref="Level/>.
        /// </summary>
        internal string ConfigIconName => Level switch
        {
            MBSnackbarLevel.Error => Configuration?.ErrorIconName ?? MBSnackbarServiceConfiguration.DefaultErrorIconName,
            MBSnackbarLevel.Info => Configuration?.InfoIconName ?? MBSnackbarServiceConfiguration.DefaultInfoIconName,
            MBSnackbarLevel.Success => Configuration?.SuccessIconName ?? MBSnackbarServiceConfiguration.DefaultSuccessIconName,
            MBSnackbarLevel.Warning => Configuration?.WarningIconName ?? MBSnackbarServiceConfiguration.DefaultWarningIconName,
            _ => throw new InvalidOperationException(),
        };


        /// <summary>
        /// CSS class to apply to the snackbar message driven by <see cref="Status"/>.
        /// </summary>
        internal string StatusClass => Status switch
        {
            SnackbarStatus.Show => "fade-in",
            SnackbarStatus.FadeOut => "fade-out",
            SnackbarStatus.Hide => "hide",
            _ => throw new InvalidOperationException(),
        };


        /// <summary>
        /// CSS class for the snackbar message driven by <see cref="Level"/>.
        /// </summary>
        internal string ContainerLevelClass => $"mb-snackbar__{Level.ToString().ToLower()}";
        

        /// <summary>
        /// CSS filter for the snackbar required for Material Icons Two-Tone theme.
        /// </summary>
        internal string IconFilterClass => $"{Level.ToString().ToLower()}-filter";
    }
}
