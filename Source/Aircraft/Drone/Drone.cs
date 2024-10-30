﻿namespace Aircraft
{
    public class Drone : BaseAircraft
    {
        private readonly string CallSign; // Unique identifier for the drone // Drone için benzersiz tanımlayıcı
        private readonly string Model; // Model of the drone // Drone model bilgisi
        private bool IsInAir { get; set; } // Indicates if the drone is currently in the air // Drone'un havada olup olmadığını gösterir
        private readonly TypesOfAircraft Type; // Enum property for aircraft type // Hava aracı türü için enum özelliği

        public Drone(string callSign, string model, TypesOfAircraft type)
        {
            CallSign = callSign; // Assigns the call sign // Çağrı işaretini atar
            Model = model; // Assigns the model // Modeli atar
            IsInAir = false; // Initially on the ground // Başlangıçta yerde
            Type = type; // Assigns the enum value // Enum değerini atar
        }

        public override void TakeOff()
        {
            IsInAir = true; // Changes status to in the air // Durumu havada olarak değiştirir
        }

        public override void Land()
        {
            IsInAir = false; // Changes status to on the ground // Durumu yerde olarak değiştirir
        }

        public override string ToString()
        {
            return $"Drone: {CallSign}, Model: {Model}, Type: {Type}, Is In Air: {IsInAir}"; // Returns a string representation of the drone // Drone'un dize temsili döndürülür
        }
    }
}