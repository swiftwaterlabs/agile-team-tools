using AgileTeamTools.Blazor.Ui.Models;
using Microsoft.AspNetCore.Components;

namespace AgileTeamTools.Blazor.Ui.Pages
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        AppState AppState { get; set; }

        [Parameter]
        public string TeamId { get; set; }

        protected override void OnParametersSet()
        {
            AppState.TeamId = TeamId;

            if(string.IsNullOrWhiteSpace(TeamId))
            {
                NavigationManager.NavigateTo(Paths.Dashboard(Guid.NewGuid().ToString()));
            }
        }

        public void ShowEstimatePage()
        {
            NavigationManager.NavigateTo(Paths.Estimate(TeamId));
        }

        public void ShowLeanCoffeePage()
        {
            NavigationManager.NavigateTo(Paths.LeanCoffee(TeamId));
        }
    }
}
