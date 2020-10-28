using System;
using System.Collections.Generic;
using System.IO;
using Validation;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;

namespace Riff.Chunk
{
    /// <summary>
    /// Base class for all chunks.
    /// </summary>
    /// <remarks>
    /// Chunks can contain other chunks, in a tree structure.
    /// </remarks>
    [JsonObject]
    public abstract class ChunkBase : IEnumerable<ChunkBase>
    {
        /// <summary>
        /// Size of the identifier field in bytes.
        /// </summary>
        public const int IdentifierSize = 4;
        
        /// <summary>
        /// Size of the length field in bytes.
        /// </summary>
        public const int LengthSize = 4;

        /// <summary>
        /// Construct a new chunk
        /// </summary>
        /// <param name="identifier">The 4 character chunk identifier</param>
        protected ChunkBase(string identifier)
        {
            Requires.NotNullOrWhiteSpace(identifier, nameof(identifier));
            Requires.Argument(identifier.Length==4, nameof(identifier), "Invalid identifier: "+ identifier);

            Identifier = identifier;
        }

        /// <summary>
        /// Read this chunk's header fields from the reader
        /// </summary>
        /// <param name="reader">The reader to read from</param>
        /// <param name="chunkFactory">Used by derived classes to create new child chunks</param>
        public virtual void Read(BinaryReader reader, IChunkFactory chunkFactory)
        {
            ChunkOffset = reader.BaseStream.Position-IdentifierSize;
            Size = reader.ReadInt32();
        }

        /// <summary>
        /// Chunk data could be very large, so we lazyily read it.
        /// </summary>
        /// <param name="source">The source stream. MUST BE THE SAME DATA SOURCE (E.G. FILE) AS WAS ORIGINALLY USED WITH Read()/param>
        /// <remarks>
        /// We do not store the BinaryRader used to read the header values: it's disposable, which in turn means
        /// this class and sub-classes would also need to be disposable. That is too heavy a burden on all users
        /// of this library.
        /// </remarks>
        public virtual byte[] ReadData(Stream source)
        {
            source.Seek(ChunkOffset + IdentifierSize + LengthSize, SeekOrigin.Begin);
            using (var reader = new BinaryReader(source, System.Text.Encoding.ASCII, true))
            {
                return reader.ReadBytes(Size);
            }
        }

        /// <summary>
        /// The offset of the start of this chunk from the beginning of the input stream 
        /// </summary>
        public long ChunkOffset { get; private set; }

        /// <summary>
        /// The 4 character chunk identifier. E.g. LIST
        /// </summary>
        /// <value></value>
        public String Identifier { get; set; }

        /// <summary>
        /// Size of the chunk data in bytes.
        /// </summary>
        /// <remarks>
        /// Size gives the size of the valid data in the chunk; it does not include the padding, the size of the identifier, or the size of the size field itself.
        /// </remarks>
        public int Size { get; private set; }

        /// <summary>
        /// Size of any necessary padding that may follow the variable length data field
        /// </summary>
        /// <remarks>
        /// The data is always padded to nearest WORD boundary
        /// </remarks>
        public int Padding
        {
            get
            {
                var wordSize = sizeof(short);
                return ((Size + wordSize - 1) / wordSize * wordSize) - Size;
            }
        }

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        virtual public IEnumerator<ChunkBase> GetEnumerator()
        {
            return Enumerable.Empty<ChunkBase>().GetEnumerator();
        }

        #endregion
    }
}