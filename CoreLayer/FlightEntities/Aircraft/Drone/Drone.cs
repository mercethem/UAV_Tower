namespace Aircraft
{
    public class Drone : BaseAircraft
    {
        public Drone(string callSign, string model, TypesOfAircraft type)
            : base(callSign, model, type) { } // Constructor for Drone // Drone için yapıcı metod

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