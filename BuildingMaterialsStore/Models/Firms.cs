using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMaterialsStore.Models
{
    class Firms
    {
        public int idFirm;
        public string FirmName { get; set; }
        public string UNP { get; set; }
        public string FirmLegalAddress { get; set; }
        public string FirmAccountNumber { get; set; }
        public string FirmBankDetails { get; set; }
        public double FirmDiscountAmount { get; set; }
    }
}
