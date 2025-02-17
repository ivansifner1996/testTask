using System.Collections.Generic;

namespace UpriseTask.Mappings.DTO
{
    public class NotFoundResponse
    {
        public string Message { get; set; }
        public IEnumerable<string> Data { get; set; }

    }
}
