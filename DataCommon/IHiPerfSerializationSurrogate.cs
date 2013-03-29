using System;
using System.IO;
namespace DataCommon
{
    public interface IHiPerfSerializationSurrogate
    {
        void Serialize(BinaryWriter writer, object graph);
        object DeSerialize(BinaryReader reader);
    }
}
