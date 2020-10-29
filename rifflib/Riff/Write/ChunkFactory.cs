using Riff.Write.Chunk;

namespace Riff.Write
{
    /// <summary>
    /// A chunk factory
    /// </summary>
    public interface IChunkFactory
    {
        /// <summary>
        /// Create the appropriate chunk instance based on the identifier
        /// </summary>
        /// <param name="identifier">4 character chunk identifier</param>
        ChunkBase Create(string identifier);
    }
}