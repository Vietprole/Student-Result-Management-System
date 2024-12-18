using System;
using Student_Result_Management_System.DTOs.GiangVien;
using Student_Result_Management_System.Models;

namespace Student_Result_Management_System.Mappers;

public static class GiangVienMapper
{
    public static GiangVienDTO ToGiangVienDTO(this GiangVien giangVien)
    {
        return new GiangVienDTO
        {
            Id = giangVien.Id,
            Ten=giangVien.TaiKhoan.HovaTen,
            KhoaId = giangVien.KhoaId,
        };
    }

    public static GiangVien ToGiangVienFromCreateDTO(this CreateGiangVienDTO createGiangVienDTO)
    {
        return new GiangVien
        {
            KhoaId = createGiangVienDTO.KhoaId,
        };
    }

    public static GiangVien ToGiangVienFromUpdateDTO(this UpdateGiangVienDTO updateGiangVienDTO)
    {
        return new GiangVien
        {
            
        };
    }
}   
