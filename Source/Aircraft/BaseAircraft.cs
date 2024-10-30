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
        public abstract void TakeOff(); // Abstract method for taking off // Kalkış için soyut metod
        public abstract void Land(); // Abstract method for landing // İniş için soyut metod
        public abstract string ToString(); // Abstract method to get string representation // Dize temsili almak için soyut metod
    }
}
