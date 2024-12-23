namespace Aircraft
{
    public enum TypesOfAircraft
    {
        Civilian, // Civilian aircraft type
        Military, // Military aircraft type 
        Unknown   // Unknown aircraft type 
    }

    public abstract class BaseAircraft : IAircraft
    {
        protected string CallSign { get; } // Unique identifier
        protected string Model { get; } // Model 
        protected bool IsInAir { get; set; } // Indicates if the aircraft is currently in the air 
        protected TypesOfAircraft Type { get; } // Aircraft type

        protected BaseAircraft(string callSign, string model, TypesOfAircraft type)
        {
            CallSign = callSign; // Assign call sign 
            Model = model; // Assign model 
            IsInAir = false; // Initially on the ground 
            Type = type; // Assign aircraft type 
        }

        public abstract void TakeOff(); // Abstract method for taking off 
        public abstract void Land(); // Abstract method for landing 

        public override string ToString()// String representation of the aircraft 
        {
            return $"{GetType().Name}: {CallSign}, Model: {Model}, Type: {Type}, Is In Air: {IsInAir}"; 
        }
    }
}
