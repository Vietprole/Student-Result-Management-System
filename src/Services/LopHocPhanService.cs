using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Student_Result_Management_System.Data;
using Student_Result_Management_System.DTOs.BaiKiemTra;
using Student_Result_Management_System.DTOs.GiangVien;
using Student_Result_Management_System.DTOs.LopHocPhan;
using Student_Result_Management_System.DTOs.SinhVien;
using Student_Result_Management_System.Interfaces;
using Student_Result_Management_System.Mappers;
using Student_Result_Management_System.Models;
using Student_Result_Management_System.Utils;

namespace Student_Result_Management_System.Services
{
    public class LopHocPhanService : ILopHocPhanService
    {
        private readonly ApplicationDBContext _context;
        public LopHocPhanService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<LopHocPhan>> GetAllLopHocPhansAsync()
        {
            var lopHocPhans =await _context.LopHocPhans.ToListAsync();
            return lopHocPhans;
        }

        public async Task<List<LopHocPhan>> GetFilteredLopHocPhansAsync(int? hocPhanId, int? hocKyId)
        {
            IQueryable<LopHocPhan> query = _context.LopHocPhans;

            if (hocPhanId.HasValue)
            {
                query = query.Where(lhp => lhp.HocPhanId == hocPhanId.Value);
            }

            if (hocKyId.HasValue)
            {
                query = query.Where(lhp => lhp.HocKyId == hocKyId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<LopHocPhan?> GetLopHocPhanByIdAsync(int id)
        {
            var lopHocPhan = await _context.LopHocPhans.FindAsync(id);
            return lopHocPhan;
        }

        public async Task<LopHocPhan> CreateLopHocPhanAsync(CreateLopHocPhanDTO lopHocPhanDTO)
        {
            var lopHocPhan = lopHocPhanDTO.ToLopHocPhanFromCreateDTO();
            await _context.LopHocPhans.AddAsync(lopHocPhan);
            await _context.SaveChangesAsync();
            return lopHocPhan;
        }

        public async Task<LopHocPhan?> UpdateLopHocPhanAsync(int id, UpdateLopHocPhanDTO lopHocPhanDTO)
        {
            var lopHocPhan = await _context.LopHocPhans.FindAsync(id) ?? throw new NotFoundException("Không tìm thấy Lớp học phần");
            lopHocPhanDTO.ToLopHocPhanFromUpdateDTO(lopHocPhan);
            await _context.SaveChangesAsync();
            return lopHocPhan;
        }

        public async Task<bool> DeleteLopHocPhanAsync(int id)
        {
            var lopHocPhan = await _context.LopHocPhans.FindAsync(id) ?? throw new NotFoundException("Không tìm thấy Lớp học phần");
            try {
                _context.LopHocPhans.Remove(lopHocPhan);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new BusinessLogicException("Lớp học phần chứa các đối tượng con, không thể xóa");
            }
            return true;
        }

        public async Task<List<SinhVien>> GetSinhViensInLopHocPhanAsync(int lopHocPhanId)
        {
            var lopHocPhan = await _context.LopHocPhans.Include(c => c.SinhViens).ThenInclude(x => x.TaiKhoan).FirstOrDefaultAsync(s => s.Id == lopHocPhanId) ?? throw new NotFoundException("Không tìm thấy Lớp học phần");
            var sinhViens = lopHocPhan.SinhViens.ToList();
            return sinhViens;
        }

        public async Task<List<SinhVien>> AddSinhViensToLopHocPhanAsync(int lopHocPhanId, int[] sinhVienIds)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.SinhViens)
                .ThenInclude(t => t.TaiKhoan)
                .FirstOrDefaultAsync(lhp => lhp.Id == lopHocPhanId) ?? throw new NotFoundException("Không tìm thấy Lớp học phần");
            
            foreach (var sinhVienId in sinhVienIds)
            {
                var sinhVien = await _context.SinhViens.FindAsync(sinhVienId);
                if (sinhVien == null) continue;
                if (!lopHocPhan.SinhViens.Contains(sinhVien))
                {
                    lopHocPhan.SinhViens.Add(sinhVien);
                }
            }

            await _context.SaveChangesAsync();
            return [.. lopHocPhan.SinhViens];
        }

        public async Task<List<SinhVien>> UpdateSinhViensInLopHocPhanAsync(int lopHocPhanId, int[] sinhVienIds)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(p => p.SinhViens)
                .ThenInclude(t => t.TaiKhoan)
                .FirstOrDefaultAsync(p => p.Id == lopHocPhanId) ?? throw new NotFoundException("Không tìm thấy lớp học phần");
            
            var sinhVienSet = new HashSet<int>(sinhVienIds);
            var sinhViens = await _context.SinhViens
                .Where(sv => sinhVienSet.Contains(sv.Id))
                .ToListAsync();

            lopHocPhan.SinhViens.Clear();
            lopHocPhan.SinhViens.AddRange(sinhViens);

            await _context.SaveChangesAsync();

            return [.. lopHocPhan.SinhViens];
        }

        public async Task<List<SinhVien>> RemoveSinhVienFromLopHocPhanAsync(int lopHocPhanId, int sinhVienId)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.SinhViens)
                .FirstOrDefaultAsync(lhp => lhp.Id == lopHocPhanId) ?? throw new NotFoundException("Không tìm thấy lớp học phần");
            
            var sinhVien = await _context.SinhViens.FindAsync(sinhVienId) ?? throw new NotFoundException($"Không tìm thấy sinh viên có id: {sinhVienId}");
            if (!lopHocPhan.SinhViens.Contains(sinhVien)) throw new BusinessLogicException($"Sinh viên có id: {sinhVienId} không học trong lớp học phần này");

            lopHocPhan.SinhViens.Remove(sinhVien);
            await _context.SaveChangesAsync();
            return [.. lopHocPhan.SinhViens];
        }

        public Task<string> CheckCongThucDiem(List<CreateBaiKiemTraDTO> createBaiKiemTraDTOs)
        {
            decimal sum = 0;
            foreach(CreateBaiKiemTraDTO i in createBaiKiemTraDTOs)
            {
                sum += i.TrongSo ?? 0;
            }
            if(sum!=1)
            {
                return Task.FromResult("Tổng trọng số phải bằng 1");
            }
            return Task.FromResult("OK");
        }

        //public async Task<DateTime?> CapNhatNgayChapNhanCTD(int lopHocPhanId, string tenNguoiChapNhanCTD)
        //{
        //    var lopHocPhan =await _context.LopHocPhans.FirstOrDefaultAsync(s => s.Id == lopHocPhanId);
        //    if (lopHocPhan == null)
        //    {
        //        return null;
        //    }
        //    lopHocPhan.TenNguoiChapNhanCTD = tenNguoiChapNhanCTD;
        //    lopHocPhan.NgayChapNhanCTD = DateTime.Now.Date;
        //    await _context.SaveChangesAsync();
        //    return lopHocPhan.NgayChapNhanCTD;

        //}

        //public async Task<DateTime?> CapNhatNgayXacNhanCTD(int lopHocPhanId, string tenNguoiXacNhanCTD)
        //{
        //    var lopHocPhan = await _context.LopHocPhans.FirstOrDefaultAsync(s => s.Id == lopHocPhanId);
        //    if (lopHocPhan == null)
        //    {
        //        return null;
        //    }
        //    lopHocPhan.TenNguoiXacNhanCTD = tenNguoiXacNhanCTD;
        //    lopHocPhan.NgayXacNhanCTD = DateTime.Now.Date;
        //    await _context.SaveChangesAsync();
        //    return lopHocPhan.NgayXacNhanCTD;
        //}
    }
}