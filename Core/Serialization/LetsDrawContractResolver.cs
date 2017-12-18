using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Core.Serialization
{
    public class LetsDrawContractResolver : DefaultContractResolver
    {
        private readonly Stream binaryContents;

        private Dictionary<Type, Func<JsonConverter>> ConverterLookup = new Dictionary<Type, Func<JsonConverter>>()
        {
            { typeof(Vector2),           () => new Vector2Converter() },
            { typeof(Vector3),           () => new Vector3Converter() },
            { typeof(Matrix4x4),         () => new Matrix4Converter() },
            { typeof(Quaternion),        () => new QuaternionConverter() },
        };

        public LetsDrawContractResolver(Stream binaryContents)
        {
            this.binaryContents = binaryContents;

            ConverterLookup.Add(typeof(Stream), () => new StreamConverter(this.binaryContents));
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);


            if (ConverterLookup.ContainsKey(objectType))
            {
                contract.Converter = ConverterLookup[objectType]();
            }
            else
            {
                var inheritedConverter = ConverterLookup.Keys.FirstOrDefault(t => t.IsAssignableFrom(objectType));

                if (inheritedConverter != null)
                    contract.Converter = ConverterLookup[inheritedConverter]();
            }

            return contract;
        }

        //protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        //{
            //IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            //return props.Where(p => p.Writable).ToList();
        //}
    }
}
