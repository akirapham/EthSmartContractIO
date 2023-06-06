﻿using RPC.Core.Gas;
using Nethereum.Web3;
using RPC.Core.Models;
using RPC.Core.Utility;
using RPC.Core.Providers;
using RPC.Core.Transaction;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Hex.HexTypes;

namespace RPC.Core.ContractIO;

public class ContractRpcWriter : IContractIO
{
    private readonly RpcRequest request;
    private string? accountAddress;

    public IWeb3? Web3 { get; set; }

    public ContractRpcWriter(RpcRequest request)
    {
        this.request = request;
    }

    public virtual string RunContractAction()
    {
        Web3 ??= InitializeWeb3();

        var transaction = new GasEstimator(Web3).EstimateGas(CreateActionInput());
        transaction.GasPrice = new GasPricer(Web3).GetCurrentWeiGasPrice();

        new GasLimitChecker(transaction, request.WriteRequest!.GasSettings).CheckAndThrow();

        var signedTransaction = new TransactionSigner(Web3).SignTransaction(transaction);
        return new TransactionSender(Web3).SendTransaction(signedTransaction);
    }

    public IWeb3 InitializeWeb3()
    {
        var accountProvider = new AccountProvider(
            mnemonicProvider: request.WriteRequest!.MnemonicProvider,
            accountId: request.WriteRequest!.AccountId,
            chainId: request.WriteRequest!.ChainId
        );
        accountAddress = accountProvider.AccountAddress;
        return Web3Base.CreateWeb3(request.RpcUrl, accountProvider.Account);
    }

    private TransactionInput CreateActionInput() =>
        new(request.Data, request.To, request.WriteRequest!.Value)
        {
            ChainId = new HexBigInteger(request.WriteRequest!.ChainId),
            From = accountAddress
        };
}
