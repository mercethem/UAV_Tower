namespace Aircraft
{
    public class UAV : BaseAircraft
    {
        public UAV(string callSign, string model, TypesOfAircraft type)
            : base(callSign, model, type) { } // Constructor for UAV // İHA için yapıcı metod

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