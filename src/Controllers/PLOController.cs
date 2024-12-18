using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Result_Management_System.Data;
using Student_Result_Management_System.DTOs.PLO;
using Student_Result_Management_System.Mappers;
using Student_Result_Management_System.Models;

namespace Student_Result_Management_System.Controllers
{
    [Route("api/plo")]
    [ApiController]
    [Authorize]
    public class PLOController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public PLOController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        // IActionResult return any value type
        // public async Task<IActionResult> Get()
        // ActionResult return specific value type, the type will displayed in Schemas section
        public async Task<IActionResult> GetAll(
            [FromQuery] int? cTDTId,
            [FromQuery] int? lopHocPhanId)
        {
            IQueryable<PLO> query = _context.PLOs;

            if (cTDTId.HasValue)
            {
                query = query.Where(p => p.CTDTId == cTDTId.Value);
            }

            if (lopHocPhanId.HasValue)
            {
                query = query.Where(p => p.HocPhans.Any(hp => hp.LopHocPhans.Any(lhp => lhp.Id == lopHocPhanId.Value)));            
            }

            var pLOs = await query.ToListAsync();
            var pLODTOs = pLOs.Select(p => p.ToPLODTO()).ToList();
            return Ok(pLODTOs);
        }

        [HttpGet("{id}")]
        // Get single entry
        public async Task<IActionResult> GetById([FromRoute] int id) // async go with Task<> to make function asynchronous
        {
            var student = await _context.PLOs.FindAsync(id);
            if (student == null)
                return NotFound();
            var studentDTO = student.ToPLODTO();
            return Ok(studentDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePLODTO createPLODTO)
        {
            var pLO = createPLODTO.ToPLOFromCreateDTO();
            await _context.PLOs.AddAsync(pLO);
            await _context.SaveChangesAsync();
            var pLODTO = pLO.ToPLODTO();
            return CreatedAtAction(nameof(GetById), new { id = pLO.Id }, pLODTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePLODTO updatePLODTO)
        {
            var pLOToUpdate = await _context.PLOs.FindAsync(id);
            if (pLOToUpdate == null)
                return NotFound();

            pLOToUpdate.Ten = updatePLODTO.Ten;
            pLOToUpdate.MoTa = updatePLODTO.MoTa;
            pLOToUpdate.CTDTId = updatePLODTO.CTDTId;
            
            await _context.SaveChangesAsync();
            var studentDTO = pLOToUpdate.ToPLODTO();
            return Ok(studentDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var pLOToDelete = await _context.PLOs.FindAsync(id);
            if (pLOToDelete == null)
                return NotFound();
            _context.PLOs.Remove(pLOToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/view-clos")]
        public async Task<IActionResult> GetCLOs([FromRoute] int id)
        {
            var pLO = await _context.PLOs
                .Include(lhp => lhp.CLOs)
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (pLO == null)
                return NotFound();

            var cLODTOs = pLO.CLOs.Select(sv => sv.ToCLODTO()).ToList();
            return Ok(cLODTOs);
        }


        [HttpPost("{id}/add-clos")]
        public async Task<IActionResult> AddCLOs([FromRoute] int id, [FromBody] int[] cLOIds)
        {
            var pLO = await _context.PLOs
                .Include(lhp => lhp.CLOs) // Include CLOs to ensure the collection is loaded
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (pLO == null)
                return NotFound("PLO not found");

            foreach (var cLOId in cLOIds)
            {
                var cLO = await _context.CLOs.FindAsync(cLOId);
                if (cLO == null)
                    return NotFound($"CLO with ID {cLOId} not found");

                // Check if the CLO is already in the PLO to avoid duplicates
                if (!pLO.CLOs.Contains(cLO))
                {
                    pLO.CLOs.Add(cLO);
                }
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCLOs), new { id = pLO.Id }, pLO.CLOs.Select(sv => sv.ToCLODTO()).ToList());
        }

        [HttpPut("{id}/update-clos")]
        public async Task<IActionResult> UpdateCLOs([FromRoute] int id, [FromBody] int[] cLOIds)
        {
            var pLO = await _context.PLOs
                .Include(p => p.CLOs)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (pLO == null)
                return NotFound("PLO not found");

            // Get existing CLO IDs
            var existingCLOIds = pLO.CLOs.Select(c => c.Id).ToList();
            
            // Find IDs to add and remove
            var idsToAdd = cLOIds.Except(existingCLOIds);
            var idsToRemove = existingCLOIds.Except(cLOIds);

            // Remove CLOs
            foreach (var removeId in idsToRemove)
            {
                var cloToRemove = pLO.CLOs.First(c => c.Id == removeId);
                pLO.CLOs.Remove(cloToRemove);
            }

            // Add new CLOs
            foreach (var addId in idsToAdd)
            {
                var clo = await _context.CLOs.FindAsync(addId);
                if (clo == null)
                    return NotFound($"CLO with ID {addId} not found");
                    
                pLO.CLOs.Add(clo);
            }

            await _context.SaveChangesAsync();
            return Ok(pLO.CLOs.Select(c => c.ToCLODTO()).ToList());
        }

        [HttpDelete("{id}/remove-clo/{cLOId}")]
        public async Task<IActionResult> RemoveCLO([FromRoute] int id, [FromRoute] int cLOId)
        {
            var pLO = await _context.PLOs
                .Include(lhp => lhp.CLOs) // Include CLOs to ensure the collection is loaded
                .FirstOrDefaultAsync(lhp => lhp.Id == id);
            if (pLO == null)
                return NotFound("PLO not found");

            var cLO = await _context.CLOs.FindAsync(cLOId);
            if (cLO == null)
                return NotFound($"CLO with ID {cLOId} not found");

            // Check if the HocPhan is in the CTDT to avoid removing non-existing HocPhan
            if (!pLO.CLOs.Contains(cLO))
            {
                return NotFound($"CLO with ID {cLOId} is not found in PLO with ID {id}");
            }
            pLO.CLOs.Remove(cLO);

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCLOs), new { id = pLO.Id }, pLO.CLOs.Select(sv => sv.ToCLODTO()).ToList());
        }
    }
}
