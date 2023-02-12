using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.DTOs
{
        //Create a Tag Request
    
    public record CreateTagRequest
    {
        public string Name { get; set; } = string.Empty;
    }
    //Create a Tag Response
    
    public record CreateTagResponse : CreateTagRequest
    {
        public int Id { get; set; }
    }
    //Tag Single Resource Request
    public record TagInfoRequest
    {
        public string Name { get; set; } = string.Empty;
    }
        

    //Tag Response.
    public record TagInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProfileId { get; set; }
        public int? TotalArtilcesUsed { get; set; }
    }

    //Tag Update Request
    public record UpdateTagRequest : CreateTagRequest
    {
        public int? Id { get; set; }
    }
}