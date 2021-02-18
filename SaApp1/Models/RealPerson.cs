using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaApp1.Models
{
    public class RealPerson
    {
        private SaApp1DBEntities _db = new SaApp1DBEntities();

        public int ID { get; set; }
        public Person Person { get; set; }
        public PersonInfo PersonInfo { get; set; }

        public RealPerson()
        { }
        public RealPerson(int personId)
        {
            ID = personId;
            Person = _db.People.FirstOrDefault(a => a.Id == personId);
            PersonInfo = _db.PersonInfoes.FirstOrDefault(a => a.PersonId == personId);
        }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}