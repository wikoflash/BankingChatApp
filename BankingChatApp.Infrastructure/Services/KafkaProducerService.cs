using BankingChatApp.Core.Entities;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BankingChatApp.Application.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig { BootstrapServers = configuration["Kafka:BootstrapServers"] };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = configuration["Kafka:Topic"];
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = messageJson });
        }
    }
}
