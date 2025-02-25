using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    //.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
    .LoadFromMemory(new List<RouteConfig>
    {
        new RouteConfig
        {
            ClusterId="API1-Cluster",
            Match = new()
            {
                Path= "/api1/{**catch-all}"
            },
            Transforms=new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                     ["RequestHeader"]= "api1-request-header",
                      ["Append"]= "api1 request"
                },
                new()
                {
                     {"ResponseHeader", "api1-response-header" },
                     {"Append", "api1 response" },
                     {"When","Always" }
                }
            }
        },
        new RouteConfig
        {
            ClusterId="API2-Cluster",
            Match = new()
            {
                Path= "/api2/{**catch-all}"
            },
            Transforms=new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                     ["RequestHeader"]= "api2-request-header",
                      ["Append"]= "api2 request"
                },
                new()
                {
                     {"ResponseHeader", "api2-response-header" },
                     {"Append", "api2 response" },
                     {"When","Always" }
                }
            }
        },
        new RouteConfig
        {
             ClusterId="API3-Cluster",
             Match = new()
             {
                Path= "/api3/{**catch-all}"
             },
             Transforms=new List<Dictionary<string, string>>
             {
                new Dictionary<string, string>
                {
                     ["RequestHeader"]= "api3-request-header",
                      ["Append"]= "api3 request"
                },
                new()
                {
                     {"ResponseHeader", "api3-response-header" },
                     {"Append", "api3 response" },
                     {"When","Always" }
                }
             }
        }
    },
    new List<ClusterConfig>
    {
        new ClusterConfig()
        {
            ClusterId="API1-Cluster",
            Destinations= new Dictionary<string, DestinationConfig>
            {
                ["destination1"] = new()
                {
                    Address= "https://localhost:7200"
                }
            }
        },

         new ClusterConfig()
        {
            ClusterId="API2-Cluster",
            Destinations= new Dictionary<string, DestinationConfig>
            {
                ["destination2"] = new()
                {
                    Address= "https://localhost:7169"
                },
            }
        },
          new ClusterConfig()
        {
            ClusterId="API3-Cluster",
            Destinations= new Dictionary<string, DestinationConfig>
            {
               ["destination3"] = new()
                {
                    Address= "https://localhost:7192"
                },
            }
        }




        ,
               
               


    });


var app = builder.Build();

app.MapReverseProxy();//middleware unutma çaðýr

app.Run();
  