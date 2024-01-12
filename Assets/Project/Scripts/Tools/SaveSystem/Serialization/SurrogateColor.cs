using System.Runtime.Serialization;
using UnityEngine;

namespace PocketHeroes.Serialization
{
    public class SurrogateColor : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color data = (Color)obj;
            info.AddValue("r", data.r);
            info.AddValue("g", data.g);
            info.AddValue("b", data.b);
            info.AddValue("a", data.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color data = (Color)obj;
            data.r = (float)info.GetValue("r", typeof(float));
            data.g = (float)info.GetValue("g", typeof(float));
            data.b = (float)info.GetValue("b", typeof(float));
            data.a = (float)info.GetValue("a", typeof(float));
            return data;
        }
    }
}
