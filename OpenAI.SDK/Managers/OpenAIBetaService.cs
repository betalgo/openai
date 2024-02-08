using OpenAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Managers
{
    /// <summary>
    /// 提供beta版本的功能
    /// </summary>
    public partial class OpenAIService : IBetaService
    {
        public IAssistantService Assistants => this;

        public IMessageService Messages => this;

        public IThreadService Threads => this;

        public IRunService Runs => this;
    }
}
