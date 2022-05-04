using System.Text.RegularExpressions;
using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Application.Models;
using Banking.Account.Query.Domain;
using Banking.Cqrs.Core.Events;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Banking.Account.Query.Infrastructure.Consumers
{
    public class BankAccountConsumerService : IHostedService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        public  KafkaSettings _KafkaSettings { get; set; }

        public BankAccountConsumerService(IServiceScopeFactory factory)
        {
            _bankAccountRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();
            _KafkaSettings = (factory.CreateScope().ServiceProvider.GetRequiredService<IOptions<KafkaSettings>>()).Value;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _KafkaSettings.GroupId,
                BootstrapServers = $"{_KafkaSettings.Hostname}:{_KafkaSettings.Port}",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            try
            {
                using (var consumerBuildlder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    var bankTopics = new string[]
                    {
                        typeof(AccountOpenedEvent).Name,
                        typeof(AccountClosedEvent).Name,
                        typeof(FundsDepositedEvent).Name,
                        typeof(FundsWithdrawnEvent).Name,
                    };
                    consumerBuildlder.Subscribe(bankTopics);
                    var cancelToken = new CancellationTokenSource();
                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuildlder.Consume(cancellationToken);
                            if(consumer.Topic == typeof(AccountOpenedEvent).Name)
                            {
                                var accountOpenedEvent = JsonConvert.DeserializeObject<AccountOpenedEvent>(consumer.Message.Value);
                                var bankAccount = new BankAccount
                                {
                                    Identifier = accountOpenedEvent!.Id,
                                    AccountHolder = accountOpenedEvent!.AccountHolder,
                                    AccountType = accountOpenedEvent!.AccountType,
                                    Balance = accountOpenedEvent.OpenigBalance,
                                    CreationDate =  accountOpenedEvent.CreatedDate
                                };
                                _bankAccountRepository.AddAsync(bankAccount).Wait();
                            }
                            if (consumer.Topic == typeof(AccountClosedEvent).Name)
                            {
                                var accountClosedEvent = JsonConvert.DeserializeObject<AccountClosedEvent>(consumer.Message.Value);
                                _bankAccountRepository.DeleteByIdentifier(accountClosedEvent.Id).Wait();
                            }
                            if (consumer.Topic == typeof(FundsDepositedEvent).Name)
                            {
                                var accountDepositedEvent = JsonConvert.DeserializeObject<FundsDepositedEvent>(consumer.Message.Value);
                                var bankAccount = new BankAccount
                                {
                                    Identifier = accountDepositedEvent!.Id,
                                    Balance = accountDepositedEvent.Amount
                                };
                                _bankAccountRepository.DepositBankAccountByIdentifier(bankAccount).Wait();
                            }
                            if (consumer.Topic == typeof(FundsWithdrawnEvent).Name)
                            {
                                var accountWithdrawEvent = JsonConvert.DeserializeObject<FundsWithdrawnEvent>(consumer.Message.Value);
                                var bankAccount = new BankAccount
                                {
                                    Identifier = accountWithdrawEvent!.Id,
                                    Balance = accountWithdrawEvent.Amount
                                };
                                _bankAccountRepository.WithdrawnBankAccountByIdentifier(bankAccount).Wait();
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuildlder.Close();
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
