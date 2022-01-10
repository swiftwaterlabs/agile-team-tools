using MudBlazor;
using System.Data;

namespace AgileTeamTools.Blazor.Ui.Models
{
    public class AppState
    {
        public event Action OnChange;

        private List<BreadcrumbItem> _breadCrumbs = new();

        public List<BreadcrumbItem> BreadCrumbs
        {
            get { return _breadCrumbs; }
        }

        private string _teamId;
        public string TeamId 
        { 
            get
            {
                return _teamId;
            }
            set
            {
                _teamId = value;
                NotifyStateChanged();
            }
        }

        public void SetBreadcrumbs(params BreadcrumbItem[] items)
        {
            _breadCrumbs = items?.ToList() ?? new List<BreadcrumbItem>();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
