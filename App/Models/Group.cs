using App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App.Models
{
    [XmlRoot("group")]
    public class Group
    {
        [XmlElement("person")]
        public Person[] Persons { get; set; }
    }
}
