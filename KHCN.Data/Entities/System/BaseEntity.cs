using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    public class BaseEntity
    {
        [Column(Order = 1)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 101)]
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 102)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Column(Order = 103)]
        [Display(Name = "Ngày sửa")]
        public DateTime UpdatedDate { get; set; }

        [Column(Order = 104)]
        [Display(Name = "Người sửa")]
        public string UpdatedBy { get; set; }
    }
}
