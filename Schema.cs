using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace HitRefresh.IWannaGraduate
{
    public enum Semester
    {
        秋季学期 = 0,
        春节学期 = 1,
        夏季学期 = 2,
        春秋学期 = 3,
        任意学期 = 4
    }

    /// <summary>
    /// 课程
    /// </summary>
    public record Course
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = true)]
        public string Name { get; set; } = string.Empty;
        [YamlMember(Alias = "semester", ApplyNamingConventions = true)]
        public Semester Semester { get; set; } = Semester.任意学期;
        [YamlMember(Alias = "score", ApplyNamingConventions = true)]
        public decimal Score { get; set; } = 0;
    }
    /// <summary>
    /// 培养方案要求
    /// </summary>
    public class Schema
    {
        [YamlMember(Alias = "type", ApplyNamingConventions = true)]
        public string Type { get; set; } = string.Empty;
        [YamlMember(Alias = "generic", ApplyNamingConventions = true)]
        public bool Generic { get; set; } = false;
        [YamlMember(Alias = "required", ApplyNamingConventions = true)]
        public List<Course> Required { get; set; } = new();
    }
    /// <summary>
    /// 完成情况
    /// </summary>
    public class AcquiredType : Schema
    {
        [YamlMember(Alias = "acquired", ApplyNamingConventions = true)]
        public List<Course> Acquired { get; set; } = new();

        public IEnumerable<Course> GetNotSatisfied()
        {
            if (!Generic) return Required.Except(Acquired);
            var satisfiedScore = Acquired.Sum(c => c.Score);
            return new Course[]{new()
            {
                Name = Required[0].Name,
                Score = Required[0].Score - satisfiedScore,
                Semester = Required[0].Semester
            }};

        }
    }
}
