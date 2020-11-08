using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App.Models
{
    [XmlRoot("person")]
    public class Person
    {
        [JsonProperty(propertyName: "firstname")]
        [XmlElement("firstname")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "lastname")]
        [XmlElement("lastname")]
        public string LastName { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        [JsonProperty(propertyName: "age")]
        [XmlElement("age")]
        public int Age { get; set; }

        [JsonProperty(propertyName: "City")]
        [XmlElement("city")]
        public string City { get; set; }

        public string Summary => $"{DisplayName}, {Age}, {City}";

        public Person()
        {
        }

        public Person(string firstName, string lastName, int age, string city)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
            this.City = city;
        }
    }
}
