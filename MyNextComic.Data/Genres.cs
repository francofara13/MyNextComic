//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyNextComic.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Genres
    {
        public Genres()
        {
            this.Comics = new HashSet<Comics>();
        }
    
        public int IdGenre { get; set; }
        public string GenreDescription { get; set; }
    
        public virtual ICollection<Comics> Comics { get; set; }
    }
}
