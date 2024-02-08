using OpenAI.Builders;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Playground.TestHelpers
{
    internal static class ThreadTestHelper
    {
        public static async Task RunThreadCreateTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Thread create Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Thread Create Test:", ConsoleColor.DarkCyan);
                var threadResult = await sdk.Beta.Threads.ThreadCreate();

                if (threadResult.Successful)
                {
                    ConsoleExtensions.WriteLine(threadResult.ToJson());
                }
                else
                {
                    if (threadResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                    ConsoleExtensions.WriteLine($"{threadResult.Error.Code}: {threadResult.Error.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task RunThreadRetrieveTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Thread retrieve Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Thread Retrieve Test:", ConsoleColor.DarkCyan);

                var threadId = "thread_eG76zeIGn8XoMN8yYOR1VxfG";
                var threadResult = await sdk.Beta.Threads.ThreadRetrieve(threadId);

                if (threadResult.Successful)
                {
                    ConsoleExtensions.WriteLine(threadResult.ToJson());
                }
                else
                {
                    if (threadResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                    ConsoleExtensions.WriteLine($"{threadResult.Error.Code}: {threadResult.Error.Message}");
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
