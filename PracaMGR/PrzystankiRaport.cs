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
    
    public partial class PrzystankiRaport
    {
        public int Id { get; set; }
        public int Numer_Przystanek { get; set; }
        public decimal Wystapienia { get; set; }
    
        public virtual Przystanki Przystanki { get; set; }
    }
}