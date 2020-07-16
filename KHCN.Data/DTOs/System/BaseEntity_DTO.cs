using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.DTOs.System
{
    public class BaseEntity_DTO
    {
        [Column(Order = 1)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Column(Order = 101)]
        [Display(Name = "Ngày tạo")]
        public string CreatedDate { get; set; }

        [Column(Order = 102)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Column(Order = 103)]
        [Display(Name = "Ngày sửa")]
        public string UpdatedDate { get; set; }

        [Column(Order = 104)]
        [Display(Name = "Người sửa")]
        public string UpdatedBy { get; set; }
    }
}
