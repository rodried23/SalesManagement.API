﻿using System;
using System.Collections.Generic;
using MediatR;

namespace SalesManagement.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        protected EntityBase()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            _domainEvents = new List<INotification>();
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void UpdatedAtNow()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}