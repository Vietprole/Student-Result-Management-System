using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Result_Management_System.DTOs.HocPhan;

public class CreateHocPhanDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Ten { get; set; } = string.Empty;
    [Required]
    public int SoTinChi { get; set; }
    [Required]
    public bool LaCotLoi { get; set; }
    [Required]
    public int KhoaId { get; set; }
}