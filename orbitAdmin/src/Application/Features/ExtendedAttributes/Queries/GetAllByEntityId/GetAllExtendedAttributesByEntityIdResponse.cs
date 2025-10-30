using System;
using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Application.Features.ExtendedAttributes.QueriesByEntityId
{
    public class GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> : IEntityAuditableExtendedAttribute
    {
        public TId Id { get; set; }
        public TEntityId EntityId { get; set; }
        public EntityExtendedAttributeType Type { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public decimal? Decimal { get; set; }
        public DateTime? DateTime { get; set; }
        public string Json { get; set; }
        public string ExternalId { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool Deleted { get; set; } = false;

    }
}