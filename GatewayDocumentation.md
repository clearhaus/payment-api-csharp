<a name='assembly'></a>
# Gateway

## Contents

- [Account](#T-Clearhaus-Gateway-Account 'Clearhaus.Gateway.Account')
  - [#ctor(apiKey)](#M-Clearhaus-Gateway-Account-#ctor-System-String- 'Clearhaus.Gateway.Account.#ctor(System.String)')
  - [#ctor(apiKey,gatewayURL)](#M-Clearhaus-Gateway-Account-#ctor-System-String,System-String- 'Clearhaus.Gateway.Account.#ctor(System.String,System.String)')
  - [gatewayURL](#F-Clearhaus-Gateway-Account-gatewayURL 'Clearhaus.Gateway.Account.gatewayURL')
  - [Timeout](#F-Clearhaus-Gateway-Account-Timeout 'Clearhaus.Gateway.Account.Timeout')
  - [Authorize(amount,currency,cc,PARes,opts)](#M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card,System-String,Clearhaus-Gateway-AuthorizationRequestOptions- 'Clearhaus.Gateway.Account.Authorize(System.String,System.String,Clearhaus.Gateway.Card,System.String,Clearhaus.Gateway.AuthorizationRequestOptions)')
  - [AuthorizeAsync(amount,currency,cc,PARes,opts)](#M-Clearhaus-Gateway-Account-AuthorizeAsync-System-String,System-String,Clearhaus-Gateway-Card,System-String,Clearhaus-Gateway-AuthorizationRequestOptions- 'Clearhaus.Gateway.Account.AuthorizeAsync(System.String,System.String,Clearhaus.Gateway.Card,System.String,Clearhaus.Gateway.AuthorizationRequestOptions)')
  - [Capture(id,amount,textOnStatement)](#M-Clearhaus-Gateway-Account-Capture-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.Capture(System.String,System.String,System.String)')
  - [CaptureAsync(id,amount,textOnStatement)](#M-Clearhaus-Gateway-Account-CaptureAsync-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.CaptureAsync(System.String,System.String,System.String)')
  - [Credit(amount,currency,cc,textOnStatement,reference)](#M-Clearhaus-Gateway-Account-Credit-System-String,System-String,Clearhaus-Gateway-Card,System-String,System-String- 'Clearhaus.Gateway.Account.Credit(System.String,System.String,Clearhaus.Gateway.Card,System.String,System.String)')
  - [CreditAsync(amount,currency,cc,textOnStatement,reference)](#M-Clearhaus-Gateway-Account-CreditAsync-System-String,System-String,Clearhaus-Gateway-Card,System-String,System-String- 'Clearhaus.Gateway.Account.CreditAsync(System.String,System.String,Clearhaus.Gateway.Card,System.String,System.String)')
  - [FetchAccountInformation()](#M-Clearhaus-Gateway-Account-FetchAccountInformation 'Clearhaus.Gateway.Account.FetchAccountInformation')
  - [FetchAccountInformationAsync()](#M-Clearhaus-Gateway-Account-FetchAccountInformationAsync 'Clearhaus.Gateway.Account.FetchAccountInformationAsync')
  - [Refund(id,amount,textOnStatement)](#M-Clearhaus-Gateway-Account-Refund-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.Refund(System.String,System.String,System.String)')
  - [RefundAsync(id,amount,textOnStatement)](#M-Clearhaus-Gateway-Account-RefundAsync-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.RefundAsync(System.String,System.String,System.String)')
  - [SigningKeys(apiKey,rsaPrivateKey)](#M-Clearhaus-Gateway-Account-SigningKeys-System-String,System-String- 'Clearhaus.Gateway.Account.SigningKeys(System.String,System.String)')
  - [ValidAPIKey()](#M-Clearhaus-Gateway-Account-ValidAPIKey 'Clearhaus.Gateway.Account.ValidAPIKey')
  - [ValidAPIKeyAsync()](#M-Clearhaus-Gateway-Account-ValidAPIKeyAsync 'Clearhaus.Gateway.Account.ValidAPIKeyAsync')
  - [Void()](#M-Clearhaus-Gateway-Account-Void-System-String- 'Clearhaus.Gateway.Account.Void(System.String)')
  - [VoidAsync()](#M-Clearhaus-Gateway-Account-VoidAsync-System-String- 'Clearhaus.Gateway.Account.VoidAsync(System.String)')
- [AccountInfo](#T-Clearhaus-Gateway-AccountInfo 'Clearhaus.Gateway.AccountInfo')
  - [acquirer](#P-Clearhaus-Gateway-AccountInfo-acquirer 'Clearhaus.Gateway.AccountInfo.acquirer')
  - [country](#P-Clearhaus-Gateway-AccountInfo-country 'Clearhaus.Gateway.AccountInfo.country')
  - [descriptor](#P-Clearhaus-Gateway-AccountInfo-descriptor 'Clearhaus.Gateway.AccountInfo.descriptor')
  - [mcc](#P-Clearhaus-Gateway-AccountInfo-mcc 'Clearhaus.Gateway.AccountInfo.mcc')
  - [merchantID](#P-Clearhaus-Gateway-AccountInfo-merchantID 'Clearhaus.Gateway.AccountInfo.merchantID')
  - [name](#P-Clearhaus-Gateway-AccountInfo-name 'Clearhaus.Gateway.AccountInfo.name')
  - [transactionRules](#P-Clearhaus-Gateway-AccountInfo-transactionRules 'Clearhaus.Gateway.AccountInfo.transactionRules')
- [Acquirer](#T-Clearhaus-Gateway-Acquirer 'Clearhaus.Gateway.Acquirer')
  - [mastercardBin](#P-Clearhaus-Gateway-Acquirer-mastercardBin 'Clearhaus.Gateway.Acquirer.mastercardBin')
  - [visaBin](#P-Clearhaus-Gateway-Acquirer-visaBin 'Clearhaus.Gateway.Acquirer.visaBin')
- [Authorization](#T-Clearhaus-Gateway-Transaction-Authorization 'Clearhaus.Gateway.Transaction.Authorization')
  - [cscStatus](#F-Clearhaus-Gateway-Transaction-Authorization-cscStatus 'Clearhaus.Gateway.Transaction.Authorization.cscStatus')
- [AuthorizationRequestOptions](#T-Clearhaus-Gateway-AuthorizationRequestOptions 'Clearhaus.Gateway.AuthorizationRequestOptions')
  - [ip](#F-Clearhaus-Gateway-AuthorizationRequestOptions-ip 'Clearhaus.Gateway.AuthorizationRequestOptions.ip')
  - [recurring](#F-Clearhaus-Gateway-AuthorizationRequestOptions-recurring 'Clearhaus.Gateway.AuthorizationRequestOptions.recurring')
  - [reference](#F-Clearhaus-Gateway-AuthorizationRequestOptions-reference 'Clearhaus.Gateway.AuthorizationRequestOptions.reference')
  - [textOnStatement](#F-Clearhaus-Gateway-AuthorizationRequestOptions-textOnStatement 'Clearhaus.Gateway.AuthorizationRequestOptions.textOnStatement')
  - [GetParameters()](#M-Clearhaus-Gateway-AuthorizationRequestOptions-GetParameters 'Clearhaus.Gateway.AuthorizationRequestOptions.GetParameters')
- [Base](#T-Clearhaus-Gateway-Transaction-Base 'Clearhaus.Gateway.Transaction.Base')
  - [id](#F-Clearhaus-Gateway-Transaction-Base-id 'Clearhaus.Gateway.Transaction.Base.id')
  - [processedAt](#F-Clearhaus-Gateway-Transaction-Base-processedAt 'Clearhaus.Gateway.Transaction.Base.processedAt')
  - [status](#F-Clearhaus-Gateway-Transaction-Base-status 'Clearhaus.Gateway.Transaction.Base.status')
  - [IsSuccess()](#M-Clearhaus-Gateway-Transaction-Base-IsSuccess 'Clearhaus.Gateway.Transaction.Base.IsSuccess')
- [Capture](#T-Clearhaus-Gateway-Transaction-Capture 'Clearhaus.Gateway.Transaction.Capture')
  - [amount](#P-Clearhaus-Gateway-Transaction-Capture-amount 'Clearhaus.Gateway.Transaction.Capture.amount')
- [Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card')
  - [#ctor()](#M-Clearhaus-Gateway-Card-#ctor 'Clearhaus.Gateway.Card.#ctor')
  - [#ctor(pan,expireMonth,expireYear)](#M-Clearhaus-Gateway-Card-#ctor-System-String,System-String,System-String- 'Clearhaus.Gateway.Card.#ctor(System.String,System.String,System.String)')
  - [#ctor(pan,expireMonth,expireYear,csc)](#M-Clearhaus-Gateway-Card-#ctor-System-String,System-String,System-String,System-String- 'Clearhaus.Gateway.Card.#ctor(System.String,System.String,System.String,System.String)')
  - [csc](#F-Clearhaus-Gateway-Card-csc 'Clearhaus.Gateway.Card.csc')
  - [expireMonth](#F-Clearhaus-Gateway-Card-expireMonth 'Clearhaus.Gateway.Card.expireMonth')
  - [expireYear](#F-Clearhaus-Gateway-Card-expireYear 'Clearhaus.Gateway.Card.expireYear')
  - [pan](#F-Clearhaus-Gateway-Card-pan 'Clearhaus.Gateway.Card.pan')
- [Constants](#T-Clearhaus-Gateway-Constants 'Clearhaus.Gateway.Constants')
  - [GatewayTestURL](#F-Clearhaus-Gateway-Constants-GatewayTestURL 'Clearhaus.Gateway.Constants.GatewayTestURL')
  - [GatewayURL](#F-Clearhaus-Gateway-Constants-GatewayURL 'Clearhaus.Gateway.Constants.GatewayURL')
- [Credit](#T-Clearhaus-Gateway-Transaction-Credit 'Clearhaus.Gateway.Transaction.Credit')
  - [amount](#P-Clearhaus-Gateway-Transaction-Credit-amount 'Clearhaus.Gateway.Transaction.Credit.amount')
- [CSCStatus](#T-Clearhaus-Gateway-Transaction-CSCStatus 'Clearhaus.Gateway.Transaction.CSCStatus')
  - [matches](#P-Clearhaus-Gateway-Transaction-CSCStatus-matches 'Clearhaus.Gateway.Transaction.CSCStatus.matches')
  - [present](#P-Clearhaus-Gateway-Transaction-CSCStatus-present 'Clearhaus.Gateway.Transaction.CSCStatus.present')
- [Refund](#T-Clearhaus-Gateway-Transaction-Refund 'Clearhaus.Gateway.Transaction.Refund')
- [Status](#T-Clearhaus-Gateway-Transaction-Status 'Clearhaus.Gateway.Transaction.Status')
  - [code](#F-Clearhaus-Gateway-Transaction-Status-code 'Clearhaus.Gateway.Transaction.Status.code')
  - [message](#F-Clearhaus-Gateway-Transaction-Status-message 'Clearhaus.Gateway.Transaction.Status.message')
- [TokenizedCard](#T-Clearhaus-Gateway-TokenizedCard 'Clearhaus.Gateway.TokenizedCard')
  - [id](#F-Clearhaus-Gateway-TokenizedCard-id 'Clearhaus.Gateway.TokenizedCard.id')
  - [last4](#F-Clearhaus-Gateway-TokenizedCard-last4 'Clearhaus.Gateway.TokenizedCard.last4')
  - [scheme](#F-Clearhaus-Gateway-TokenizedCard-scheme 'Clearhaus.Gateway.TokenizedCard.scheme')
- [Void](#T-Clearhaus-Gateway-Transaction-Void 'Clearhaus.Gateway.Transaction.Void')

<a name='T-Clearhaus-Gateway-Account'></a>
## Account `type`

##### Namespace

Clearhaus.Gateway

##### Summary

Represents an account that integrates towards the Clearhaus gateway.

##### Example

This is an example of how to create an authorization and capture money.

```C#
 using Clearhaus.Gateway;
 using Clearhaus.Gateway.Transaction.Options;
 public static void main()
 {
     var apiKey = "My Secret UUID";
     var card = new Card
     {
         pan         = "some PAN",
         expireMonth = "12",
         expireYear  = "2047",
         csc         = "666"
     };
     var account = new Account(apiKey);
     var authOptions = new AuthorizationRequestOptions
     {
         recurring = true,
         reference = "sdg7SF12KJHjj"
     };
     Authorization myAuth;
     try
     {
         myAuth = new Authorize("100", "DKK", card, authOptions);
     }
     catch(ClrhsNetException e)
     {
         // Failure connecting to clearhaus.
         // You should retry this.
         return;
     }
     catch(ClrhsAuthException e)
     {
         // ApiKey was invalid
         // You need to change the apiKey.
         // This can be avoided by checking the key first:
         // account.ValidAPIKey() == true
         return;
     }
     catch(ClrhsGatewayException e)
     {
         // Something was funky with the Clearhaus gateway
         // You could retry this, but maybe give it a few seconds.
         return;
     }
     if (!myAuth.IsSuccess())
     {
         // The statuscode returned implies that an error occurred.
         Console.WriteLine(auth.status.message);
     }
 }
  
```

##### Remarks

Requests made with valid parameters will not throw exceptions, unless the invalid parameter is the API key.
 Result objects have a `status`field which contains a `code`and a `message`.
 These codes/messages can be looked up here https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#transaction-status-codes.

<a name='M-Clearhaus-Gateway-Account-#ctor-System-String-'></a>
### #ctor(apiKey) `constructor`

##### Summary

Creates an account object with associated apiKey.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| apiKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The API Key associated with the merchant account on which the transactions are to be performed. |

<a name='M-Clearhaus-Gateway-Account-#ctor-System-String,System-String-'></a>
### #ctor(apiKey,gatewayURL) `constructor`

##### Summary

Creates an account object with associated apiKey, specify alternate gateway address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| apiKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The API Key associated with the merchant account on which the transactions are to be performed. |
| gatewayURL | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | URL to use as remote Gateway address. Default `Constants.GatewayURL`.
. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | If gatewayUrl is null |
| [System.UriFormatException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UriFormatException 'System.UriFormatException') | If gatewayUrl is invalid URI |

<a name='F-Clearhaus-Gateway-Account-gatewayURL'></a>
### gatewayURL `constants`

##### Summary

URL address of Clearhaus Gateway. By default `Constants.GatewayURL`.
.

<a name='F-Clearhaus-Gateway-Account-Timeout'></a>
### Timeout `constants`

##### Summary

Set the timeout for all following requests against the Gateway.

##### Remarks

Default value is 5 seconds.
This value is passed straight through to a System.Net.HttpClient object without verification.

<a name='M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card,System-String,Clearhaus-Gateway-AuthorizationRequestOptions-'></a>
### Authorize(amount,currency,cc,PARes,opts) `method`

##### Summary

Creates an authorization against the Gateway.
See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#authentication

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount of money to reserve, minor units of `currency`(Required) |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Currency in which `amount`is specified (Required) |
| cc | [Clearhaus.Gateway.Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card') | Card to authorize against. [Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card')(Omittable, see Clearhaus Documentation) |
| PARes | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | 3D-Secure result (omittable) |
| opts | [Clearhaus.Gateway.AuthorizationRequestOptions](#T-Clearhaus-Gateway-AuthorizationRequestOptions 'Clearhaus.Gateway.AuthorizationRequestOptions') | Optional parameters for authorizations or null (Omittable) |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-AuthorizeAsync-System-String,System-String,Clearhaus-Gateway-Card,System-String,Clearhaus-Gateway-AuthorizationRequestOptions-'></a>
### AuthorizeAsync(amount,currency,cc,PARes,opts) `method`

##### Summary

[Authorize](#M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card,System-String,Clearhaus-Gateway-AuthorizationRequestOptions- 'Clearhaus.Gateway.Account.Authorize(System.String,System.String,Clearhaus.Gateway.Card,System.String,Clearhaus.Gateway.AuthorizationRequestOptions)')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount of money to reserve, minor units of `currency`(Required) |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Currency in which `amount`is specified (Required) |
| cc | [Clearhaus.Gateway.Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card') | Card to authorize against. [Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card')(Omittable, see Clearhaus Documentation) |
| PARes | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | 3D-Secure result (omittable) |
| opts | [Clearhaus.Gateway.AuthorizationRequestOptions](#T-Clearhaus-Gateway-AuthorizationRequestOptions 'Clearhaus.Gateway.AuthorizationRequestOptions') | Optional parameters for authorizations or null (Omittable) |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-Capture-System-String,System-String,System-String-'></a>
### Capture(id,amount,textOnStatement) `method`

##### Summary

Capture reserved money.
See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#captures

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | UUID of authorization (Required) |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to capture (Omittable) |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Text to appear on cardholder bank statement (Omittable) |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-CaptureAsync-System-String,System-String,System-String-'></a>
### CaptureAsync(id,amount,textOnStatement) `method`

##### Summary

Capture reserved money.
See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#captures

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | UUID of authorization (Required) |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to capture (Omittable) |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Text to appear on cardholder bank statement (Omittable) |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-Credit-System-String,System-String,Clearhaus-Gateway-Card,System-String,System-String-'></a>
### Credit(amount,currency,cc,textOnStatement,reference) `method`

##### Summary

Transfer funds to cardholder account.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to transfer |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Currency to use for transfer |
| cc | [Clearhaus.Gateway.Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card') | Card to transfer to |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Statement on cardholders bank account |
| reference | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | External reference |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-CreditAsync-System-String,System-String,Clearhaus-Gateway-Card,System-String,System-String-'></a>
### CreditAsync(amount,currency,cc,textOnStatement,reference) `method`

##### Summary

Transfer funds to cardholder account.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to transfer |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Currency to use for transfer |
| cc | [Clearhaus.Gateway.Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card') | Card to transfer to |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Statement on cardholders bank account |
| reference | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | External reference |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-FetchAccountInformation'></a>
### FetchAccountInformation() `method`

##### Summary

Fetches account information about the associated apiKey.

##### Returns

An AccountInfo object

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

##### Remarks

Calls the gateways 'account/' endpoint.

<a name='M-Clearhaus-Gateway-Account-FetchAccountInformationAsync'></a>
### FetchAccountInformationAsync() `method`

##### Summary

Fetches account information about the associated apiKey.

##### Returns

An AccountInfo object

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

##### Remarks

Calls the gateways 'account/' endpoint.

<a name='M-Clearhaus-Gateway-Account-Refund-System-String,System-String,System-String-'></a>
### Refund(id,amount,textOnStatement) `method`

##### Summary

Refund funds captured on an authorization.
https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#refunds

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | UUID of authorization |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to refund or empty string, must be less than captured |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Overrides text on authorization |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-RefundAsync-System-String,System-String,System-String-'></a>
### RefundAsync(id,amount,textOnStatement) `method`

##### Summary

Refund funds captured on an authorization.
https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#refunds

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | UUID of authorization |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to refund or empty string, must be less than captured |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Overrides text on authorization |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-SigningKeys-System-String,System-String-'></a>
### SigningKeys(apiKey,rsaPrivateKey) `method`

##### Summary

Sets the Clearhaus API key and associated rsaPrivateKey to be
 used for signing. When these parameters are set, all requests are
 signed.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| apiKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | API key issued to trusted integrator. |
| rsaPrivateKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | RSA Signing key PEM associated with apiKey. See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#rsa-signature. |

##### Example

```C#
 // This apikey is associated with the account. In general, this
 // represents a merchants API key.
 var apiKey        = "[My secret UUID]";
 // This APIKey would likely be your key, that is, the key
 // belonging to a technical integrator.
 var signingApiKey = "[My secret UUID]";
 // This is the private key corresponding to a public key you have
 // exchanged with Clearhaus.
 var rsaPrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
     MIICWwIBAAKBgQC9KAaGN0y4vHOuXFZLE+GHIgd6Ya8IgL55cWxXzWO8T/AykXyi
     kayr4vy3aTpcJ8JEsZcDWnOkDpyBbaULLjfH7WtTm1Vyt4GrHatv6XtSB921rAIB
     BxKAZUU0BDLdFlztjFgu8qRow2GPxEjltDgiEINwYzhYBbST9EyrowvgcwIBAwKB
     gH4arwQk3dB9onQ9jty3669sBPxBH1sAfvug8uUzl9Lf9XcLqGxhHcfsqHpGJuga
     gYMhD1eRom1fEwDzw1zJeoQjhNuwMmVEajbCrmboT1+wXOZYZdf6UqwgzUJOFCES
     M8cIeStzdnRGLdW56b4q4edohA2Gtb3DV3RslA9xvwCbAkEA3rTybL5hChFAMuiK
     zo5SeSDcHI4MLX1q4TAJ2Dyb4YTE4N8ok2YA8fX+oZOwEDYiM8HfZtVbKKByqOud
     4M7/jQJBANlvF6ZLecbRGMa9Sr518AYxgArbMOIZE1LhRrbYD5mKfh7DRTIMuWgm
     0IvWmGOvJL/7fLJSYDgQ8qiC9peeX/8CQQCUeKGd1ECxYNV3RbHfCYxQwJK9tAge
     U5yWIAaQKGfrrdiV6hsM7qtL+VRrt8q1eWwigT+Z45IbFaHF8mlAif+zAkEAkPS6
     btz72eC7LyjcfvlKrsuqsed17BC3jJYvJJAKZlxUFIIuIV3Q8BngXTm67R9t1VJT
     IYxAJWChxaykZRQ//wJAFM6sXZYIl9SKAcY6iRFElmL1nw3NTFWKU/2/y5fsOn9U
     drtnrCH+i7Iedp+K0qUASitBWAATnHEJ2Q0Pc8LEJQ==
     -----END RSA PRIVATE KEY-----";
 var account = new Account(apiKey);
 account.SigningKeys(signingApiKey, rsaPrivateKey);
 // If the key is trusted, API requests against the Clearhaus
 // gateway using `account` will now have a signed body.
  
```

<a name='M-Clearhaus-Gateway-Account-ValidAPIKey'></a>
### ValidAPIKey() `method`

##### Summary

Connects to the Gateway attempts to authorize with the apiKey.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Thrown if connection to the gateway fails. |

<a name='M-Clearhaus-Gateway-Account-ValidAPIKeyAsync'></a>
### ValidAPIKeyAsync() `method`

##### Summary

Connects to the Gateway attempts to authorize with the apiKey.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Thrown if connection to the gateway fails. |

<a name='M-Clearhaus-Gateway-Account-Void-System-String-'></a>
### Void() `method`

##### Summary

Void (annul) an authorization
See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#voids

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-Gateway-Account-VoidAsync-System-String-'></a>
### VoidAsync() `method`

##### Summary

Void (annul) an authorization
See https://github.com/clearhaus/gateway-api-docs/blob/master/source/index.md#voids

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='T-Clearhaus-Gateway-AccountInfo'></a>
## AccountInfo `type`

##### Namespace

Clearhaus.Gateway

##### Summary

Deserialization of account information supplied by an /account call against the gateway.

<a name='P-Clearhaus-Gateway-AccountInfo-acquirer'></a>
### acquirer `property`

##### Summary

Acquirer BIN information

<a name='P-Clearhaus-Gateway-AccountInfo-country'></a>
### country `property`

##### Summary

Merchant name

<a name='P-Clearhaus-Gateway-AccountInfo-descriptor'></a>
### descriptor `property`

##### Summary

The default `text_on_statement`

<a name='P-Clearhaus-Gateway-AccountInfo-mcc'></a>
### mcc `property`

##### Summary

Merchant Category Code

<a name='P-Clearhaus-Gateway-AccountInfo-merchantID'></a>
### merchantID `property`

##### Summary

ID of merchant in Clearhaus system.

<a name='P-Clearhaus-Gateway-AccountInfo-name'></a>
### name `property`

##### Summary

Merchant name

<a name='P-Clearhaus-Gateway-AccountInfo-transactionRules'></a>
### transactionRules `property`

##### Summary

Transaction rules in Clearhaus rule language

<a name='T-Clearhaus-Gateway-Acquirer'></a>
## Acquirer `type`

##### Namespace

Clearhaus.Gateway

##### Summary

Information about acquirer BIN

<a name='P-Clearhaus-Gateway-Acquirer-mastercardBin'></a>
### mastercardBin `property`

##### Summary

BIN for MasterCard systems

<a name='P-Clearhaus-Gateway-Acquirer-visaBin'></a>
### visaBin `property`

##### Summary

BIN for VISA systems

<a name='T-Clearhaus-Gateway-Transaction-Authorization'></a>
## Authorization `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Class that represents a completed authorization.

<a name='F-Clearhaus-Gateway-Transaction-Authorization-cscStatus'></a>
### cscStatus `constants`

##### Summary

CSC Status

<a name='T-Clearhaus-Gateway-AuthorizationRequestOptions'></a>
## AuthorizationRequestOptions `type`

##### Namespace

Clearhaus.Gateway

##### Summary

Optionals for Authorization transaction, .

<a name='F-Clearhaus-Gateway-AuthorizationRequestOptions-ip'></a>
### ip `constants`

##### Summary

IPv4/IPv6 address of cardholder initiating authorization

<a name='F-Clearhaus-Gateway-AuthorizationRequestOptions-recurring'></a>
### recurring `constants`

##### Summary

Mark authorization as recurring

<a name='F-Clearhaus-Gateway-AuthorizationRequestOptions-reference'></a>
### reference `constants`

##### Summary

Authorization reference

<a name='F-Clearhaus-Gateway-AuthorizationRequestOptions-textOnStatement'></a>
### textOnStatement `constants`

##### Summary

Statement on cardholders bank transaction

<a name='M-Clearhaus-Gateway-AuthorizationRequestOptions-GetParameters'></a>
### GetParameters() `method`

##### Summary

Returns the parameters with correct keys, used internally.

##### Parameters

This method has no parameters.

<a name='T-Clearhaus-Gateway-Transaction-Base'></a>
## Base `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Base class for a Clearhaus transaction result

<a name='F-Clearhaus-Gateway-Transaction-Base-id'></a>
### id `constants`

##### Summary

UUID identifying the transaction

<a name='F-Clearhaus-Gateway-Transaction-Base-processedAt'></a>
### processedAt `constants`

##### Summary

Datetime the transaction was processed

<a name='F-Clearhaus-Gateway-Transaction-Base-status'></a>
### status `constants`

##### Summary

Status of the query

<a name='M-Clearhaus-Gateway-Transaction-Base-IsSuccess'></a>
### IsSuccess() `method`

##### Summary

Helper to check if transaction was a success

##### Parameters

This method has no parameters.

<a name='T-Clearhaus-Gateway-Transaction-Capture'></a>
## Capture `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Represents a capture transaction

<a name='P-Clearhaus-Gateway-Transaction-Capture-amount'></a>
### amount `property`

##### Summary

The amount captured by this transaction

<a name='T-Clearhaus-Gateway-Card'></a>
## Card `type`

##### Namespace

Clearhaus.Gateway

##### Summary

Represents a credit card

<a name='M-Clearhaus-Gateway-Card-#ctor'></a>
### #ctor() `constructor`

##### Summary

Emtpy constructor

##### Parameters

This constructor has no parameters.

<a name='M-Clearhaus-Gateway-Card-#ctor-System-String,System-String,System-String-'></a>
### #ctor(pan,expireMonth,expireYear) `constructor`

##### Summary

Construct a new card

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pan | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Primary Account Number" |
| expireMonth | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Month of card expiry |
| expireYear | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Year of card expiry |

<a name='M-Clearhaus-Gateway-Card-#ctor-System-String,System-String,System-String,System-String-'></a>
### #ctor(pan,expireMonth,expireYear,csc) `constructor`

##### Summary

Construct a new card

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pan | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Primary Account Number" |
| expireMonth | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Month of card expiry |
| expireYear | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Year of card expiry |
| csc | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | CSC |

<a name='F-Clearhaus-Gateway-Card-csc'></a>
### csc `constants`

##### Summary

Card security code

<a name='F-Clearhaus-Gateway-Card-expireMonth'></a>
### expireMonth `constants`

##### Summary

Month of card expiry

<a name='F-Clearhaus-Gateway-Card-expireYear'></a>
### expireYear `constants`

##### Summary

Year of card expiry

<a name='F-Clearhaus-Gateway-Card-pan'></a>
### pan `constants`

##### Summary

Primary Account Number

<a name='T-Clearhaus-Gateway-Constants'></a>
## Constants `type`

##### Namespace

Clearhaus.Gateway

##### Summary

For gateway constants

<a name='F-Clearhaus-Gateway-Constants-GatewayTestURL'></a>
### GatewayTestURL `constants`

##### Summary

Testing Gateway endpoint

<a name='F-Clearhaus-Gateway-Constants-GatewayURL'></a>
### GatewayURL `constants`

##### Summary

Production Gateway endpoint

<a name='T-Clearhaus-Gateway-Transaction-Credit'></a>
## Credit `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Represents a credit transaction

<a name='P-Clearhaus-Gateway-Transaction-Credit-amount'></a>
### amount `property`

##### Summary

Amount of money transferred

<a name='T-Clearhaus-Gateway-Transaction-CSCStatus'></a>
## CSCStatus `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Status of CSC for the authorization.

<a name='P-Clearhaus-Gateway-Transaction-CSCStatus-matches'></a>
### matches `property`

##### Summary

Whether the CSC matched

<a name='P-Clearhaus-Gateway-Transaction-CSCStatus-present'></a>
### present `property`

##### Summary

Whether the CSC was present in authorization

<a name='T-Clearhaus-Gateway-Transaction-Refund'></a>
## Refund `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Represents a completed refund transaction

<a name='T-Clearhaus-Gateway-Transaction-Status'></a>
## Status `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Status of a query

<a name='F-Clearhaus-Gateway-Transaction-Status-code'></a>
### code `constants`

##### Summary

See http://docs.gateway.clearhaus.com/#Transactionstatuscodes

<a name='F-Clearhaus-Gateway-Transaction-Status-message'></a>
### message `constants`

##### Summary

Message associated with status code

<a name='T-Clearhaus-Gateway-TokenizedCard'></a>
## TokenizedCard `type`

##### Namespace

Clearhaus.Gateway

##### Summary

Represents a card tokenized by Clearhaus transaction gateway.

##### Remarks

Card tokenization is deprecated.

<a name='F-Clearhaus-Gateway-TokenizedCard-id'></a>
### id `constants`

##### Summary

UUID

<a name='F-Clearhaus-Gateway-TokenizedCard-last4'></a>
### last4 `constants`

##### Summary

Last 4 digits

<a name='F-Clearhaus-Gateway-TokenizedCard-scheme'></a>
### scheme `constants`

##### Summary

Card scheme

<a name='T-Clearhaus-Gateway-Transaction-Void'></a>
## Void `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Represents a completed void transaction
