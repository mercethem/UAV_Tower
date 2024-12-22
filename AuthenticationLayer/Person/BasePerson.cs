namespace AuthenticationServiceUser.Person
{
    public abstract class BasePerson : IPerson
    {
        protected string CitizenID { get; } // Citizen ID property for identification. // Kimlik doğrulama için vatandaşlık ID'si özelliği.
        protected string Name { get; } // Name of the person. // Kişinin adı.
        protected string Surname { get; } // Surname of the person. // Kişinin soyadı.

        protected BasePerson(string citizenID, string name, string surname)
        {
            CitizenID = citizenID; // Initialize CitizenID. // CitizenID'yi başlat.
            Name = name; // Initialize Name. // Adı başlat.
            Surname = surname; // Initialize Surname. // Soyadı başlat.
        }

        public abstract string DisplayInfo(); // Abstract method to display information about the person. // Kişi hakkında bilgi göstermek için soyut metod.
    }
}
