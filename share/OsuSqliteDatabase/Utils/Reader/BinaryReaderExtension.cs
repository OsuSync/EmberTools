using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OsuSqliteDatabase.Utils.Reader
{
    public static class BinaryReaderExtension
    {
        public static string ReadOsuString(this BinaryReader reader)
        {
            if (0 == reader.ReadByte()) return null;
            return reader.ReadString();
        }
        public static DateTime ReadDateTime(this BinaryReader reader)
        {
            return new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
        }
        public static byte[] ReadByteArray(this BinaryReader reader)
        {
            int len = reader.ReadInt32();
            if (len > 0) return reader.ReadBytes(len);
            if (len < 0) return null;
            return new byte[] { };
        }

        public static char[] ReadCharArray(this BinaryReader reader)
        {
            int len = reader.ReadInt32();
            if (len > 0) return reader.ReadChars(len);
            if (len < 0) return null;
            return new char[] { };
        }
        public static Dictionary<T, U> ReadDictionary<T, U>(this BinaryReader reader)
        {
            int count = reader.ReadInt32();
            if (count < 0) return null;
            Dictionary<T, U> d = new Dictionary<T, U>();
            for (int i = 0; i < count; i++) d[(T)reader.ReadObject()] = (U)reader.ReadObject();
            return d;
        }

        private enum ObjType : byte
        {
            Null,
            Bool,
            Byte,
            UShort,
            UInt,
            ULong,
            SByte,
            Short,
            Int,
            Long,
            Char,
            String,
            Float,
            Double,
            Decimal,
            DateTime,
            ByteArray,
            CharArray,
        }
        public static object ReadObject(this BinaryReader reader)
        {
            return (ObjType)reader.ReadByte() switch
            {
                ObjType.Bool => reader.ReadBoolean(),
                ObjType.Byte => reader.ReadByte(),
                ObjType.UShort => reader.ReadUInt16(),
                ObjType.UInt => reader.ReadUInt32(),
                ObjType.ULong => reader.ReadUInt64(),
                ObjType.SByte => reader.ReadSByte(),
                ObjType.Short => reader.ReadInt16(),
                ObjType.Int => reader.ReadInt32(),
                ObjType.Long => reader.ReadInt64(),
                ObjType.Char => reader.ReadChar(),
                ObjType.String => reader.ReadString(),
                ObjType.Float => reader.ReadSingle(),
                ObjType.Double => reader.ReadDouble(),
                ObjType.Decimal => reader.ReadDecimal(),
                ObjType.DateTime => reader.ReadDateTime(),
                ObjType.ByteArray => reader.ReadByteArray(),
                ObjType.CharArray => reader.ReadCharArray(),
                _ => null,
            };
        }
    }
}
