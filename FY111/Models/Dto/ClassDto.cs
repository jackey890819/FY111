using FY111.Models.FY111;
using System.ComponentModel.DataAnnotations;

namespace FY111.Models.Dto
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int? Duration { get; set; }
    }

    public class ClassDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public byte SignupEnabled { get; set; }
        public byte CheckinEnabled { get; set; }
        public int? Duration { get; set; }

        public ClassDetailDto() { }
        public ClassDetailDto(Class @class)
        {
            Id = @class.Id;
            Name = @class.Name;
            Ip = @class.Ip;
            Image = @class.Image; 
            Content = @class.Content;
            Duration = @class.Duration;
            SignupEnabled = @class.SignupEnabled;
            CheckinEnabled = @class.CheckinEnabled;
        }
    }

    public class ClassCreateModel
    {
        [Required]
        public string Name { get; set; }
        public string Ip { get; set; } = "";
        public string Content { get; set; } = "";
        public byte SignupEnabled { get; set; } = 1;
        public byte CheckinEnabled { get; set; } = 0;
        public int? Duration { get; set; } = null;
    }

    public class ClassUpdateModel
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Ip { get; set; }
        public string? Image { get; set; }
        public string? Content { get; set; }
        public byte? SignupEnabled { get; set; }
        public byte? CheckinEnabled { get; set; }
        public int? Duration { get; set; }
    }

}
