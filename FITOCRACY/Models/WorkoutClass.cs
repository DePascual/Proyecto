using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FITOCRACY.Models
{
    public class WorkoutClass
    {
        public string nombre { get; set; }
        public string puntacion { get; set; }
        public List<string> tracks { get; set; }
    }
}