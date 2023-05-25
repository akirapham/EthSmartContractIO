﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RPC.Core.Models;

public class RpcRequest
{
    public RpcRequest(string to, string data)
    {
        JsonRpc = "2.0";
        Method = "eth_call";
        Params = new JArray()
        {
            new JObject()
            {
                { "to", to },
                { "data", data }
            },
            "latest"
        };
        Id = 0;
    }

    [JsonProperty("jsonrpc")]
    public string JsonRpc { get; set; }

    [JsonProperty("method")]
    public string Method { get; set; }

    [JsonProperty("params")]
    public JArray Params { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }
}