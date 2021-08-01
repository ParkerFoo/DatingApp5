using System;
using System.Collections.Generic;


namespace API.DTO
{
    public class MemberDto
    {
        public int Id { get; set; } //EF will be able to indetify this is the PK, just make sure call it as 'Id'. Will auto increment
        public string Username { get; set; }    
        public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; } 
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }     
    }
}