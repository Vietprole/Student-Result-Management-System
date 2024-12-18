using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Result_Management_System.Models;

public class KetQua
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(4, 2)")]
    public decimal Diem { get; set; }
    public int SinhVienId { get; set; }
    public SinhVien SinhVien { get; set; } = null!;
    public int CauHoiId { get; set; }
    public CauHoi CauHoi { get; set; } = null!;
}
