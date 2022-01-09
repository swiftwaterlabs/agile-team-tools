using AgileTeamTools.Blazor.Ui.Models;
using AgileTeamTools.Blazor.Ui.Themes;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.ThemeManager;

namespace AgileTeamTools.Blazor.Ui.Shared
{
    public partial class MainLayout:IDisposable
    {
        [Inject]
        AppState AppState { get; set; }

        public NavMenu NavMenu { get; set; }

        public bool IsDrawerOpen = false;

        private readonly ThemeManagerTheme _themeManager = new()
        {
            Theme = new EnterpriseTheme(),
            DrawerClipMode = DrawerClipMode.Always,
            FontFamily = "Montserrat",
            DefaultBorderRadius = 6,
            AppBarElevation = 1,
            DrawerElevation = 1
        };

        protected override void OnInitialized()
        {
            AppState.OnChange += StateHasChanged;

            StateHasChanged();
        }

        public void Dispose()
        {
            AppState.OnChange -= StateHasChanged;
        }

        void DrawerToggle()
        {
            IsDrawerOpen = !IsDrawerOpen;
            NavMenu?.RefreshMenu();
        }
    }
}
