namespace Transcript.Service.Processors
{
    internal interface ITranscriptProcessor
    {
        /// <summary>
        /// Process all the mp3 files placed in the provided path
        /// </summary>
        /// <param name="path">path to process</param>
        /// <returns>True if doesn't technical error</returns>
        Task<bool> ProcessVoiceFiles(string path);
    }
}
