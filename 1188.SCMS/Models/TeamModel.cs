using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using _1188.SCMS.Web;

namespace _1188.SCMS.Models
{
    public class TeamModel 
    {
        public int ID { get; set; }

        [StringLength(70, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        public bool Checked { get; set; }
      
    }
}
