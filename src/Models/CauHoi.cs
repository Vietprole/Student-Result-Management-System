using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Result_Management_System.Models;

public class CauHoi
{
    public int Id { get; set; }
    public string Ten { get; set; } = string.Empty;
    [Column(TypeName = "decimal(4, 2)")]
    public decimal TrongSo { get; set; }
    public int BaiKiemTraId { get; set; }
    public int ThangDiem { get; set; }
    public BaiKiemTra BaiKiemTra { get; set; } = null!;
    public List<CLO> CLOs { get; set; } = [];
}
