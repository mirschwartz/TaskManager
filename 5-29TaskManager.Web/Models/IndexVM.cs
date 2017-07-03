using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Data;

namespace _5_29TaskManager.Web.Models
{
    public class IndexVM
    {
        public IEnumerable<Task> Tasks { get; set; }
        public User User { get; set; }
    }
}