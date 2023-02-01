using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Common;

namespace LibraryManagementSystem.Models
{
    public class BookInfo
    {
        public int Id { get; set; }

        [Required] public string BookName { get; set; }

        [Required] public string AuthorName { get; set; }

        public CommonEnums.Status Status { get; set; }
    }
}