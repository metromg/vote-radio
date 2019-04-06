using System;
using System.IO;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Domain.MasterData.Objects
{
    public class FileInfo
    {
        private readonly Func<Stream> _dataProvider;

        public FileInfo(string contentType, byte[] data)
        {
            ContentType = contentType;
            ContentLength = data.Length;
            _dataProvider = () => new MemoryStream(data);
        }

        public FileInfo(string contentType, long contentLength, Stream data)
        {
            ContentType = contentType;
            ContentLength = contentLength;
            _dataProvider = () =>
            {
                data.Position = 0;
                return data;
            };
        }

        public FileInfo(Image image, Func<Stream> dataProvider)
        {
            ContentType = image.ContentType;
            ContentLength = image.ContentLength;
            _dataProvider = dataProvider;
        }

        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        public Stream Data()
        {
            return _dataProvider();
        }

        public byte[] ToByteArray()
        {
            byte[] data;
            using (var ms = new MemoryStream())
            using (var ds = Data())
            {
                ds.CopyTo(ms);
                data = ms.ToArray();
            }

            return data;
        }
    }
}
