using System;
using System.Collections.Generic;
using FY111.Models.FY111;

namespace FY111.Dtos
{

    public class OperationUnitLogDto // SetOperationRecord
    {
        public string unit_code { get; set; }
    }

    public class CheckPointDto // SetOperationCheckpoint and GetOperationCheckpoint
    {
        public string CKPT_id { get; set; }
        public int? PointType { get; set; }

        public CheckPointDto() { }
        public CheckPointDto(OperationCheckpoint ckpt)
        {
            CKPT_id = ckpt.CkptId;
            PointType = ckpt.PointType;
        }
    }
    public class OperationLittleUnitCheckPoinDto_2 // GetOperationCheckpoint
    {
        public string little_unit_code { get; set; }
        public ICollection<CheckPointDto> CheckPoint { get; set; }

        public OperationLittleUnitCheckPoinDto_2()
        {
        }
    }

    public class OperationLittleUnitCheckPointDto // SetOperationCheckpoint
    {
        public string code { get; set; }
        public ICollection<CheckPointDto> CheckPoint { get; set; }
    }

    public class OperationCheckPointDto // SetOperationCheckpoint
    {
        public string unit_code { get; set; }
        public ICollection<OperationLittleUnitCheckPointDto> little_unit { get; set; }
    }

    public class OperationLittleUnitDisasterDto // SetOperationDisaster
    {
        public string code { get; set; }
        public ICollection<string> OccDisaster { get; set; }
    }

    public class OperationDisasterDto // SetOperationDisaster
    {
        public string unit_code { get; set; }
        public ICollection<OperationLittleUnitDisasterDto> little_unit { get; set; }
    }
    
    public class OccdisasterDto // GetOperationDisaster
    {
        public string dd_code { get; set; }
        public string dd_content { get; set; }
        public OccdisasterDto(Occdisaster occdisaster)
        {
            dd_code = occdisaster.Code;
            dd_content = occdisaster.Content;
        }
    }

    public class ScoreListDataDto // GetOperationDisaster
    {
        public ICollection<OccdisasterDto> NewestData { get; set; }
        public ICollection<OccdisasterDto> HighestData { get; set; }
        public ScoreListDataDto()
        {
            NewestData = new HashSet<OccdisasterDto>();
            HighestData = new HashSet<OccdisasterDto>();
        }
    }

    public class OperationLittleUnitDisasterDto_2 // GetOperationDisaster
    {
        public string little_unit_code { get; set; }
        public ScoreListDataDto ScoreListData { get; set; }
        public OperationLittleUnitDisasterDto_2()
        {
            ScoreListData = new ScoreListDataDto();
        }
    }
    public class OperationDisasterDto_2 // GetOperationDisaster
    {
        public string unit_code { get; set; }
        public ICollection<OperationLittleUnitDisasterDto_2> little_unit { get; set; }
        public OperationDisasterDto_2()
        {
            little_unit = new HashSet<OperationLittleUnitDisasterDto_2>();
        }
    }

    public class ScoreData // SetOperationResult
    {
        public int score { get; set; }
        public string pass { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public TimeSpan TestTimeLast { get; set; }
    }

    public class OperationLittleUnitResultDto // SetOperationResult
    {
        public string code { get; set; }
        public ScoreData ScoreData { get; set; }
    }

    public class OperationResultDto // SetOperationResult
    {
        public string unit_code { get; set; }
        public ICollection<OperationLittleUnitResultDto> little_unit { get; set; }
    }

    public class ScoreData_2 // GetOperationResult and GetLastScore
    {
        public int score { get; set; }
        public bool pass { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public TimeSpan? TestTimeLast { get; set; }
    }

    public class OperationLittleUnitResultDto_2 // GetOperationResult
    {
        public string little_unit_code { get; set; }
        public ScoreData_2 ScoreData { get; set; }
        public OperationLittleUnitResultDto_2()
        {
            ScoreData = new ScoreData_2();
        }
    }

    public class OperationResultDto_2 // GetOperationResult
    {
        public string unit_code { get; set; }
        public bool unit_pass { get; set; }
        public ICollection<OperationLittleUnitResultDto_2> ScoreState { get; set; }
        public OperationResultDto_2()
        {
            ScoreState = new List<OperationLittleUnitResultDto_2>();
        }
    }

    public class ScoreListDataDto_2 // GetLastScore
    {
        public ScoreData_2 NewestData { get; set; }
        public ScoreData_2 HighestData { get; set; }
        public ScoreListDataDto_2()
        {
            NewestData = new ScoreData_2();
            HighestData = new ScoreData_2();
        }
    }

    public class OperationLittleUnitResultDto_3 // GetLastScore
    {
        public string little_unit_code { get; set; }
        public ScoreListDataDto_2 ScoreListData { get; set; }
        public OperationLittleUnitResultDto_3()
        {
            ScoreListData = new ScoreListDataDto_2();
        }
    }

    public class OperationResultDto_3 // GetLastScore
    {
        public string unit_code { get; set; }
        public ICollection<OperationLittleUnitResultDto_3> little_unit { get; set; }

        public OperationResultDto_3()
        {
            little_unit = new List<OperationLittleUnitResultDto_3>();
        }
    }
}
