using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FY111.Resources;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class Device
    {
        public Device()
        {
            LoginLogs = new HashSet<LoginLog>();
        }

        public int Id { get; set; }

        [Display(Name = "Icon", ResourceType = typeof(DisplayAttributeResources))]
        public string Icon { get; set; }

        [Display(Name = "DeviceName", ResourceType = typeof(DisplayAttributeResources))]
        public string Name { get; set; }
        public virtual ICollection<LoginLog> LoginLogs { get; set; }
    }
}
