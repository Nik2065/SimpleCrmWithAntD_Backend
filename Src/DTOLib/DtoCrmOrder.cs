using System;

namespace DTOLib
{
    public class DtoCrmOrder
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }
    }
}
