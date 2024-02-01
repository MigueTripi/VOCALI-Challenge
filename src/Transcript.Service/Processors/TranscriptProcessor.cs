
namespace Transcript.Service.Processors
{
    using Transcript.Service.Interfaces;

    internal class TranscriptProcessor : ITranscriptProcessor
    {
        private readonly IConfigurationHelper configurationHelper;
        private readonly IInvoxService InvoxService;
        private int TranscriptRetries => configurationHelper.GetConfigValueWithDefault("TranscriptRetries", 3);

        /// <summary>
        /// Constructor for TranscriptProcessor class
        /// </summary>
        /// <param name="configHelper">To manage app settings</param>
        /// <param name="invoxService">Service to ask the transcriptions</param>
        public TranscriptProcessor(IConfigurationHelper configHelper, IInvoxService invoxService) 
        {
            configurationHelper = configHelper;
            InvoxService = invoxService;
        }

        /// <inheritdoc />
        public async Task<bool> ProcessVoiceFiles(string path)
        {

            string[] filesToProcess = this.GetFilesToProcess(path, "*.mp3");
            var batchSize = configurationHelper.GetConfigValueWithDefault("BatchSize", 3);

            await this.InternalProcessVoiceFilesAsync(filesToProcess, batchSize);

            return true;
        }

        /// <summary>
        /// This routine manage all the logic for sequence requests we can send at the same time to InvoxService. 
        /// </summary>
        /// <param name="filesToProcess"> all the file names we have to transcript</param>
        /// <param name="batchSize">How many request at the same time we can process</param>
        /// <returns></returns>
        private async Task InternalProcessVoiceFilesAsync(string[] filesToProcess, int batchSize)
        {
            var semaphore = new SemaphoreSlim(batchSize);

            var tasks = new List<Task>();

            foreach (var fileToProcess in filesToProcess)
            {
                // Wait for a slot to become available in the semaphore
                await semaphore.WaitAsync();

                // Start the task for the current endpoint
                var task = ProcessAudioFile(fileToProcess, 1)
                    .ContinueWith(_ =>
                    {
                        // Release the semaphore slot when the task is complete
                        semaphore.Release();
                    });

                tasks.Add(task);

                // If we have reached the batch size, wait for any of the tasks to complete
                if (tasks.Count == batchSize)
                {
                    var completedTask = await Task.WhenAny(tasks);
                    tasks.Remove(completedTask);
                }
            }

            // Wait for any remaining tasks to complete
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// This routine asks transcription files to InvoxService and store the file content. Meantime manage retries logic when something is wrong
        /// </summary>
        /// <param name="fileName">file to transcript</param>
        /// <param name="tryNumber">current retry number</param>
        /// <returns></returns>
        private async Task ProcessAudioFile(string fileName, int tryNumber)
        {
            var transcriptRetries = configurationHelper.GetConfigValueWithDefault("TranscriptRetries", 3);
            byte[] fileContent = [];
            try
            {
                fileContent = await this.InvoxService.TranscriptVoiceFile(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR when processing file {fileName}. Error detail: {e.Message}");
                if (tryNumber < this.TranscriptRetries)
                {
                    Console.WriteLine($"Retry file {fileName}");
                    await ProcessAudioFile(fileName, ++tryNumber);
                }
            }

            var newFileName = fileName.Replace("mp3", "txt");
            await File.WriteAllBytesAsync(newFileName, fileContent);
        }
        
        /// <summary>
        /// Retrieve the files we need to process
        /// </summary>
        /// <param name="path"> Directory where to find the files</param>
        /// <param name="searchCriteria">filter the files in the directory (mp3 in our case)</param>
        /// <returns>file names to process</returns>
        private string[] GetFilesToProcess(string path, string searchCriteria)
        {
            try
            {
                string[] files = Directory.GetFiles(path, searchCriteria);
                Console.WriteLine($"There are {files.Length} files with this search criteria {searchCriteria} at path {path}.");
                return files;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error trying to read fields ot path {path}. SearchCriteria: {searchCriteria}. Error details: {e.Message}");
                throw;
            }
        }
    }
}
