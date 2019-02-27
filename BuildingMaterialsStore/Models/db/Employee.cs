using BuildingMaterialsStore.Models.db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Employee
    {
        [Key]
        [ForeignKey("User")]
        public int idEmployee;
        public string EmpLastName;
        public string EmpFirstName;
        public string EmpPatronymic;
        public DateTime EmpDateOfBirth;
        public string EmpAddress;
        public string EmpPhoneNomber;
        public string Position;
        public byte Experience;

        ICollection<Storage> Storages;
        public Employee()
        {
            Storages = new List<Storage>();
        }

        public User User;
    }
}
