using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace HitRefresh.IWannaGraduate
{
    [SuitInfo("我想毕业")]
    public class Program : SuitClient
    {
        private AcquiredType[] _acquired;
        public Program()
        {
            var deserializer = new DeserializerBuilder().Build();
            var f = File.ReadAllText("schema.yml");
            var schema = deserializer.Deserialize<Schema[]>(f);
            _acquired = schema.Select(s => new AcquiredType
            {
                Generic = s.Generic,
                Required = s.Required,
                Type = s.Type
            }).ToArray();
        }
        public static void Main(string[] args)
        {
            //MobileSuit映射Program类里的每一个非静态函数为一个指令。整个Program类是单例的。
            Suit.GetBuilder().Build<Program>().Run();
        }
        [SuitInfo("查看学习情况")]
        public void Save()
        {
            if (File.Exists("dump.yml")) File.Delete("dump.yml");
            var serializer = new SerializerBuilder().Build();
            File.WriteAllText("dump.yml", serializer.Serialize(_acquired));
        }
        [SuitInfo("载入学习情况")]
        public void Load()
        {
            if (!File.Exists("dump.yml")) return;
            var deserializer = new DeserializerBuilder().Build();
            _acquired = deserializer.Deserialize<AcquiredType[]>(File.ReadAllText("dump.yml"));
        }
        [SuitInfo("查看学习情况")]
        public void Print()
        {
            IO.WriteLine("待修学分：", ConsoleColor.Red);
            IO.AppendWriteLinePrefix();
            IO.WriteLine("课程类别\t课程名称\t学分\t所属学期", OutputType.ListTitle);
            foreach (var schema in _acquired)
            {
                foreach (var course in schema.GetNotSatisfied())
                {
                    if (course.Score > 0)
                        IO.WriteLine($"{schema.Type}\t{course.Name}\t{course.Score}\t{Enum.GetName(typeof(Semester), course.Semester)}");
                }
            }
            IO.SubtractWriteLinePrefix();
            IO.WriteLine("已修课程：", ConsoleColor.Green);
            IO.AppendWriteLinePrefix();
            IO.WriteLine("课程类别\t课程名称\t学分\t所属学期", OutputType.ListTitle);
            foreach (var schema in _acquired)
            {
                IO.WriteLine($"{schema.Type}: ", OutputType.ListTitle);
                if (schema.Acquired.Any())
                {
                    IO.AppendWriteLinePrefix();
                    IO.WriteLine("课程名称\t学分\t所属学期", OutputType.ListTitle);
                    foreach (var course in schema.Acquired)
                    {
                        IO.WriteLine($"{course.Name}\t{course.Score}\t{Enum.GetName(typeof(Semester), course.Semester)}");
                    }
                    IO.SubtractWriteLinePrefix();
                }

            }
            IO.SubtractWriteLinePrefix();
        }
        [SuitInfo("填写学习情况")]
        public void Fill()
        {
            foreach (var acq in _acquired)
            {
                IO.WriteLine($"填写类别：{acq.Type}的学习情况", OutputType.ListTitle);
                if (acq.Generic)
                {
                    //自选类课程
                    acq.Acquired.Where(course =>
                        IO.ReadLine($"已选课程{course.Name},学分{course.Score},保留吗？y|n", "y") == "n").ToList()
                        .ForEach(c => acq.Acquired.Remove(c));
                    var genericCourse = acq.Required.First();
                    IO.WriteLine($"请填写所有满足[{genericCourse.Name}]的课程，格式'<名称>,<学分>'，一行一个，空行结束");
                    for (; ; )
                    {
                        var line = IO.ReadLine();
                        if (string.IsNullOrEmpty(line)) break;
                        var spl = line.Split(',', '，');
                        acq.Acquired.Add(new()
                        {
                            Name = spl[0],
                            Score = decimal.Parse(spl[1]),
                            Semester = genericCourse.Semester
                        });
                    }
                }
                else
                {
                    foreach (var course in acq.Required)
                    {
                        var recordLearned = acq.Acquired.Contains(course);
                        var learned = IO.ReadLine($"是否已学课程{course.Name},学分{course.Score}？y|n",
                            "y") == "y";
                        if (learned == recordLearned) continue;
                        if (learned) acq.Acquired.Add(course);
                        else acq.Acquired.Remove(course);
                    }
                }

            }
        }
    }
}
