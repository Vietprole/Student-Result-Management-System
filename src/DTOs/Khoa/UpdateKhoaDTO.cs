using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Result_Management_System.DTOs.Khoa;

public class UpdateKhoaDTO
{
    [Required]
    public string Ten { get; set; } = string.Empty;
}
