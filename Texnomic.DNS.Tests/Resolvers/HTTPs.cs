﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BinarySerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Texnomic.DNS.Abstractions;
using Texnomic.DNS.Abstractions.Enums;
using Texnomic.DNS.Models;
using Texnomic.DNS.Extensions;

namespace Texnomic.DNS.Tests.Resolvers
{
    [TestClass]
    public class HTTPs
    {
        private ushort ID;
        private IProtocol Resolver;
        private IMessage RequestMessage;
        private IMessage ResponseMessage;
        private BinarySerializer BinarySerializer;

        [TestInitialize]
        public void Initialize()
        {
            BinarySerializer = new BinarySerializer();

            Resolver = new Protocols.HTTPs(IPAddress.Parse("8.8.8.8"), "3082010A0282010100968CDA0112B4D30B36AB5DE74D242F9ECFDB6E491CC70672E044ECD148AAB07430C5C3EE2BC81B0EA24405D163B6EFACE5C2693D3B3D7BEFF3B676460814B5AACDAA7ED94CADC9166403FA65951681C4C200B032C65D5CDE5798A86A6131E790EDB3CF02A73696121F40E8F71F3345BD9E2C39F443EE37ABF19ADA88E433F637653113ABB2FBDBB2FC5C83E72108117E6B0482FEE6F2C59ADFF151C4968DB0EAE69142D5159FD614D5C1B0660812EC6B577C074D1800EDDCA18AF125C4CF6A8EDB940014B8512A8BC881441A45799024AB9ECF22AF96666A7124BEE014E2A5BC0708E8310BE18DE910843EAC7EBCC2EC0AD727FF9E3180F7647B064659890AAD0203010001");

            ID = (ushort)new Random().Next();

            RequestMessage = new Message()
            {
                ID = ID,
                RecursionDesired = true,
                Questions = new List<IQuestion>()
                {
                    new Question()
                    {
                        Domain = Domain.FromString("facebook.com"),
                        Class = RecordClass.Internet,
                        Type = RecordType.A
                    }
                }
            };
        }


        [TestMethod]
        public async Task MessageAsync()
        {
            ResponseMessage = await Resolver.ResolveAsync(RequestMessage);

            Assertions();
        }

        [TestMethod]
        public async Task BytesAsync()
        {
            var RequestBytes = await BinarySerializer.SerializeAsync(RequestMessage);

            var ResponseBytes = await Resolver.ResolveAsync(RequestBytes);

            ResponseMessage = await BinarySerializer.DeserializeAsync<Message>(ResponseBytes);

            Assertions();
        }

        public void Assertions()
        {
            Assert.IsNotNull(ResponseMessage);
            Assert.AreEqual(ID, ResponseMessage.ID);
            Assert.IsNotNull(ResponseMessage.Questions);
            Assert.IsNotNull(ResponseMessage.Answers);
            Assert.IsInstanceOfType(ResponseMessage.Answers.First().Record, typeof(DNS.Records.A));
        }
    }
}
