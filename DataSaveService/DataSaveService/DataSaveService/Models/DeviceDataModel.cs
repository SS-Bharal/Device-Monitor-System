using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaveService.Models
{
    public class DeviceDataModel
    {
        [Key]
        public Guid Id { get; set; }
        public string DeviceCode { get; set; } = string.Empty;
        public Guid? RandomAlarmId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
