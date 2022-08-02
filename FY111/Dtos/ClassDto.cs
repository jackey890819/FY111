using FY111.Models.FY111;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FY111.Dtos
{
    public class C
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string image { get; set; }

    }

    public class ClassDto
    {
        public C @class { get; set; }
        public ICollection<ClassUnitDto> units { get; set; }

        public ClassDto(Class c)
        {
            @class = new C();
            @class.id = c.Id;
            @class.code = c.Code;
            @class.name = c.Name;
            @class.content = c.Content;
            @class.image = c.Image;
        }
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
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public byte? SignupEnabled { get; set; }
        public byte? CheckinEnabled { get; set; }
        public int? Duration { get; set; }
    }
    public class UnitDto
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public ICollection<ClassLittleUnitDto> littleUnits { get; set; }
    }
    public class ClassUnitDto
    {
        public UnitDto unit { get; set; }
        public ClassUnitDto(ClassUnit u)
        {
            unit = new UnitDto();
            unit.id = u.Id;
            unit.code = u.Code;
            unit.name = u.Name;
            unit.image = u.Image;
        }
    }
    public class ClassLittleUnitDto
    {
        public string code { get; set; }
        public string name { get; set; }
        public string image { get; set; }

        public ClassLittleUnitDto(ClassLittleunit lu)
        {
            code = lu.Code;
            name = lu.Name;
            image = lu.Image;
        }
    }
}
