using LibraryManagementSystem.Common;

namespace LibraryManagementSystem.Models
{
    public class ViewBorrowInfo
    {
        public int BookInfoId { get; set; }
        public long BorrowInfoId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
    }
}