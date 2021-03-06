﻿using System;
using System.Diagnostics.Contracts;

namespace UserZoom.Shared
{
    [ContractClass(typeof(ICanBeIdentifiableContractClass<>))]
    public interface ICanBeIdentifiable<TId> : IEquatable<ICanBeIdentifiable<TId>>
        where TId : IEquatable<TId>
    {
        TId Id { get; set; }
    }
}
