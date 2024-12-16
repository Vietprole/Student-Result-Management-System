using System;
using Student_Result_Management_System.DTOs.SinhVien;
using Student_Result_Management_System.Models;

namespace Student_Result_Management_System.Mappers;

public static class SinhVienMapper
{
    public static SinhVienDTO ToSinhVienDTO(this SinhVien sinhVienModel)
    {
        return new SinhVienDTO
        {
            Id = sinhVienModel.Id,
            Ten = sinhVienModel.TaiKhoan?.Ten ?? string.Empty,
            KhoaId = sinhVienModel.KhoaId, //Để đây hồi tin sửa
            TenKhoa = sinhVienModel.Khoa?.Ten ?? string.Empty,
            NamNhapHoc = sinhVienModel.NamNhapHoc
        };
    }

    public static SinhVien ToSinhVienFromCreateDTO(this CreateSinhVienDTO createSinhVienDTO)
    {
        return new SinhVien
        {
            KhoaId = createSinhVienDTO.KhoaId,
            NamNhapHoc = createSinhVienDTO.NamNhapHoc
        };
    }
}
