namespace API_DB.Controllers
{
    public class Person
    {
        //attributes
        public int id { get; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string streetName { get; set; }
        public int houseNumber { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int zipCode { get; set; }

        //constructors
        public Person() { }
        public Person(string fName, string lName)
        {
            firstName = fName;
            lastName = lName;
        }
        public Person(int personid, string fName, string lName, string street, int house, string c, string s, int zip)
        {
            id = personid;
            firstName = fName;
            lastName = lName;
            streetName = street;
            houseNumber = house;
            city = c;
            state = s;
            zipCode = zip;
        }

    }
}