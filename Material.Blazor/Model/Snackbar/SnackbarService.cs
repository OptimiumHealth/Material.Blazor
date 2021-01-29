using System;

namespace Material.Blazor.Internal
{
    /// <summary>
    /// The internal implementation of <see cref="IMBSnackbarService"/>.
    /// </summary>
    internal class SnackbarService : IMBSnackbarService
    {
        private MBSnackbarServiceConfiguration configuration = new MBSnackbarServiceConfiguration();
        ///<inheritdoc/>
        public MBSnackbarServiceConfiguration Configuration
        { 
            get => configuration; 
            set
            { 
                configuration = value;
                configuration.OnValueChanged += ConfigurationChanged;
                OnTriggerStateHasChanged?.Invoke(); 
            }
        }


        private event Action<MBSnackbarLevel, MBSnackbarSettings> OnAdd;
        private event Action OnTriggerStateHasChanged;

        ///<inheritdoc/>
        event Action<MBSnackbarLevel, MBSnackbarSettings> IMBSnackbarService.OnAdd
        {
            add => OnAdd += value;
            remove => OnAdd -= value;
        }


        ///<inheritdoc/>
        event Action IMBSnackbarService.OnTriggerStateHasChanged
        {
            add => OnTriggerStateHasChanged += value;
            remove => OnTriggerStateHasChanged -= value;
        }


        public SnackbarService(MBSnackbarServiceConfiguration configuration)
        {
            Configuration = configuration;
        }

        private void ConfigurationChanged() => OnTriggerStateHasChanged?.Invoke();


        ///<inheritdoc/>
#nullable enable annotations
        public void ShowSnackbar(
            MBSnackbarLevel level,
            string message,
            string heading = null,
            MBSnackbarCloseMethod? closeMethod = null,
            string cssClass = null,
            string iconName = null,
            IMBIconFoundry? iconFoundry = null,
            bool? showIcon = null,
            uint? timeout = null,
            bool debug = false)
#nullable restore annotations
        {
#if !DEBUG
            if (debug)
            {
                return;
            }
#endif

            var settings = new MBSnackbarSettings()
            {
                Message = message,
                Heading = heading,
                CloseMethod = closeMethod,
                CssClass = cssClass,
                IconName = iconName,
                IconFoundry = iconFoundry,
                ShowIcon = showIcon,
                Timeout = timeout
            };

            if (OnAdd is null)
            {
                throw new InvalidOperationException($"Material.Blazor: you attempted to show a snackbar notification from a {Utilities.GetTypeName(typeof(IMBSnackbarService))} but have not placed a {Utilities.GetTypeName(typeof(MBSnackbarAnchor))} component at the top of either App.razor or MainLayout.razor");
            }

            OnAdd?.Invoke(level, settings);
        }
    }
}
