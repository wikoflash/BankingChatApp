using BankingChatApp.Core.Entities;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingChatApp.Application.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly string _topic;
        private readonly ChatService _chatService;

        public KafkaConsumerService(IConfiguration configuration, ChatService chatService)
        {
            var config = new ConsumerConfig
            {
                GroupId = configuration["Kafka:GroupId"],
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(config).Build();
            _topic = configuration["Kafka:Topic"];
            _chatService = chatService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = JsonConvert.DeserializeObject<ChatMessage>(consumeResult.Message.Value);

                // Save the message via ChatService
                await _chatService.AddMessageAsync(message);
            }
        }
    }
}
