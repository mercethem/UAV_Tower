namespace Aircraft
{
    public class UAV : BaseAircraft
    {
        public UAV(string callSign, string model, TypesOfAircraft type)
            : base(callSign, model, type) { } // Constructor for UAV 

        public override void TakeOff()// Changes status to in the air
        {
            IsInAir = true; 
        }

        public override void Land()// Changes status to on the ground
        {
            IsInAir = false; 
        }
    }
}