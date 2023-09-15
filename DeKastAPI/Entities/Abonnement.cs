namespace DeKastAPI.Entities
{
    public class Abonnement
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public AbonnementType Type { get; set; }

        public List<AbonnementUse> AbonnementUses { get; set; }
    }
}
