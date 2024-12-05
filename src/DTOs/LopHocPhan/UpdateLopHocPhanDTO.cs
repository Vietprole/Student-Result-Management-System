using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Result_Management_System.DTOs.LopHocPhan;

public class UpdateLopHocPhanDTO
{
    [Required]
    public string Ten { get; set; } = string.Empty;
    [Required]
    public int HocPhanId { get; set; }
    [Required]
    public int KiHocId { get; set; }
}
