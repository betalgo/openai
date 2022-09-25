namespace OpenAI.GPT3.ObjectModels.SharedModels
{
    public interface IOpenAiModels
    {
        public interface IId
        {
            string Id { get; set; }
        }

        public interface IModel
        {
            string Model { get; set; }
        }

        public interface ILogProbs
        {
            int? LogProbs { get; set; }
        }

        public interface ITemperature
        {
            float? Temperature { get; set; }
        }

        public interface ICreatedAt
        {
            public int CreatedAt { get; set; }
        }
    }
}