//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PracaMGR
{
    using System;
    using System.Collections.Generic;
    
    public partial class Trasy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trasy()
        {
            this.TrasyRaport = new HashSet<TrasyRaport>();
        }
    
        public int Id { get; set; }
        public string Linia { get; set; }
        public string Przystanki { get; set; }
        public string Czas { get; set; }
        public string Typ { get; set; }
        public string Notka { get; set; }
        public string Znacznik { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrasyRaport> TrasyRaport { get; set; }
    }
}