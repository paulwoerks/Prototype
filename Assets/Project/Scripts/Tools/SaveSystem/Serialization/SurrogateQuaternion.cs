using System.Runtime.Serialization;
using UnityEngine;

namespace PocketHeroes.Serialization
{
    public class SurrogateQuaternion : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Quaternion data = (Quaternion)obj;
            info.AddValue("x", data.x);
            info.AddValue("y", data.y);
            info.AddValue("z", data.z);
            info.AddValue("w", data.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion data = (Quaternion)obj;
            data.x = (float)info.GetValue("x", typeof(float));
            data.y = (float)info.GetValue("y", typeof(float));
            data.z = (float)info.GetValue("z", typeof(float));
            data.w = (float)info.GetValue("w", typeof(float));
            return data;
        }
    }
}
