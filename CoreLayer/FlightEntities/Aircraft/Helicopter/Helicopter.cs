namespace Aircraft
{
    public class Helicopter : BaseAircraft
    {
        public Helicopter(string callSign, string model, TypesOfAircraft type)
            : base(callSign, model, type) { } // Constructor for Helicopter 

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