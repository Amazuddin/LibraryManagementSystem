using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Common;

namespace LibraryManagementSystem.Models
{
    public class BorrowInfo
    {
        public long Id { get; set; }

        [ForeignKey("UserId")] 
        public User User { get; set; }
        public int UserId { get; set; }

        [ForeignKey("BookInfoId")] 
        public BookInfo BookInfo { get; set; }
        public int BookInfoId { get; set; }

        public CommonEnums.BooleanState IsReturn { get; set; }
    }
}