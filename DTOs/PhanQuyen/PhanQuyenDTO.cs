using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Result_Management_System.DTOs.PhanQuyen
{
    public class PhanQuyenDTO
    {
        public int Id {get; set;}
        public string TenQuyen {get; set;} = string.Empty;
        public int ChucVuId {get; set;}
    }
}