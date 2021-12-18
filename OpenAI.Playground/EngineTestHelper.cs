using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models;
using OpenAI.SDK.Models.RequestModels;

namespace OpenAI.Playground
{
    internal static class EngineTestHelper
    {
        public static async Task UploadSampleFileAndGetSearchResponse(IOpenAISdk sdk)
        {
            try
            {
                var engineList = await sdk.Engine.ListEngines();
                Console.WriteLine("Engine List:");
                if (engineList == null)
                {
                    throw new NullReferenceException(nameof(engineList));
                }
                Console.WriteLine(string.Join(",", engineList.Engines.Select(r => r.Id)));

                foreach (var engineItem in engineList.Engines)
                {
                    Console.WriteLine($"Retrieve Engine:{engineItem.Id}");
                    var engine = await sdk.Engine.RetrieveEngine(engineItem.Id);
                    Console.WriteLine(engine.Successful
                        ? $"Retrieved Engine:{engine.Id}"
                        : $"Couldn't Retrieve Engine:{engineItem.Id}");
                    }

                }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}