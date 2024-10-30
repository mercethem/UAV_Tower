namespace Aircraft
{
    public enum TypesOfAircraft
    {
        Civilian, // Civilian aircraft type // Sivil hava aracı türü
        Military, // Military aircraft type // Askeri hava aracı türü
        Unknown   // Unknown aircraft type // Bilinmeyen hava aracı türü
    }

    public abstract class BaseAircraft : IAircraft
    {
        protected string CallSign { get; } // Unique identifier // Benzersiz tanımlayıcı
        protected string Model { get; } // Model // Model
        protected bool IsInAir { get; set; } // Indicates if the aircraft is currently in the air // Hava aracının havada olup olmadığını gösterir
        protected TypesOfAircraft Type { get; } // Aircraft type // Hava aracı türü

        protected BaseAircraft(string callSign, string model, TypesOfAircraft type)
        {
            CallSign = callSign; // Assign call sign // Çağrı işaretini atar
            Model = model; // Assign model // Modeli atar
            IsInAir = false; // Initially on the ground // Başlangıçta yerde
            Type = type; // Assign aircraft type // Hava aracı türünü atar
        }

        public abstract void TakeOff(); // Abstract method for taking off // Kalkış için soyut metod
        public abstract void Land(); // Abstract method for landing // İniş için soyut metod 

        public override string ToString()
        {
            return $"{GetType().Name}: {CallSign}, Model: {Model}, Type: {Type}, Is In Air: {IsInAir}"; // String representation of the aircraft // Hava aracının dize temsili
        }
    }
}
