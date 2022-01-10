using AgileTeamTools.Blazor.Ui.Models;
using Microsoft.AspNetCore.Components;

namespace AgileTeamTools.Blazor.Ui.Shared
{
    public partial class NavMenu:IDisposable
    {
        [Inject]
        AppState AppState { get; set; }

        public bool IsLoading { get; set; } = false;

        public string TeamId { get; set; }

        protected override Task OnInitializedAsync()
        {
            AppState.OnChange += StateHasChanged;

            return RefreshMenu();
        }

        protected override Task OnParametersSetAsync()
        {
            return RefreshMenu();
        }

        public async Task RefreshMenu()
        {

            IsLoading = true;

            TeamId = AppState.TeamId;

            IsLoading = false;

            StateHasChanged();
        }

        public void Dispose()
        {
            AppState.OnChange -= StateHasChanged;
        }
    }
}
