﻿using Nethereum.Web3;
using EthSmartContractIO.Gas;
using EthSmartContractIO.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace EthSmartContractIO.Builders;

public class ServiceProviderBuilder
{
    private readonly IServiceCollection services;

    public ServiceProviderBuilder()
    {
        services = new ServiceCollection();
    }

    public ServiceProvider Build()
    {
        return services.BuildServiceProvider();
    }

    public ServiceProviderBuilder AddWeb3(IWeb3 web3)
    {
        services.AddSingleton(web3);
        return this;
    }

    public ServiceProviderBuilder AddGasPricer(IGasPricer gasPricer)
    {
        services.AddSingleton(gasPricer);
        return this;
    }

    public ServiceProviderBuilder AddTransactionSigner(ITransactionSigner transactionSigner)
    {
        services.AddSingleton(transactionSigner);
        return this;
    }

    public ServiceProviderBuilder AddTransactionSender(ITransactionSender transactionSender)
    {
        services.AddSingleton(transactionSender);
        return this;
    }
}