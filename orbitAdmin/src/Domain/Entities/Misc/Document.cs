using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.ExtendedAttributes;
using System;

namespace SchoolV01.Domain.Entities.Misc
{
    public class Document : AuditableEntityWithExtendedAttributes<int, int, Document, DocumentExtendedAttribute>
    {
        public string Title { get; set; }
        public int? Number { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string User { get; set; }
        public string URL { get; set; }
        public int? DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}