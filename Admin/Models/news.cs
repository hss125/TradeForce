//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Admin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class news
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LangTitle { get; set; }
        public string Content { get; set; }
        public string Img { get; set; }
        public Nullable<System.DateTime> NewsDate { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
        public Nullable<int> IsDelete { get; set; }
        public string Lang { get; set; }
    }
}
