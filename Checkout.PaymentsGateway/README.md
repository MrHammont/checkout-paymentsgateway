# Checkout-PaymentsGateway

## Background
As part of the interview process with Checkout.com, I've been asked to build a payment gateway that will allow merchants to delegate the process of their payments to this service.

## Requirements
The product requirements for this initial phase are the following:
1. A merchant should be able to process a payment through the payment gateway and receive either a
successful or unsuccessful response
2. A merchant should be able to retrieve the details of a previously made payment

### Process a payment
The payment gateway will need to provide merchants with a way to process a payment. To do this, the
merchant should be able to submit a request to the payment gateway. A payment request should include
appropriate fields such as the card number, expiry month/date, amount, currency, and cvv.

####  Note: Simulating the bank
In the solution I've been asked to simulate or otherwise mock out the Bank part of processing flow. This component should be able to be switched out for a real bank once we move into
production. We should assume that a bank response returns a unique identifier and a status that
indicates whether the payment was successful.

### Retrieving a payment’s details

The second requirement for the payment gateway is to allow a merchant to retrieve details of a
previously made payment using its identifier. Doing this will help the merchant with their reconciliation
and reporting needs. The response should include a masked card number and card details along with a
status code which indicates the result of the payment.


## Installation

To run this project you'll need the following tools:
- Visual studio 2019 
- .NET Core 3.1 SDK installed
- Docker with Linux containers running

## Usage

You'll need to clone this repository and run it using Visual Studio. It will build and run the docker-compose.yml at the root of the project for you.

