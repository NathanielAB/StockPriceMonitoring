# Stock Price Monitoring and Alerts

## Overview

This project is a backend service built with .NET 8 that simulates or fetches live stock prices, allows users to register threshold alerts, and notifies them in real-time via Server-Sent Events (SSE) when the alert conditions are met.

## Endpoints

### Create an Alert

```
POST /alerts

Headers:
x-user-id: "e5669ce9-be81-4ed7-99f7-741c57aab250"

Payload:
{
  "type": "above",
  "stockSymbol": "AAPL",
  "thresholdPrice": 200.0,
}
```

### Get User Alerts
```
GET /alerts

Headers:
x-user-id: "e5669ce9-be81-4ed7-99f7-741c57aab250"
```

### Delete User Alerts
```
DELETE /alerts

Headers:
x-user-id: "e5669ce9-be81-4ed7-99f7-741c57aab250"

Payload:
{
  "alertId ": "86c0535c-a2e5-4232-beda-36a9810955c0"
}
```

### Get Notifications (Server-Sent Events)
Subscribe to real time events from the server to get notifications for a particular user
```
GET /notifications

Headers:
x-user-id: "e5669ce9-be81-4ed7-99f7-741c57aab250"
```

### Reset Alerts
Resets the `Triggered` boolean value of all the alerts to make testing easier when testing the notifications
```
POST /alerts/reset
```

## Caching
The stock prices are polled every 10 seconds however there is a cache configured to be 20 seconds using `IMemoryCache`

## Frontend and Testing
To test the notifications one can use the `frontend.html` file. It is recommended to reset the alerts from `/alerts/reset` since the application only triggers an alert once. The reset endpoint resets every alert to be triggerable again

By default there are 2 alerts created that one can use or else one can create new alerts
```json
[
  {
    "type": "above",
    "id": "bebca24e-a1ad-455a-95bc-c716a3d5bbe5",
    "userId": "a0821448-1cae-4000-bd04-937b0f852de6",
    "stockSymbol": "AAPL",
    "thresholdPrice": "20"
  },
  {
    "type": "below",
    "id": "32c2f25f-91fd-4f95-85bb-21752f1bd271",
    "userId": "30057bd5-b54e-4a30-910e-4c6660daed18",
    "stockSymbol": "AMZN",
    "thresholdPrice": "20"
  }
]
```

![image](https://github.com/user-attachments/assets/d8d62014-f4bd-4bf6-8798-ce6aaf590713)

If the application is ran in Development mode (e.g. locally on Visual Studio), there is also available the swagger UI http://localhost:5200/swagger

## Containerization

Use `docker compose up` to start the application on docker, for shutdown use `docker compose down`

## Scaling Considerations
Some ways to optimize the alert checking mechanism would be to:
* Use a persistent store like Redis or SQL
* Use Kafka or queues consumption instead of polling
  
### Possible Race Condition
A user may modify or delete an alert while it is being evaluated for triggering.

### Bottleneck
The bottleneck of the current design is that it's not distributed, so if there is an increase of user alerts, the application will suffer to go through all of them on a single instance. Ideally alerts are stored in a fast storage system like Redis and the application useses Kafka to be able to distrubite and scale up the number of instances of the application while also guranteeing ordering of the messages by partitioning the messages.
