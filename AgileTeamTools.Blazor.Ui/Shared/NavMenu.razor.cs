namespace AgileTeamTools.Blazor.Ui.Shared
{
    public partial class NavMenu
    {
        public bool IsLoading { get; set; } = false;

        protected override Task OnParametersSetAsync()
        {
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
