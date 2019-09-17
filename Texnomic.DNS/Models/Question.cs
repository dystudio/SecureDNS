﻿using System;
using BinarySerialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Texnomic.DNS.Abstractions;
using Texnomic.DNS.Abstractions.Enums;
using Texnomic.DNS.Extensions;
using Texnomic.DNS.Factories;

namespace Texnomic.DNS.Models
{
    public class Question : IQuestion
    {
        [FieldOrder(0)]
        [SubtypeFactory(nameof(Domain), typeof(DomainFactory), BindingMode = BindingMode.OneWayToSource)]
        //[SubtypeDefault(typeof(Domain))]
        [JsonIgnore]
        public IDomain Domain { get; set; }

        [Ignore]
        [JsonPropertyName("name")]
        public string Name
        {
            get => Domain.Name;
            set => Domain = new Domain(value);
        }

        [FieldOrder(1)]
        [FieldBitLength(16)]
        [FieldEndianness(Endianness.Big)]
        [JsonPropertyName("type")]
        public RecordType Type { get; set; }

        [FieldOrder(2)]
        [FieldBitLength(16)]
        [FieldEndianness(Endianness.Big)]
        [JsonIgnore]
        public RecordClass Class { get; set; }

        public byte[] ToArray()
        {
            var Serializer = new BinarySerializer();
            return Serializer.Serialize(this);
        }

        public async Task<byte[]> ToArrayAsync()
        {
            var Serializer = new BinarySerializer();
            return await Serializer.SerializeAsync(this);
        }
    }
}
