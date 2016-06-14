using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHost.Dto
{
    public sealed class UZTaskCreationDto
    {
        [Required, MinLength(2)]
        public string Title { get; set; }
        public List<string> Items { get; set; }
    }
}
