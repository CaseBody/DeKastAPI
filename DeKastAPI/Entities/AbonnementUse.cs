namespace DeKastAPI.Entities
{
    public class AbonnementUse
    {
        public int Id { get; set; }
        public int AbonnementId { get; set; }
        public int WeekNumber { get; set; }

        public Abonnement Abonnement { get; set; }
    }
}
