using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingChallenge.Models
{
    public record ScoreReturn
    {
        public List<ValidationError> Errors { get; set; }
        public int Score { get; set; }
    }
}
