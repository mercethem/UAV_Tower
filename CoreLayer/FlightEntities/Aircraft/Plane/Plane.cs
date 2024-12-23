namespace Aircraft
{
    public class Plane : BaseAircraft
    {
        public Plane(string callSign, string model, TypesOfAircraft type)
            : base(callSign, model, type) { } // Constructor for Plane

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