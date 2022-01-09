using AgileTeamTools.Blazor.Ui.Models;
using Microsoft.AspNetCore.Components;

namespace AgileTeamTools.Blazor.Ui.Shared
{
    public partial class NavMenu
    {
        [Inject]
        AppState AppState { get; set; }

        public bool IsLoading { get; set; } = false;

        public string TeamId { get; set; }

        protected override Task OnParametersSetAsync()
        {
            TeamId = AppState.TeamId;
            return RefreshMenu();
        }


        public async Task RefreshMenu()
        {

            IsLoading = true;
            
            IsLoading = false;

            StateHasChanged();
        }
    }
}
