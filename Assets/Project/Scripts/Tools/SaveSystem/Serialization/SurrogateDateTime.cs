using System;
using System.Runtime.Serialization;

namespace PocketHeroes.Serialization
{
    public class SurrogateDateTime : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            DateTime data = (DateTime)obj;
            info.AddValue("ticks", data.Ticks);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new DateTime((long)info.GetValue("ticks", typeof(long)));
        }
    }
}
