using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Result_Management_System.Data;
using Student_Result_Management_System.DTOs.LopHocPhan;
using Student_Result_Management_System.Mappers;
using Student_Result_Management_System.Models;

namespace Student_Result_Management_System.Controllers
{
    [Route("api/lophocphan")]
    [ApiController]
    [Authorize]
    public class LopHocPhanController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public LopHocPhanController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        // IActionResult return any value type
        // public async Task<IActionResult> Get()
        // ActionResult return specific value type, the type will displayed in Schemas section
        public async Task<IActionResult> GetAll([FromQuery] int? hocPhanId) // async go with Task<> to make function asynchronous
        {
            IQueryable<LopHocPhan> query = _context.LopHocPhans;
            if (hocPhanId.HasValue)
            {
                query = query.Where(n => n.HocPhanId == hocPhanId.Value);
            }

            var lopHocPhans = await query.ToListAsync();
            var lopHocPhanDTOs = lopHocPhans.Select(sv => sv.ToLopHocPhanDTO()).ToList();
            return Ok(lopHocPhanDTOs);
        }

        [HttpGet("{id}")]
        // Get single entry
        public async Task<IActionResult> GetById([FromRoute] int id) // async go with Task<> to make function asynchronous
        {
            var student = await _context.LopHocPhans.FindAsync(id);
            if (student == null)
                return NotFound();
            var studentDTO = student.ToLopHocPhanDTO();
            return Ok(studentDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLopHocPhanDTO createLopHocPhanDTO)
        {
            var lopHocPhan = createLopHocPhanDTO.ToLopHocPhanFromCreateDTO();
            await _context.LopHocPhans.AddAsync(lopHocPhan);
            await _context.SaveChangesAsync();
            var lopHocPhanDTO = lopHocPhan.ToLopHocPhanDTO();
            return CreatedAtAction(nameof(GetById), new { id = lopHocPhan.Id }, lopHocPhanDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLopHocPhanDTO updateLopHocPhanDTO)
        {
            var lopHocPhanToUpdate = await _context.LopHocPhans.FindAsync(id);
            if (lopHocPhanToUpdate == null)
                return NotFound();

            lopHocPhanToUpdate.Ten = updateLopHocPhanDTO.Ten;
            lopHocPhanToUpdate.HocPhanId = updateLopHocPhanDTO.HocPhanId;
            
            await _context.SaveChangesAsync();
            var studentDTO = lopHocPhanToUpdate.ToLopHocPhanDTO();
            return Ok(studentDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var lopHocPhanToDelete = await _context.LopHocPhans.FindAsync(id);
            if (lopHocPhanToDelete == null)
                return NotFound();
            _context.LopHocPhans.Remove(lopHocPhanToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/view-sinhviens")]
        public async Task<IActionResult> GetSinhViens([FromRoute] int id)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.SinhViens)
                .ThenInclude(sv => sv.TaiKhoan)
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (lopHocPhan == null)
                return NotFound();

            var sinhVienDTOs = lopHocPhan.SinhViens.Select(sv => sv.ToSinhVienDTO()).ToList();
            return Ok(sinhVienDTOs);
        }


        [HttpPost("{id}/add-sinhviens")]
        public async Task<IActionResult> AddSinhViens([FromRoute] int id, [FromBody] int[] sinhVienIds)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.SinhViens) // Include SinhViens to ensure the collection is loaded
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (lopHocPhan == null)
                return NotFound("LopHocPhan not found");

            foreach (var sinhVienId in sinhVienIds)
            {
                var sinhVien = await _context.SinhViens.FindAsync(sinhVienId);
                if (sinhVien == null)
                    return NotFound($"SinhVien with ID {sinhVienId} not found");

                // Check if the SinhVien is already in the LopHocPhan to avoid duplicates
                if (!lopHocPhan.SinhViens.Contains(sinhVien))
                {
                    lopHocPhan.SinhViens.Add(sinhVien);
                }
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSinhViens), new { id = lopHocPhan.Id }, lopHocPhan.SinhViens.Select(sv => sv.ToSinhVienDTO()).ToList());
        }

        [HttpPut("{id}/update-sinhviens")]
        public async Task<IActionResult> UpdateSinhViens([FromRoute] int id, [FromBody] int[] sinhVienIds)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(p => p.SinhViens)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (lopHocPhan == null)
                return NotFound("Lop hoc phan not found");

            // Get existing SinhVien IDs
            var existingSinhVienIds = lopHocPhan.SinhViens.Select(c => c.Id).ToList();
            
            // Find IDs to add and remove
            var idsToAdd = sinhVienIds.Except(existingSinhVienIds);
            var idsToRemove = existingSinhVienIds.Except(sinhVienIds);

            // Remove SinhViens
            foreach (var removeId in idsToRemove)
            {
                var sinhVienToRemove = lopHocPhan.SinhViens.First(c => c.Id == removeId);
                lopHocPhan.SinhViens.Remove(sinhVienToRemove);
            }

