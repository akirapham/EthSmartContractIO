﻿using RPC.Core.Gas;
using Nethereum.Web3;
using RPC.Core.Models;
using RPC.Core.Transaction;
using Nethereum.Web3.Accounts;
using Nethereum.JsonRpc.Client;

namespace RPC.Core.ContractIO;

public class ContractRpcWriter : IRpcAction<TransactionInput>
{
    private readonly GasPricer gasPricer;
    private readonly GasEstimator gasEstimator;
    private readonly TransactionSigner transactionSigner;
    private readonly TransactionSender transactionSender;

    public ContractRpcWriter(IWeb3 web3)
    {
        gasPricer = new(web3);
        gasEstimator = new(web3);
        transactionSigner = new(web3);
        transactionSender = new(web3);
    }

    public string ExecuteAction(TransactionInput input) =>
        WriteToNetwork(input);

    public string WriteToNetwork(TransactionInput transactionInput)
    {
        var transaction = gasEstimator.EstimateGas(transactionInput);
        transaction.GasPrice = gasPricer.GetCurrentWeiGasPrice();

        var signedTransaction = transactionSigner.SignTransaction(transaction);
        return transactionSender.SendTransaction(signedTransaction);
    }

    public static IWeb3 CreateWeb3(string rpcConnection, Account account)
    {
        var client = new RpcClient(new Uri(rpcConnection));
        return new Web3(account, client);
    }
}