using System;

namespace WSPack.VisualStudio.Shared
{
  /// <summary>
  /// Wrapper para implementação de IStrem
  /// </summary>
  public class StreamWrapper : System.IO.Stream
  {
    readonly Microsoft.VisualStudio.OLE.Interop.IStream _iStream;

    /// <summary>
    /// Cria uma instância da classe <see cref="StreamWrapper"/>
    /// </summary>
    /// <param name="stream">The stream.</param>
    public StreamWrapper(Microsoft.VisualStudio.OLE.Interop.IStream stream)
    {
      _iStream = stream;
    }

    /// <summary>
    /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
    /// </summary>
    public override bool CanRead
    {
      get { return _iStream != null; }
    }

    /// <summary>
    /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
    /// </summary>
    public override bool CanSeek
    {
      get { return true; }
    }

    /// <summary>
    /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
    /// </summary>
    public override bool CanWrite
    {
      get { return true; }
    }

    /// <summary>
    /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
    /// </summary>
    public override void Flush()
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      _iStream.Commit(0);
    }

    /// <summary>
    /// When overridden in a derived class, gets the length in bytes of the stream.
    /// </summary>
    public override long Length
    {
      get
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        Microsoft.VisualStudio.OLE.Interop.STATSTG[] stat = new Microsoft.VisualStudio.OLE.Interop.STATSTG[1];
        _iStream.Stat(stat, (uint)Microsoft.VisualStudio.OLE.Interop.STATFLAG.STATFLAG_DEFAULT);

        return (long)stat[0].cbSize.QuadPart;
      }
    }

    /// <summary>
    /// When overridden in a derived class, gets or sets the position within the current stream.
    /// </summary>
    public override long Position
    {
      get
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        return Seek(0, System.IO.SeekOrigin.Current);
      }

      set
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        Seek(value, System.IO.SeekOrigin.Begin);
      }
    }

    /// <summary>
    /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
    /// </summary>
    /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
    /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
    /// <returns>
    /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Buffer cannot be null.</exception>
    public override int Read(byte[] buffer, int offset, int count)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (buffer == null)
        throw new ArgumentNullException("Buffer cannot be null.");

      byte[] b = buffer;

      if (offset != 0)
      {
        b = new byte[buffer.Length - offset];
        buffer.CopyTo(b, 0);
      }

      _iStream.Read(b, (uint)count, out uint byteCounter);

      if (offset != 0)
      {
        b.CopyTo(buffer, offset);
      }

      return (int)byteCounter;
    }

    /// <summary>
    /// When overridden in a derived class, sets the position within the current stream.
    /// </summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
    /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
    /// <returns>
    /// The new position within the current stream.
    /// </returns>
    public override long Seek(long offset, System.IO.SeekOrigin origin)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      Microsoft.VisualStudio.OLE.Interop.LARGE_INTEGER l = new Microsoft.VisualStudio.OLE.Interop.LARGE_INTEGER();
      Microsoft.VisualStudio.OLE.Interop.ULARGE_INTEGER[] ul = new Microsoft.VisualStudio.OLE.Interop.ULARGE_INTEGER[1] { new Microsoft.VisualStudio.OLE.Interop.ULARGE_INTEGER() };
      l.QuadPart = offset;
      _iStream.Seek(l, (uint)origin, ul);
      return (long)ul[0].QuadPart;
    }

    /// <summary>
    /// When overridden in a derived class, sets the length of the current stream.
    /// </summary>
    /// <param name="value">The desired length of the current stream in bytes.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    public override void SetLength(long value)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (!CanWrite)
        throw new InvalidOperationException();

      Microsoft.VisualStudio.OLE.Interop.ULARGE_INTEGER ul = new Microsoft.VisualStudio.OLE.Interop.ULARGE_INTEGER
      {
        QuadPart = (ulong)value
      };
      _iStream.SetSize(ul);
    }

    /// <summary>
    /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The number of bytes to be written to the current stream.</param>
    /// <exception cref="System.ArgumentNullException">Buffer cannot be null.</exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="System.IO.IOException">Failed to write the total number of bytes to IStream!</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (buffer == null)
        throw new ArgumentNullException("Buffer cannot be null.");
      else if (!CanWrite)
        throw new InvalidOperationException();

      if (count > 0)
      {

        byte[] b = buffer;

        if (offset != 0)
        {
          b = new byte[buffer.Length - offset];
          buffer.CopyTo(b, 0);
        }

        _iStream.Write(b, (uint)count, out uint byteCounter);
        if (byteCounter != count)
          throw new System.IO.IOException("Failed to write the total number of bytes to IStream!");

        if (offset != 0)
        {
          b.CopyTo(buffer, offset);
        }
      }
    }

  }
}