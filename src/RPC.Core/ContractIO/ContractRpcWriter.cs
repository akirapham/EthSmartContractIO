﻿using RPC.Core.Gas;
using RPC.Core.Models;
using RPC.Core.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace RPC.Core.ContractIO;

public class ContractRpcWriter : IContractIO
{
    private readonly RpcRequest request;
    private readonly IServiceProvider serviceProvider;
    private IGasPricer GasPricer =>
        serviceProvider.GetRequiredService<IGasPricer>();
    private ITransactionSigner TransactionSigner =>
        serviceProvider.GetRequiredService<ITransactionSigner>();
    private ITransactionSender TransactionSender =>
        serviceProvider.GetRequiredService<ITransactionSender>();

    public ContractRpcWriter(RpcRequest request, IServiceProvider? serviceProvider = null) 
    {
        this.serviceProvider = new ServiceManager(request, serviceProvider);
        this.request = request;
    }

    public virtual string RunContractAction() =>
        TransactionSender.SendTransaction(
            TransactionSigner.SignTransaction(new ReadyTransaction(request, GasPricer)));
}
