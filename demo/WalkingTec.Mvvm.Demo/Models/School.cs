using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Demo.Models
{
    public enum SchoolTypeEnum
    {
        [Display(Name = "公立学校")]
        PUB,
        [Display(Name = "私立学校")]
        PRI
    }

    public class School : BasePoco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int ID { get; set; }

        [Display(Name = "学校编码")]
        [Required(ErrorMessage = "{0}是必填项")]
        [RegularExpression("^[0-9]{3,3}$", ErrorMessage = "{0}必须是3位数字")]
        public string SchoolCode { get; set; }

        [Display(Name = "学校名称")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        [Required(ErrorMessage = "{0}是必填项")]
        [Column("SchoolName2")]
        public string SchoolName { get; set; }

        [Display(Name = "学校类型")]
        [Required(ErrorMessage = "{0}是必填项")]
        public SchoolTypeEnum? SchoolType { get; set; }

        [Display(Name = "备注")]
        [Required(ErrorMessage = "Validate.{0}required")]
        [JsonConverter(typeof(JsonStringConverter))]
        public string Remark { get; set; }

        [Display(Name = "时间")]
        public TimeSpan Duration { get; set; }

        [Display(Name = "级别")]
        public int Level { get; set; }

        [Display(Name = "专业")]
        public List<Major> Majors { get; set; }

        [Display(Name = "多照片")]
        public List<SchoolPhoto> Photos { get; set; }

        [Display(Name = "多附件")]
        public List<SchoolFiles> Files { get; set; }

        [Display(Name = "照片")]
        public Guid? PhotoId { get; set; }

        [Display(Name = "照片")]
        public FileAttachment Photo { get; set; }

        [Display(Name = "附件")]
        public Guid? FileId { get; set; }

        [Display(Name = "附件")]
        public FileAttachment File { get; set; }

        public City Location { get; set; }

        public Guid? CityId { get; set; }
    }

    public class SchoolPhoto : TopBasePoco, ISubFile
    {
        public int SchoolId { get; set; }
        public School School { get; set; }

        public Guid FileId { get; set; }
        public FileAttachment File { get; set; }
        public int Order { get; set; }
    }

    public class SchoolFiles : TopBasePoco, ISubFile
    {
        public int SchoolId { get; set; }
        public School School { get; set; }

        public Guid FileId { get; set; }
        public FileAttachment File { get; set; }
        public int Order { get; set; }
    }

}
