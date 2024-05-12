# power-gateway

## Docker Debug

``` bash
docker run --rm -it -v ${PWD}:/app -p 6000:6000 mcr.microsoft.com/dotnet/sdk:8.0 bash

cd /app/src/WebApi
dotnet run --urls=http://*:6000
```

## Test Curl

``` bash
curl -X 'POST' \
  'http://localhost:5251/api/Logs/apisix' \
  -H 'accept: */*' \
  --data-raw '
[
    {
        "response": {
            "status": 200,
            "headers": {
                "server": "APISIX/3.6.0",
                "content-type": "application/json; charset=utf-8",
                "connection": "close",
                "date": "Sun, 12 May 2024 15:45:08 GMT"
            },
            "size": 107,
            "body": "{\"message\":\"Pong\"}"
        },
        "request": {
            "querystring": {},
            "headers": {
                "sec-fetch-site": "same-origin",
                "accept-encoding": "gzip, deflate, br, zstd",
                "sec-fetch-mode": "cors",
                "host": "www.example.com",
                "referer": "http://www.example.com/swagger/index.html",
                "sec-ch-ua": "\"Google Chrome\";v=\"123\", \"Not:A-Brand\";v=\"8\", \"Chromium\";v=\"123\"",
                "accept-language": "en,zh-CN;q=0.9,zh;q=0.8,en-GB;q=0.7,zh-TW;q=0.6",
                "sec-ch-ua-mobile": "?0",
                "sec-fetch-dest": "empty",
                "accept": "*/*",
                "sec-ch-ua-platform": "\"Windows\"",
                "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36"
            },
            "url": "http://www.example.com:9443/api/Ping",
            "uri": "/api/Ping",
            "size": 23,
            "method": "GET"
        },
        "apisix_latency": 0.99985504150391,
        "client_ip": "127.0.0.1",
        "service_id": "",
        "server": {
            "version": "3.6.0",
            "hostname": "a6ddb7cd6feb"
        },
        "upstream_latency": 1,
        "latency": 1.9998550415039,
        "start_time": 1715528709002,
        "route_id": "1",
        "upstream": "172.16.0.9:5000"
    }
]
'
```
