namespace Transcript.Service.Services
{
    using Transcript.Service.Interfaces;
    internal class InvoxService : IInvoxService
    {
        private readonly IConfigurationHelper configurationHelper;
        
        private int SuccessRequests;
        private int ReadFileCount;
        private Dictionary<int, string> MockFiles = new Dictionary<int, string>();

        public InvoxService(IConfigurationHelper configHelper)
        {
            configurationHelper = configHelper;
            InitializeMockFiles();
        }

        public Task<byte[]> TranscriptVoiceFile(string voiceFile)
        {
            if (this.ShouldThrowError())
            {
                this.SuccessRequests = 0;
                throw new Exception("Exception due to 5% of failure rate has been reached");
            }

            var result = GetTranscriptContent();
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
        private byte[] GetTranscriptContent()
        {
            var path = this.GetNextFileName();
            try
            {
                Console.WriteLine($"*** Reading file {path} content....");
                return File.ReadAllBytes(path);
            }
            catch
            {
                Console.WriteLine($"Error when reading file content of {path}");
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
                string[] files = Directory.GetFiles(".\\MockData", "*.txt");
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
