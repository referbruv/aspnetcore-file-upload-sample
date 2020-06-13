using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ReaderStore.WebApp.Models
{
    public class ReaderResponseModel
    {
        public bool IsSuccess { get; set; }
        public string WorkPath { get; set; }
        public string ReaderId { get; set; }
        public string RedirectTo { get; set; }
    }

    public class ReaderRequestModel : ReaderResponseModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(250)]
        public string EmailAddress { get; set; }
        public IFormFile Work { get; set; }
    }
}