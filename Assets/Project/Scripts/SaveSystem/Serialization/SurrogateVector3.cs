using System.Runtime.Serialization;
using UnityEngine;

namespace PocketHeroes.Serialization
{
    public class SurrogateVector3 : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 data = (Vector3)obj;
            info.AddValue("x", data.x);
            info.AddValue("y", data.y);
            info.AddValue("z", data.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 data = (Vector3)obj;
            data.x = (float)info.GetValue("x", typeof(float));
            data.y = (float)info.GetValue("y", typeof(float));
            data.z = (float)info.GetValue("z", typeof(float));
            return data;
        }
    }
}
