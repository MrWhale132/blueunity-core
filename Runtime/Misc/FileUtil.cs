using System;
using System.IO;
using UnityEngine;


namespace Theblueway.Core.Runtime
{
    public class FileUtil
    {
        public static void WriteDataWithHeader(string filePath, byte[] dataBytes, byte[] headerBytes, bool relative = true)
        {
            if (relative)
            {
                var basePath = Application.persistentDataPath;
                filePath = Path.Combine(basePath, filePath);
            }

            // Ensure directory exists
            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new BinaryWriter(stream);

            // Write: [HeaderLength][Header][Data]
            writer.Write(headerBytes.Length);
            writer.Write(headerBytes);
            writer.Write(dataBytes);
        }



        public static byte[] ReadDataFromFile(string filePath, bool relative = true)
        {
            if (relative)
            {
                var basePath = Application.persistentDataPath;
                filePath = Path.Combine(basePath, filePath);
            }

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);

            // Read header length
            int headerLength = reader.ReadInt32();

            if (headerLength <= 0 || headerLength > stream.Length)
            {
                throw new Exception("Invalid header length - corrupted or wrong format");
            }


            // Skip header
            reader.BaseStream.Seek(headerLength, SeekOrigin.Current);

            // Read remaining data
            long remainingLength = reader.BaseStream.Length - reader.BaseStream.Position;
            byte[] dataBytes = reader.ReadBytes((int)remainingLength);

            return dataBytes;
        }




        public static byte[] ReadHeader(string filePath, bool relative = true)
        {
            if (relative)
            {
                var basePath = Application.persistentDataPath;
                filePath = Path.Combine(basePath, filePath);
            }

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);

            int headerLength = reader.ReadInt32();
            byte[] headerBytes = reader.ReadBytes(headerLength);

            return headerBytes;
        }
    }
}
