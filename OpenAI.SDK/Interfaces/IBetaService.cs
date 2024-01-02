using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Interfaces
{
    public interface IBetaService
    {
        public IAssistantService Assistants { get; }

        public IMessageService Messages { get; }

        public IThreadService Threads { get; }

        public IRunService Runs { get; }
    }
}
