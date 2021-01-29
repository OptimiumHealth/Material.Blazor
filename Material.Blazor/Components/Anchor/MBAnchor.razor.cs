﻿using Material.Blazor.Internal;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Material.Blazor
{
    /// <summary>
    /// An anchor component that displays toast notification that you display via
    /// <see cref="IMBToastService.ShowToast(MBToastLevel, string, string, MBToastCloseMethod?, string, string, IMBIconFoundry?, bool?, uint?, bool)"/>.
    /// Place this component at the top of either App.razor or MainLayout.razor.
    /// </summary>
    public partial class MBToastAnchor : ComponentFoundation
    {
        [Inject] private IMBToastService ToastService { get; set; }


        private List<ToastInstance> DisplayedToasts { get; set; } = new List<ToastInstance>();
        private Queue<ToastInstance> PendingToasts { get; set; } = new Queue<ToastInstance>();
        private string PositionClass => $"mb-toast__{ToastService.Configuration.Position.ToString().ToLower()}";


        private readonly SemaphoreSlim displayedToastsSemaphore = new SemaphoreSlim(1);
        private readonly SemaphoreSlim pendingToastsSemaphore = new SemaphoreSlim(1);


        // Would like to use <inheritdoc/> however DocFX cannot resolve to references outside Material.Blazor
        protected override void OnInitialized()
        {
            ToastService.OnAdd += AddToast;
        }



        /// <summary>
        /// Adds a toast to the anchor, enqueuing it ready for future display if the maximum number of toasts has been reached.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="settings"></param>
        private void AddToast(MBToastLevel level, MBToastSettings settings)
        {
            InvokeAsync(async () =>
            {
                settings.Configuration = ToastService.Configuration;
                settings.Level = level;

                var toastInstance = new ToastInstance
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = DateTime.Now,
                    Settings = settings
                };

                await pendingToastsSemaphore.WaitAsync();

                try
                {
                    PendingToasts.Enqueue(toastInstance);

                    await displayedToastsSemaphore.WaitAsync();

                    try
                    {
                        FlushPendingToasts();
                    }
                    finally
                    {
                        displayedToastsSemaphore.Release();
                    }
                }
                finally
                {
                    pendingToastsSemaphore.Release();
                }

                StateHasChanged();
            });
        }


        private void FlushPendingToasts()
        {
            bool FlushNext() => PendingToasts.Count() > 0 && (ToastService.Configuration.MaxToastsShowing <= 0 || DisplayedToasts.Where(t => t.Settings.Status != ToastStatus.Hide).Count() < ToastService.Configuration.MaxToastsShowing);

            while (FlushNext())
            {
                var toastInstance = PendingToasts.Dequeue();

                DisplayedToasts.Add(toastInstance);

                if (toastInstance.Settings.AppliedCloseMethod != MBToastCloseMethod.CloseButton)
                {
                    InvokeAsync(() =>
                    {
                        var timeout = toastInstance.Settings.AppliedTimeout;
                        var toastTimer = new System.Timers.Timer(toastInstance.Settings.AppliedTimeout);
                        toastTimer.Elapsed += (sender, args) => { CloseToast(toastInstance.Id); };
                        toastTimer.AutoReset = false;
                        toastTimer.Start();
                    });
                }
            }

            StateHasChanged();
        }



        /// <summary>
        /// Closes a toast and removes it from the anchor, with a fade out routine.
        /// </summary>
        /// <param name="toastId"></param>
        public void CloseToast(Guid toastId)
        {
            InvokeAsync(async () =>
            {

                await displayedToastsSemaphore.WaitAsync();

                try
                {
                    var toastInstance = DisplayedToasts.SingleOrDefault(x => x.Id == toastId);

                    if (toastInstance is null)
                    {
                        return;
                    }

                    toastInstance.Settings.Status = ToastStatus.FadeOut;
                    StateHasChanged();
                }
                finally
                {
                    displayedToastsSemaphore.Release();
                }

                var toastTimer = new System.Timers.Timer(500);
                toastTimer.Elapsed += (sender, args) => { RemoveToast(toastId); };
                toastTimer.AutoReset = false;
                toastTimer.Start();

                StateHasChanged();
            });
        }


        private void RemoveToast(Guid toastId)
        {
            InvokeAsync(async () =>
            {
                await displayedToastsSemaphore.WaitAsync();

                try
                {
                    var toastInstance = DisplayedToasts.SingleOrDefault(x => x.Id == toastId);

                    if (toastInstance is null)
                    {
                        return;
                    }

                    toastInstance.Settings.Status = ToastStatus.Hide;

                    if (DisplayedToasts.Where(x => x.Settings.Status == ToastStatus.FadeOut).Count() == 0)
                    {
                        DisplayedToasts.RemoveAll(x => x.Settings.Status == ToastStatus.Hide);
                    }

                    StateHasChanged();

                    FlushPendingToasts();
                }
                finally
                {
                    displayedToastsSemaphore.Release();
                }
            });
        }
    }
    /// <summary>
    /// An anchor component that displays snackbar notification that you display via
    /// <see cref="IMBSnackbarService.ShowSnackbar(MBSnackbarLevel, string, string, MBSnackbarCloseMethod?, string, string, IMBIconFoundry?, bool?, uint?, bool)"/>.
    /// Place this component at the top of either App.razor or MainLayout.razor.
    /// </summary>
    public partial class MBSnackbarAnchor : ComponentFoundation
    {
        [Inject] private IMBSnackbarService SnackbarService { get; set; }


        private List<SnackbarInstance> DisplayedSnackbars { get; set; } = new List<SnackbarInstance>();
        private Queue<SnackbarInstance> PendingSnackbars { get; set; } = new Queue<SnackbarInstance>();
        private string PositionClass => $"mb-snackbar__{SnackbarService.Configuration.Position.ToString().ToLower()}";


        private readonly SemaphoreSlim displayedSnackbarsSemaphore = new SemaphoreSlim(1);
        private readonly SemaphoreSlim pendingSnackbarsSemaphore = new SemaphoreSlim(1);


        // Would like to use <inheritdoc/> however DocFX cannot resolve to references outside Material.Blazor
        protected override void OnInitialized()
        {
            SnackbarService.OnAdd += AddSnackbar;
        }



        /// <summary>
        /// Adds a snackbar to the anchor, enqueuing it ready for future display if the maximum number of snackbars has been reached.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="settings"></param>
        private void AddSnackbar(MBSnackbarLevel level, MBSnackbarSettings settings)
        {
            InvokeAsync(async () =>
            {
                settings.Configuration = SnackbarService.Configuration;
                settings.Level = level;

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


        private void FlushPendingSnackbars()
        {
            bool FlushNext() => PendingSnackbars.Count() > 0 && (SnackbarService.Configuration.MaxSnackbarsShowing <= 0 || DisplayedSnackbars.Where(t => t.Settings.Status != SnackbarStatus.Hide).Count() < SnackbarService.Configuration.MaxSnackbarsShowing);

            while (FlushNext())
            {
                var snackbarInstance = PendingSnackbars.Dequeue();

                DisplayedSnackbars.Add(snackbarInstance);

                if (snackbarInstance.Settings.AppliedCloseMethod != MBSnackbarCloseMethod.CloseButton)
                {
                    InvokeAsync(() =>
                    {
                        var timeout = snackbarInstance.Settings.AppliedTimeout;
                        var snackbarTimer = new System.Timers.Timer(snackbarInstance.Settings.AppliedTimeout);
                        snackbarTimer.Elapsed += (sender, args) => { CloseSnackbar(snackbarInstance.Id); };
                        snackbarTimer.AutoReset = false;
                        snackbarTimer.Start();
                    });
                }
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
