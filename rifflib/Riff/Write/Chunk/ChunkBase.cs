using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Riff.Write.Chunk
{
    /// <summary>
    /// Base class for all chunks.
    /// </summary>
    /// <remarks>
    /// Chunks can contain other chunks, in a tree structure.
    /// </remarks>
    public abstract class ChunkBase : IList<ChunkBase>
    {
        /// <summary>
        /// The 4 character chunk identifier. E.g. LIST
        /// </summary>
        /// <value></value>
        public String Identifier { get; set; }

        /// <summary>
        /// Size of the chunk data
        /// </summary>
        /// <value></value>
        public abstract int Size { get; }

        public virtual void Write(BinaryWriter writer)
        {
            writer.WriteFixedString(Identifier, RiffUtils.IdentifierSize);
            writer.Write(Size);
        }
        
        #region IList<>

        public virtual ChunkBase this[int index] 
        { 
            get { throw new NotImplementedException(); } 
            set { throw new NotImplementedException(); } 
        }

        public virtual int IndexOf(ChunkBase item) { throw new NotImplementedException(); } 
        public virtual void Insert(int index, ChunkBase item) { throw new NotImplementedException(); } 
        public virtual void RemoveAt(int index) { throw new NotImplementedException(); } 
        
        #endregion

        #region ICollection<>
        public virtual int Count { get { return 0; } }
        public virtual bool IsReadOnly { get { return true; } }

        public virtual void Add(ChunkBase item)  { throw new NotImplementedException(); } 
        public virtual void Clear()  { throw new NotImplementedException(); } 
        public virtual bool Contains(ChunkBase item) { return false; }
        public virtual void CopyTo(ChunkBase[] array, int arrayIndex)  { throw new NotImplementedException(); } 
        public virtual bool Remove(ChunkBase item)  { throw new NotImplementedException(); } 

        #endregion

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