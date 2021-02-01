using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Material.Blazor.Internal
{
    /// <summary>
    /// An anchor component that displays snackbar notification that you display via
    /// <see cref="IMBSnackbarService.ShowSnackbar"/>.
    /// Place this component at the top of either App.razor or MainLayout.razor.
    /// </summary>
    public partial class InternalSnackbarAnchor : ComponentFoundation
    {
        [Inject] private IMBSnackbarService SnackbarService { get; set; }


        private List<SnackbarInstance> DisplayedSnackbars { get; set; } = new List<SnackbarInstance>();
        private Queue<SnackbarInstance> PendingSnackbars { get; set; } = new Queue<SnackbarInstance>();


        private readonly SemaphoreSlim displayedSnackbarsSemaphore = new SemaphoreSlim(1);
        private readonly SemaphoreSlim pendingSnackbarsSemaphore = new SemaphoreSlim(1);


        // Would like to use <inheritdoc/> however DocFX cannot resolve to references outside Material.Blazor
        protected override void OnInitialized()
        {
            SnackbarService.OnAdd += AddSnackbar;
            SnackbarService.OnTriggerStateHasChanged += OnTriggerStateHasChanged;
        }


        public new void Dispose()
        {
            SnackbarService.OnAdd -= AddSnackbar;
            SnackbarService.OnTriggerStateHasChanged -= OnTriggerStateHasChanged;

            base.Dispose();
        }


        /// <summary>
        /// Adds a snackbar to the anchor, enqueuing it ready for future display if the maximum number of snackbars has been reached.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="settings"></param>
        private void AddSnackbar(MBSnackbarSettings settings)
        {
            InvokeAsync(async () =>
            {
                settings.Configuration = SnackbarService.Configuration;

                var snackbarInstance = new SnackbarInstance
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = DateTime.Now,
                    Settings = settings
                };

                await pendingSnackbarsSemaphore.WaitAsync();

                try
                {
                    PendingSnackbars.Enqueue(snackbarInstance);

                    await displayedSnackbarsSemaphore.WaitAsync();

                    try
                    {
                        FlushPendingSnackbars();
                    }
                    finally
                    {
                        displayedSnackbarsSemaphore.Release();
                    }
                }
                finally
                {
                    pendingSnackbarsSemaphore.Release();
                }

                StateHasChanged();
            });
        }

        private void OnTriggerStateHasChanged() => _ = InvokeAsync(StateHasChanged);


        private void FlushPendingSnackbars()
        {
            bool FlushNext() => PendingSnackbars.Count() > 0 && (SnackbarService.Configuration.MaxSnackbarsShowing <= 0 || DisplayedSnackbars.Where(t => t.Settings.Status != SnackbarStatus.Hide).Count() < SnackbarService.Configuration.MaxSnackbarsShowing);

            while (FlushNext())
            {
                var snackbarInstance = PendingSnackbars.Dequeue();

                DisplayedSnackbars.Add(snackbarInstance);

                InvokeAsync(() =>
                {
                    var timeout = snackbarInstance.Settings.AppliedTimeout;
                    var snackbarTimer = new System.Timers.Timer(snackbarInstance.Settings.AppliedTimeout);
                    snackbarTimer.Elapsed += (sender, args) => { CloseSnackbar(snackbarInstance.Id); };
                    snackbarTimer.AutoReset = false;
                    snackbarTimer.Start();
                });
            }

            StateHasChanged();
        }



        /// <summary>
        /// Closes a snackbar and removes it from the anchor, with a fade out routine.
        /// </summary>
        /// <param name="snackbarId"></param>
        public void CloseSnackbar(Guid snackbarId)
        {
            InvokeAsync(async () =>
            {

                await displayedSnackbarsSemaphore.WaitAsync();

                try
                {
                    var snackbarInstance = DisplayedSnackbars.SingleOrDefault(x => x.Id == snackbarId);

                    if (snackbarInstance is null)
                    {
                        return;
                    }

                    snackbarInstance.Settings.Status = SnackbarStatus.FadeOut;
                    StateHasChanged();
                }
                finally
                {
                    displayedSnackbarsSemaphore.Release();
                }

                var snackbarTimer = new System.Timers.Timer(500);
                snackbarTimer.Elapsed += (sender, args) => { RemoveSnackbar(snackbarId); };
                snackbarTimer.AutoReset = false;
                snackbarTimer.Start();

                StateHasChanged();
            });
        }


        private void RemoveSnackbar(Guid snackbarId)
        {
            InvokeAsync(async () =>
            {
                await displayedSnackbarsSemaphore.WaitAsync();

                try
                {
                    var snackbarInstance = DisplayedSnackbars.SingleOrDefault(x => x.Id == snackbarId);

                    if (snackbarInstance is null)
                    {
                        return;
                    }

                    snackbarInstance.Settings.Status = SnackbarStatus.Hide;

                    if (DisplayedSnackbars.Where(x => x.Settings.Status == SnackbarStatus.FadeOut).Count() == 0)
                    {
                        DisplayedSnackbars.RemoveAll(x => x.Settings.Status == SnackbarStatus.Hide);
                    }

                    StateHasChanged();

                    FlushPendingSnackbars();
                }
                finally
                {
                    displayedSnackbarsSemaphore.Release();
                }
            });
        }
    }
}