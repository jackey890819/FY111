using FY111.Models.FY111;
using System.ComponentModel.DataAnnotations;

namespace FY111.Models.Dto
{
    public class MetaverseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public int? Duration { get; set; }
    }

    public class MetaverseDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Icon { get; set; }
        public string Introduction { get; set; }
        public byte SignupEnabled { get; set; }
        public byte CheckinEnabled { get; set; }
        public int? Duration { get; set; }

        public MetaverseDetailDto() { }
        public MetaverseDetailDto(Metaverse metaverse)
        {
            Id = metaverse.Id;
            Name = metaverse.Name;
            Ip = metaverse.Ip;
            Icon = metaverse.Icon; 
            Introduction = metaverse.Introduction;
            Duration = metaverse.Duration;
            SignupEnabled = metaverse.SignupEnabled;
            CheckinEnabled = metaverse.CheckinEnabled;
        }
    }

    public class MetaverseCreateModel
    {
        [Required]
        public string Name { get; set; }
        public string Ip { get; set; } = "";
        public string Introduction { get; set; } = "";
        public byte SignupEnabled { get; set; } = 1;
        public byte CheckinEnabled { get; set; } = 0;
        public int? Duration { get; set; } = null;
    }

    public class MetaverseUpdateModel
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Ip { get; set; }
        public string? Icon { get; set; }
        public string? Introduction { get; set; }
        public byte? SignupEnabled { get; set; }
        public byte? CheckinEnabled { get; set; }
        public int? Duration { get; set; }
    }

}
