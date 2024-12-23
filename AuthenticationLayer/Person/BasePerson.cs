namespace AuthenticationServiceUser.Person
{
    public abstract class BasePerson : IPerson
    {
        protected string CitizenID { get; } // Citizen ID property for identification
        protected string Name { get; } // Name of the person.
        protected string Surname { get; } // Surname of the person.

        protected BasePerson(string citizenID, string name, string surname)
        {
            CitizenID = citizenID; // Initialize CitizenID.
            Name = name; // Initialize Name. 
            Surname = surname; // Initialize Surname. 
        }

        public abstract string DisplayInfo(); // Abstract method to display information about the person.
    }
}
