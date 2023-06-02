﻿using Xunit;
using RPC.Core.Models;
using RPC.Core.Tests.Mocks;
using Nethereum.Hex.HexTypes;

namespace RPC.Core.ContractIO.Tests;

public class ContractRpcWriterTests
{
    private Request request;

    public ContractRpcWriterTests()
    {
        request = new Request(
            rpcUrl: "http://localhost:8545/",
            accountId: 0,
            chainId: 1,
            from: "0xA98b8386a806966c959C35c636b929FE7c5dD7dE",
            to: "0xA98b8386a806966c959C35c636b929FE7c5dD7dE",
            value: new HexBigInteger(10000000000000000),
            gasSettings: new GasSettings(30000, 6)
        );
    }

    [Fact]
    internal void RunContractAction_ExpectedTransactionHex()
    {
        var contractRpcWriter = new ContractRpcWriter(request)
        {
            Web3 = MockWeb3.GetMock,
            SecretManager = MockSecretManager.GetMock
        };

        var result = contractRpcWriter.RunContractAction();

        Assert.NotNull(result);
        Assert.Equal("transactionHash", result);
    }

    [Fact]
    internal void CheckGasLimits_LimitsNotExceeded_WithoutThrows()
    {
        var contractRpcWriter = new ContractRpcWriter(request)
        {
            Web3 = MockWeb3.GetMock,
            SecretManager = MockSecretManager.GetMock
        };

        var result = contractRpcWriter.RunContractAction();

        Assert.NotNull(result);
        Assert.Equal("transactionHash", result);
    }

    [Fact]
    internal void CheckGasLimits_GasLimitExceeded_ThrowException()
    {
        request = new Request(
            rpcUrl: "http://localhost:8545/",
            accountId: 0,
            chainId: 1,
            from: "0xA98b8386a806966c959C35c636b929FE7c5dD7dE",
            to: "0xA98b8386a806966c959C35c636b929FE7c5dD7dE",
            value: new HexBigInteger(10000000000000000),
            gasSettings: new GasSettings(20000, 6)
        );

        var contractRpcWriter = new ContractRpcWriter(request)
        {
            Web3 = MockWeb3.GetMock,
            SecretManager = MockSecretManager.GetMock
        };

        Action testCode = () => contractRpcWriter.RunContractAction();

        var exception = Assert.Throws<InvalidOperationException>(testCode);
        Assert.Equal("Gas limit exceeded.", exception.Message);
    }

    [Fact]
    internal void CheckGasLimits_GasPriceExceeded_ThrowException()
    {
        request = new Request(
            rpcUrl: "http://localhost:8545/",
            accountId: 0,
            chainId: 1,
            from: "0xA98b8386a806966c959C35c636b929FE7c5dD7dE",
            to: "0xA98b8386a806966c959C35c636b929FE7c5dD7dE",
            value: new HexBigInteger(10000000000000000),
            gasSettings: new GasSettings(30000, 4)
        );

        var contractRpcWriter = new ContractRpcWriter(request)
        {
            Web3 = MockWeb3.GetMock,
            SecretManager = MockSecretManager.GetMock
        };

        Action testCode = () => contractRpcWriter.RunContractAction();

        var exception = Assert.Throws<InvalidOperationException>(testCode);
        Assert.Equal("Gas price exceeded.", exception.Message);
    }
}
