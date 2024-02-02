namespace Transcript.Service.Services
{
    using Transcript.Service.Interfaces;
    public class InvoxService : IInvoxService
    {
        private readonly IFileHelper FileHelper;
        
        private int ReadFileCount;
        private Dictionary<int, string> MockFiles = new Dictionary<int, string>();
        public int SuccessRequests;

        public InvoxService(IFileHelper fileHelper)
        {
            FileHelper = fileHelper;
            InitializeMockFiles();
        }

        public virtual Task<byte[]> TranscriptVoiceFile(string voiceFile)
        {
            if (this.ShouldThrowError())
            {
                this.SuccessRequests = 0;
                throw new Exception("Exception due to 5% of failure rate has been reached");
            }

            var result = GetTranscriptContent(voiceFile);
            this.SuccessRequests++;
            return Task.FromResult(result);
        }

        /// <summary>
        /// Error must be thrown for each 5% percentage requests
        /// </summary>
        /// <returns></returns>
        private bool ShouldThrowError()
        {
            return (SuccessRequests * 0.05) >= 1;
        }

        /// <summary>
        /// Retrieve mocked file content
        /// </summary>
        /// <returns>File content</returns>
        private byte[] GetTranscriptContent(string voiceFile)
        {
            var fileName = this.GetNextFileName();
            try
            {
                Console.WriteLine($"*** Reading file {fileName} content....");
                return this.FileHelper.GetFileContent(fileName);
            }
            catch
            {
                Console.WriteLine($"Error when reading file content of {fileName}");
                throw;
            }
        }

        /// <summary>
        /// Iitialize the files we are going to retrieve as transcript content.
        /// </summary>
        private void InitializeMockFiles()
        {
            try
            {
                string[] files = this.FileHelper.GetFilesAtDirectory(".\\MockData", "*.txt");
                for (int i = 0; i < files.Length; i++)
                {
                    MockFiles.Add(i, files[i]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error trying to read transcript files. Error details: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Returns the next mock file to read the content
        /// </summary>
        /// <returns>file name</returns>
        private string GetNextFileName()
        {
            int fileNumber = (ReadFileCount % MockFiles.Count) + 1;
            ReadFileCount++;
            return $".\\MockData\\User_{fileNumber}.txt";
        }
    }
}
