namespace Transcript.Service.Interfaces
{
    public interface IInvoxService
    {
        /// <summary>
        /// Process provided voice file and return a mocked content
        /// </summary>
        /// <param name="voiceFile">file name to process</param>
        /// <returns>transcript content</returns>
        Task<byte[]> TranscriptVoiceFile(string voiceFile);
    }
}