            // Add new SinhViens
            foreach (var addId in idsToAdd)
            {
                var sinhVien = await _context.SinhViens.FindAsync(addId);
                if (sinhVien == null)
                    return NotFound($"SinhVien with ID {addId} not found");
                    
                lopHocPhan.SinhViens.Add(sinhVien);
            }

            await _context.SaveChangesAsync();
            return Ok(lopHocPhan.SinhViens.Select(c => c.ToSinhVienDTO()).ToList());
        }

        [HttpDelete("{id}/remove-sinhvien/{sinhVienId}")]
        public async Task<IActionResult> RemoveSinhVien([FromRoute] int id, [FromRoute] int sinhVienId)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.SinhViens) // Include SinhViens to ensure the collection is loaded
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (lopHocPhan == null)
                return NotFound("LopHocPhan not found");

            var sinhVien = await _context.SinhViens.FindAsync(sinhVienId);
            if (sinhVien == null)
                return NotFound($"SinhVien with ID {sinhVienId} not found");

            // Check if the HocPhan is in the CTDT to avoid removing non-existing HocPhan
            if (!lopHocPhan.SinhViens.Contains(sinhVien))
            {
                return NotFound($"SinhVien with ID {sinhVienId} is not found in LopHocPhan with ID {id}");
            }
            lopHocPhan.SinhViens.Remove(sinhVien);

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSinhViens), new { id = lopHocPhan.Id }, lopHocPhan.SinhViens.Select(sv => sv.ToSinhVienDTO()).ToList());
        }

        [HttpGet("{id}/view-giangviens")]
        public async Task<IActionResult> GetGiangViens([FromRoute] int id)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.GiangViens)
                .ThenInclude(gv => gv.TaiKhoan)
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (lopHocPhan == null)
                return NotFound();

            var giangVienDTOs = lopHocPhan.GiangViens.Select(sv => sv.ToGiangVienDTO()).ToList();
            return Ok(giangVienDTOs);
        }


        [HttpPost("{id}/add-giangviens")]
        public async Task<IActionResult> AddGiangViens([FromRoute] int id, [FromBody] int[] giangVienIds)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.GiangViens) // Include GiangViens to ensure the collection is loaded
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (lopHocPhan == null)
                return NotFound("LopHocPhan not found");

            foreach (var giangVienId in giangVienIds)
            {
                var giangVien = await _context.GiangViens.FindAsync(giangVienId);
                if (giangVien == null)
                    return NotFound($"GiangVien with ID {giangVienId} not found");

                // Check if the GiangVien is already in the LopHocPhan to avoid duplicates
                if (!lopHocPhan.GiangViens.Contains(giangVien))
                {
                    lopHocPhan.GiangViens.Add(giangVien);
                }
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGiangViens), new { id = lopHocPhan.Id }, lopHocPhan.GiangViens.Select(sv => sv.ToGiangVienDTO()).ToList());
        }

        [HttpPut("{id}/update-giangviens")]
        public async Task<IActionResult> UpdateGiangViens([FromRoute] int id, [FromBody] int[] giangVienIds)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(p => p.GiangViens)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (lopHocPhan == null)
                return NotFound("Lop hoc phan not found");

            // Get existing GiangVien IDs
            var existingGiangVienIds = lopHocPhan.GiangViens.Select(c => c.Id).ToList();
            
            // Find IDs to add and remove
            var idsToAdd = giangVienIds.Except(existingGiangVienIds);
            var idsToRemove = existingGiangVienIds.Except(giangVienIds);

            // Remove GiangViens
            foreach (var removeId in idsToRemove)
            {
                var giangVienToRemove = lopHocPhan.GiangViens.First(c => c.Id == removeId);
                lopHocPhan.GiangViens.Remove(giangVienToRemove);
            }

            // Add new GiangViens
            foreach (var addId in idsToAdd)
            {
                var giangVien = await _context.GiangViens.FindAsync(addId);
                if (giangVien == null)
                    return NotFound($"GiangVien with ID {addId} not found");
                    
                lopHocPhan.GiangViens.Add(giangVien);
            }

            await _context.SaveChangesAsync();
            return Ok(lopHocPhan.GiangViens.Select(c => c.ToGiangVienDTO()).ToList());
        }

        [HttpDelete("{id}/remove-giangvien/{giangVienId}")]
        public async Task<IActionResult> RemoveGiangVien([FromRoute] int id, [FromRoute] int giangVienId)
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.GiangViens) // Include GiangViens to ensure the collection is loaded
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (lopHocPhan == null)
                return NotFound("LopHocPhan not found");

            var giangVien = await _context.GiangViens.FindAsync(giangVienId);
            if (giangVien == null)
                return NotFound($"GiangVien with ID {giangVienId} not found");

            // Check if the HocPhan is in the CTDT to avoid removing non-existing HocPhan
            if (!lopHocPhan.GiangViens.Contains(giangVien))
            {
                return NotFound($"GiangVien with ID {giangVienId} is not found in LopHocPhan with ID {id}");
            }
            lopHocPhan.GiangViens.Remove(giangVien);

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGiangViens), new { id = lopHocPhan.Id }, lopHocPhan.GiangViens.Select(sv => sv.ToGiangVienDTO()).ToList());
        }
    }
}
