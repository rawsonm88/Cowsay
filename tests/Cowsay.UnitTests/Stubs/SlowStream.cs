using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cowsay.UnitTests.Stubs
{
    internal class SlowStream : Stream
    {
        private readonly Stream _baseStream;
        private int _threadsReadingCount = 0;

        public SlowStream(Stream baseStream)
        {
            _baseStream = baseStream;
        }

        public override bool CanRead => _baseStream.CanRead;

        public override bool CanSeek => _baseStream.CanSeek;

        public override bool CanWrite => _baseStream.CanWrite;

        public override long Length => _baseStream.Length;

        public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

        public int ThreadsReadingCount => _threadsReadingCount;

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Interlocked.Increment(ref _threadsReadingCount);

            int output;

            try
            {
                Delay().Wait();
                output = _baseStream.Read(buffer, offset, count);
            }
            finally
            {
                Interlocked.Decrement(ref _threadsReadingCount);
            }

            return output;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _baseStream.Write(buffer, offset, count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _threadsReadingCount);

            int output;

            try
            {
                await Delay();
                output = await _baseStream.ReadAsync(buffer, offset, count);
            }
            finally
            {
                Interlocked.Decrement(ref _threadsReadingCount);
            }

            return output;
        }

        private Task Delay()
        {
            return Task.Delay(TimeSpan.FromMilliseconds(500));
        }
    }
}
