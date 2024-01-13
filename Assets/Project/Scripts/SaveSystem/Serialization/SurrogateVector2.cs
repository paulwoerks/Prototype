using System.Runtime.Serialization;
using UnityEngine;

namespace PocketHeroes.Serialization
{
    public class SurrogateVector2 : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 data = (Vector2)obj;
            info.AddValue("x", data.x);
            info.AddValue("y", data.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 data = (Vector2)obj;
            data.x = (float)info.GetValue("x", typeof(float));
            data.y = (float)info.GetValue("y", typeof(float));
            return data;
        }
    }
}