Once the project is running, two Open API endpoints will be available at the links below, which will provide with all the required information about the api definition, request and response contracts, and the ability of trying the functionality of the APIs
- [Identity API](https://localhost:8001/swagger/index.html)
- [PaymentsGateway API](https://localhost:5001/swagger/index.html)

### Example of usage
In order to request information or create any transaction through the *PaymentsGateway API* you must acquire a valid token from the *Identity API*.
```
curl -X POST "https://localhost:8001/identity/v1/register" -H "accept: text/plain" -H "Content-Type: application/json" -d "{\"email\":\"user@email.com\",\"password\":\"String123!\"}"
```
The previous request will return an object with the user's personal token, valid for 10 minutes, and a the reference of the refresh token in case it's needed
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGVtYWlsY29tIiwianRpIjoiZDRmOTQ2ZWYtZDc4YS00YzhhLWFjNjYtNDM3NmVjMWJjZGI3IiwiZW1haWwiOiJ1c2VyQGVtYWlsY29tIiwiaWQiOiIwNDE4YTE1YS02ZThlLTRjYWItYmVjMy0yNDkwNWJjM2UwNTAiLCJuYmYiOjE1ODQ1NjgwMDYsImV4cCI6MTU4NDU2ODYwNiwiaWF0IjoxNTg0NTY4MDA2fQ.joSh4J6xhX1emgmU4uz5QuBSgk7nQwsBHrZKZ-I1FKg",
  "refreshToken": "8546f76e-534d-4828-ad6b-2f2f20b1c93d"
}
```
The next would be to create a transaction adding the previous token to the header as `Authorization: bearer {token}` and providing a body with the information of the transaction
```
curl -X POST "https://localhost:5001/paymentsgateway/v1/payments" -H "accept: text/plain" -H "Authorization: bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGVtYWlsY29tIiwianRpIjoiZDRmOTQ2ZWYtZDc4YS00YzhhLWFjNjYtNDM3NmVjMWJjZGI3IiwiZW1haWwiOiJ1c2VyQGVtYWlsY29tIiwiaWQiOiIwNDE4YTE1YS02ZThlLTRjYWItYmVjMy0yNDkwNWJjM2UwNTAiLCJuYmYiOjE1ODQ1NjgwMDYsImV4cCI6MTU4NDU2ODYwNiwiaWF0IjoxNTg0NTY4MDA2fQ.joSh4J6xhX1emgmU4uz5QuBSgk7nQwsBHrZKZ-I1FKg" -H "Content-Type: application/json" -d "{\"cardHolderName\":\"Jonh Smith\",\"cardNumber\":\"4539252527166077\",\"cardExpirationMonth\":\"12\",\"cardExpirationYear\":\"2020\",\"cvv\":\"300\",\"amount\":250,\"currency\":\"GBP\"}"
```
If everything goes as expected and the response status is 200, the response body will contain information about the transaction as shown below
```
{
  "data": {
    "transactionId": "ce5ec595-4772-4a9a-97c2-d6bd608c7682",
    "transactionStatus": "Successful"
  },
  "status": "Success"
}
```
The next and last step would be to retrieve the transaction details by requesting the *Get* endpoint with the *transactionId* that we got in the last request
```
curl -X GET "https://localhost:5001/paymentsgateway/v1/payments/ce5ec595-4772-4a9a-97c2-d6bd608c7682" -H "accept: text/plain" -H "Authorization: bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGVtYWlsY29tIiwianRpIjoiNzgwZmM2YjEtODk3OC00ODU3LTkwN2ItYzQzMjFkOTFjMzg5IiwiZW1haWwiOiJ1c2VyQGVtYWlsY29tIiwiaWQiOiIwNDE4YTE1YS02ZThlLTRjYWItYmVjMy0yNDkwNWJjM2UwNTAiLCJuYmYiOjE1ODQ1NzA0NzAsImV4cCI6MTU4NDU3MTA3MCwiaWF0IjoxNTg0NTcwNDcwfQ.7RSsTHwqSEz9YVhfJQ6dglAWKtHPMB3odtehFXIgDu4"
```
```
{
    "transactionId": "ce5ec595-4772-4a9a-97c2-d6bd608c7682",
    "cardDetails": {
      "holderName": "Jonh Smith",
      "number": "************6077",
      "expirationDate": "12/2020"
    },
    "transactionDetails": {
      "amount": 250,
      "currency": "GBP",
      "transactionDate": "2020-03-18T21:53:20.8735097",
      "transactionStatus": "Successful"
    }
  },
  "status": "Success"
}
```
## API Specifications
### Identity API
#### Endpoints
| Resource | Operation | Verb | Uri |
| ---      | ---       | ---  | --- |
| /identity/v1/login | Login with existing user | POST | https://localhost:8001/identity/v1/login |
| /identity/v1/register | Register a new user | POST | https://localhost:8001/identity/v1/register |
| /identity/v1/refresh | Refresh Jwt token | POST | https://localhost:8001/identity/v1/refresh |

### PaymentsGateway API
#### Endpoints
| Resource | Operation | Verb | Uri |
| ---      | ---       | ---  | --- |
| /paymentsgateway/v1/payments/{transactionId} | Get transaction data | GET | https://localhost:5001/paymentsgateway/v1/payments/{transactionId} |
| /paymentsgateway/v1/payments | Create a new transaction | POST | https://localhost:5001/paymentsgateway/v1/payments |

## Directory Structure

```
-  root
    | - src
    |    | -> Solution projects
    |
    | - tests    
    |    | -> Test projects
    |
    | - deployment
    |    | - build
    |    |    | - build.dockerfile
    |    |    | - runtime.dockerfile
    |    |    | - run-build.sh
    |    | - Makefile
    |
    | - docker-compose.yml


```

## Solution Diagram

![Image description](https://lh5.googleusercontent.com/kpKknAEEmqHPDzo1k-H5SWhQaShxfCn6Cq_TJdddyTyLzEyQPQKEQ9X1VYIo0RmIfhX7D3x0Sgfpk8bkAVhL=w1920-h937-rw)]

## Building

Under deployment/build folder you'll find *run-build.sh* that will build the solution and run the tests in a docker container.
It will also generate the *PaymentsGateway API* artifacts and it will create the runtime image, ready to be deployed.

## Extra implementations

- Application Logging and Metrics ready to be shared with AppInsights
- Containerization
- Authorization and Authentication
- Open API for both APIs
- Continuous integration script
- Encryption
- Data Storage
- Response Caching
- Resilliency
- Dependency instrumentation
- Tracing