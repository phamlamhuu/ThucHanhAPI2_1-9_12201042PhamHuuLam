﻿using System.ComponentModel.DataAnnotations;

namespace ThucHanhAPI2.Model.Domain
{
    public class Publisher
    {
        [Key]
        public int Id {  get; set; }
        public string? Name { get; set; }

        public List<Book>? Books { get; set; }
    }
}
