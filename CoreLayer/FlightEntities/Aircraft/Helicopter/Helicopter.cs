namespace Aircraft
{
    public class Helicopter : BaseAircraft
    {
        public Helicopter(string callSign, string model, TypesOfAircraft type)
            : base(callSign, model, type) { } // Constructor for Helicopter // Helikopter için yapıcı metod

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