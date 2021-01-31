using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingChallenge.Models
{
    public record ValidationError
    {
        public string Message { get; set; }
        public int FrameNum { get; set; }
        public int RollNum { get; set; }
    }
}
