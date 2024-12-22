namespace Aircraft
{
    public class Plane : BaseAircraft
    {
        public Plane(string callSign, string model, TypesOfAircraft type)
            : base(callSign, model, type) { } // Constructor for Plane // Uçak için yapıcı metod

        public override void TakeOff()
        {
            IsInAir = true; // Changes status to in the air // Durumu havada olarak değiştirir
        }

        public override void Land()
        {
            IsInAir = false; // Changes status to on the ground // Durumu yerde olarak değiştirir
        }
    }
}