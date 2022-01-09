using Microsoft.AspNetCore.Components;

namespace AgileTeamTools.Blazor.Ui.Pages
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public void ShowEstimatePage()
        {
            NavigationManager.NavigateTo("estimate");
        }

        public void ShowLeanCoffeePage()
        {
            NavigationManager.NavigateTo("leancoffee");
        }
    }
}
