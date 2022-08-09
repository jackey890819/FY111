using FY111.Models.FY111;
using System.Collections.Generic;

namespace FY111.Dtos
{
    public class SignupManageDto
    {
        public SignupManageDto(List<training> trainings, List<Class> classes)
        {
            Trainings = trainings;
            Classes = classes;
        }

        public IEnumerable<training> Trainings { get; set; }
        public IEnumerable<Class> Classes { get; set; }
    }
}
