namespace AgileTeamTools.Blazor.Ui.Models
{
    public static class Paths
    {
        public static string Dashboard(string teamId)
        {
            return $"/{teamId}";
        }

        public static string Estimate(string teamId)
        {
            return $"/{teamId}/estimate";
        }

        public static string LeanCoffee(string teamId)
        {
            return $"/{teamId}/leancoffee";
        }


    }
}
