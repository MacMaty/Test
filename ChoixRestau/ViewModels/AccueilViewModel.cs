using ChoixRestau.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixRestau.ViewModels
{
    public class AccueilViewModel
    {
        [Display(Name = "Le Message")]
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public List<Resto> lesResto { get; set; }
        public Resto Resto { get; set; }
        public string Login { get; set; }
    }
}