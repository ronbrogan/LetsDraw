using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenTK;

namespace Foundation.Serialization
{
    public class LetsDrawContractResolver : DefaultContractResolver
    {
        public new static readonly LetsDrawContractResolver Instance = new LetsDrawContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            if(objectType == typeof(Vector2))
            {
                contract.Converter = new Vector2Converter();
            }
            else if (objectType == typeof(Vector3))
            {
                contract.Converter = new Vector3Converter();
            }
            else if (objectType == typeof(Matrix3))
            {
                contract.Converter = new Matrix3Converter();
            }
            else if (objectType == typeof(Matrix4))
            {
                contract.Converter = new Matrix4Converter();
            }
            else if (objectType == typeof(Quaternion))
            {
                contract.Converter = new QuaternionConverter();
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
