<a name='assembly'></a>
# Gateway

## Contents

- [Account](#T-Clearhaus-Gateway-Account 'Clearhaus.Gateway.Account')
  - [#ctor(apiKey)](#M-Clearhaus-Gateway-Account-#ctor-System-String- 'Clearhaus.Gateway.Account.#ctor(System.String)')
  - [gatewayURL](#F-Clearhaus-Gateway-Account-gatewayURL 'Clearhaus.Gateway.Account.gatewayURL')
  - [Authorize()](#M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card- 'Clearhaus.Gateway.Account.Authorize(System.String,System.String,Clearhaus.Gateway.Card)')
  - [Authorize(amount,currency,cc,opts)](#M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card,Clearhaus-Gateway-Transaction-Options-AuthorizationOptions- 'Clearhaus.Gateway.Account.Authorize(System.String,System.String,Clearhaus.Gateway.Card,Clearhaus.Gateway.Transaction.Options.AuthorizationOptions)')
  - [Capture()](#M-Clearhaus-Gateway-Account-Capture-System-String- 'Clearhaus.Gateway.Account.Capture(System.String)')
  - [Capture()](#M-Clearhaus-Gateway-Account-Capture-Clearhaus-Gateway-Transaction-Authorization- 'Clearhaus.Gateway.Account.Capture(Clearhaus.Gateway.Transaction.Authorization)')
  - [Capture()](#M-Clearhaus-Gateway-Account-Capture-Clearhaus-Gateway-Transaction-Authorization,System-String- 'Clearhaus.Gateway.Account.Capture(Clearhaus.Gateway.Transaction.Authorization,System.String)')
  - [Capture(id,amount,textOnStatement)](#M-Clearhaus-Gateway-Account-Capture-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.Capture(System.String,System.String,System.String)')
  - [Credit(amount,currency,cc,textOnStatement,reference)](#M-Clearhaus-Gateway-Account-Credit-System-String,System-String,Clearhaus-Gateway-Card,System-String,System-String- 'Clearhaus.Gateway.Account.Credit(System.String,System.String,Clearhaus.Gateway.Card,System.String,System.String)')
  - [FetchAccountInformation()](#M-Clearhaus-Gateway-Account-FetchAccountInformation 'Clearhaus.Gateway.Account.FetchAccountInformation')
  - [newRestBuilder(path,args)](#M-Clearhaus-Gateway-Account-newRestBuilder-System-String,System-String[]- 'Clearhaus.Gateway.Account.newRestBuilder(System.String,System.String[])')
  - [Refund(id,amount,textOnStatement)](#M-Clearhaus-Gateway-Account-Refund-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.Refund(System.String,System.String,System.String)')
  - [SigningKeys(apiKey,rsaPrivateKey)](#M-Clearhaus-Gateway-Account-SigningKeys-System-String,System-String- 'Clearhaus.Gateway.Account.SigningKeys(System.String,System.String)')
  - [ValidAPIKey()](#M-Clearhaus-Gateway-Account-ValidAPIKey 'Clearhaus.Gateway.Account.ValidAPIKey')
  - [Void()](#M-Clearhaus-Gateway-Account-Void-Clearhaus-Gateway-Transaction-Authorization- 'Clearhaus.Gateway.Account.Void(Clearhaus.Gateway.Transaction.Authorization)')
  - [Void()](#M-Clearhaus-Gateway-Account-Void-System-String- 'Clearhaus.Gateway.Account.Void(System.String)')
- [Authorization](#T-Clearhaus-Gateway-Transaction-Authorization 'Clearhaus.Gateway.Transaction.Authorization')
  - [cscStatus](#P-Clearhaus-Gateway-Transaction-Authorization-cscStatus 'Clearhaus.Gateway.Transaction.Authorization.cscStatus')
- [AuthorizationOptions](#T-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions 'Clearhaus.Gateway.Transaction.Options.AuthorizationOptions')
  - [ip](#F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-ip 'Clearhaus.Gateway.Transaction.Options.AuthorizationOptions.ip')
  - [recurring](#F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-recurring 'Clearhaus.Gateway.Transaction.Options.AuthorizationOptions.recurring')
  - [reference](#F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-reference 'Clearhaus.Gateway.Transaction.Options.AuthorizationOptions.reference')
  - [textOnStatement](#F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-textOnStatement 'Clearhaus.Gateway.Transaction.Options.AuthorizationOptions.textOnStatement')
  - [GetParameters()](#M-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-GetParameters 'Clearhaus.Gateway.Transaction.Options.AuthorizationOptions.GetParameters')
- [Base](#T-Clearhaus-Gateway-Transaction-Base 'Clearhaus.Gateway.Transaction.Base')
  - [id](#P-Clearhaus-Gateway-Transaction-Base-id 'Clearhaus.Gateway.Transaction.Base.id')
  - [processedAt](#P-Clearhaus-Gateway-Transaction-Base-processedAt 'Clearhaus.Gateway.Transaction.Base.processedAt')
  - [status](#P-Clearhaus-Gateway-Transaction-Base-status 'Clearhaus.Gateway.Transaction.Base.status')
  - [isSuccess()](#M-Clearhaus-Gateway-Transaction-Base-isSuccess 'Clearhaus.Gateway.Transaction.Base.isSuccess')
- [Capture](#T-Clearhaus-Gateway-Transaction-Capture 'Clearhaus.Gateway.Transaction.Capture')
  - [amount](#P-Clearhaus-Gateway-Transaction-Capture-amount 'Clearhaus.Gateway.Transaction.Capture.amount')
- [Credit](#T-Clearhaus-Gateway-Transaction-Credit 'Clearhaus.Gateway.Transaction.Credit')
  - [amount](#P-Clearhaus-Gateway-Transaction-Credit-amount 'Clearhaus.Gateway.Transaction.Credit.amount')
- [CSCStatus](#T-Clearhaus-Gateway-Transaction-CSCStatus 'Clearhaus.Gateway.Transaction.CSCStatus')
  - [matches](#P-Clearhaus-Gateway-Transaction-CSCStatus-matches 'Clearhaus.Gateway.Transaction.CSCStatus.matches')
  - [present](#P-Clearhaus-Gateway-Transaction-CSCStatus-present 'Clearhaus.Gateway.Transaction.CSCStatus.present')
- [Refund](#T-Clearhaus-Gateway-Transaction-Refund 'Clearhaus.Gateway.Transaction.Refund')
- [RestRequest](#T-Clearhaus-Gateway-Util-RestRequest 'Clearhaus.Gateway.Util.RestRequest')
- [RestRequestBuilder](#T-Clearhaus-Gateway-Util-RestRequestBuilder 'Clearhaus.Gateway.Util.RestRequestBuilder')
- [Status](#T-Clearhaus-Gateway-Transaction-Status 'Clearhaus.Gateway.Transaction.Status')
  - [code](#P-Clearhaus-Gateway-Transaction-Status-code 'Clearhaus.Gateway.Transaction.Status.code')
  - [message](#P-Clearhaus-Gateway-Transaction-Status-message 'Clearhaus.Gateway.Transaction.Status.message')
- [Void](#T-Clearhaus-Gateway-Transaction-Void 'Clearhaus.Gateway.Transaction.Void')

<a name='T-Clearhaus-Gateway-Account'></a>
## Account `type`

##### Namespace

Clearhaus.Gateway

##### Summary

Represents an account that integrates towards the Clearhaus gateway.

<a name='M-Clearhaus-Gateway-Account-#ctor-System-String-'></a>
### #ctor(apiKey) `constructor`

##### Summary

Creates an account object with associated apiKey.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| apiKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The API Key associated with the merchant account where transactions must end up. |

<a name='F-Clearhaus-Gateway-Account-gatewayURL'></a>
### gatewayURL `constants`

##### Summary

URL address of Clearhaus Gateway. By default `Constants.GatewayURL`.
            .

<a name='M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card-'></a>
### Authorize() `method`

##### Summary

[Authorize](#M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card- 'Clearhaus.Gateway.Account.Authorize(System.String,System.String,Clearhaus.Gateway.Card)')

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-Gateway-Account-Authorize-System-String,System-String,Clearhaus-Gateway-Card,Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-'></a>
### Authorize(amount,currency,cc,opts) `method`

##### Summary

Creates an authorization against the Gateway ()

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Minor units of `currency`. |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Currency in which `amount` is specified. |
| cc | [Clearhaus.Gateway.Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card') | Card to authorize against. [Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card'). |
| opts | [Clearhaus.Gateway.Transaction.Options.AuthorizationOptions](#T-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions 'Clearhaus.Gateway.Transaction.Options.AuthorizationOptions') | Optionals parameters for authorizations or null. |

<a name='M-Clearhaus-Gateway-Account-Capture-System-String-'></a>
### Capture() `method`

##### Summary

[Capture](#M-Clearhaus-Gateway-Account-Capture-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.Capture(System.String,System.String,System.String)')

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-Gateway-Account-Capture-Clearhaus-Gateway-Transaction-Authorization-'></a>
### Capture() `method`

##### Summary

[Capture](#M-Clearhaus-Gateway-Account-Capture-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.Capture(System.String,System.String,System.String)')

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-Gateway-Account-Capture-Clearhaus-Gateway-Transaction-Authorization,System-String-'></a>
### Capture() `method`

##### Summary

[Capture](#M-Clearhaus-Gateway-Account-Capture-System-String,System-String,System-String- 'Clearhaus.Gateway.Account.Capture(System.String,System.String,System.String)')

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-Gateway-Account-Capture-System-String,System-String,System-String-'></a>
### Capture(id,amount,textOnStatement) `method`

##### Summary

Capture reserved money.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | UUID of authorization |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to capture |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Text to appear on cardholder bank statement |

<a name='M-Clearhaus-Gateway-Account-Credit-System-String,System-String,Clearhaus-Gateway-Card,System-String,System-String-'></a>
### Credit(amount,currency,cc,textOnStatement,reference) `method`

##### Summary

Transfer funds to cartholder account.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to transfer |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Currency to use for transfer |
| cc | [Clearhaus.Gateway.Card](#T-Clearhaus-Gateway-Card 'Clearhaus.Gateway.Card') | Card to transfer to |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Statement on cardholders bank account |
| reference | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | External reference |

<a name='M-Clearhaus-Gateway-Account-FetchAccountInformation'></a>
### FetchAccountInformation() `method`

##### Summary

Fetches account information about the class apiKey.

##### Returns

An AccountInfo object

##### Parameters

This method has no parameters.

##### Remarks

Calls the gateways 'account/' endpoint.

<a name='M-Clearhaus-Gateway-Account-newRestBuilder-System-String,System-String[]-'></a>
### newRestBuilder(path,args) `method`

##### Summary

Helper to ensure apikey is applied to all rest calls.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | URL path with format options like string.Format. |
| args | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | String arguments to format string path. |

<a name='M-Clearhaus-Gateway-Account-Refund-System-String,System-String,System-String-'></a>
### Refund(id,amount,textOnStatement) `method`

##### Summary

Refund funds captured on an authorization.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Authorization UUID |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Amount to refund, must be less than captured |
| textOnStatement | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Overrides text on authorization |

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
| rsaPrivateKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | RSA Signing key associated with apiKey. |

<a name='M-Clearhaus-Gateway-Account-ValidAPIKey'></a>
### ValidAPIKey() `method`

##### Summary

Connects to the Gateway attempts to authorize with the apiKey.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.Gateway.ClrhsNetException](#T-Clearhaus-Gateway-ClrhsNetException 'Clearhaus.Gateway.ClrhsNetException') | Thrown if connection to the gateway fails. |

<a name='M-Clearhaus-Gateway-Account-Void-Clearhaus-Gateway-Transaction-Authorization-'></a>
### Void() `method`

##### Summary

[Void](#M-Clearhaus-Gateway-Account-Void-System-String- 'Clearhaus.Gateway.Account.Void(System.String)')

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-Gateway-Account-Void-System-String-'></a>
### Void() `method`

##### Summary

Void (annul) an authorization.

##### Parameters

This method has no parameters.

<a name='T-Clearhaus-Gateway-Transaction-Authorization'></a>
## Authorization `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Class that represents a completed authorization.

<a name='P-Clearhaus-Gateway-Transaction-Authorization-cscStatus'></a>
### cscStatus `property`

##### Summary

CSC Status

<a name='T-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions'></a>
## AuthorizationOptions `type`

##### Namespace

Clearhaus.Gateway.Transaction.Options

##### Summary

Optionals for Authorization transaction, .

<a name='F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-ip'></a>
### ip `constants`

##### Summary

IPv4/IPv6 address of cardholder initiating authorization

<a name='F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-recurring'></a>
### recurring `constants`

##### Summary

Mark authorization as recurring

<a name='F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-reference'></a>
### reference `constants`

##### Summary

Authorization reference

<a name='F-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-textOnStatement'></a>
### textOnStatement `constants`

##### Summary

Statement on cardholders bank transaction

<a name='M-Clearhaus-Gateway-Transaction-Options-AuthorizationOptions-GetParameters'></a>
### GetParameters() `method`

##### Summary

Returns the parameters with correct keys.

##### Parameters

This method has no parameters.

<a name='T-Clearhaus-Gateway-Transaction-Base'></a>
## Base `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Base class for a Clearhaus transaction result

<a name='P-Clearhaus-Gateway-Transaction-Base-id'></a>
### id `property`

##### Summary

UUID identifying the transaction

<a name='P-Clearhaus-Gateway-Transaction-Base-processedAt'></a>
### processedAt `property`

##### Summary

Datetime the transaction was processed

<a name='P-Clearhaus-Gateway-Transaction-Base-status'></a>
### status `property`

##### Summary

Status of the query

<a name='M-Clearhaus-Gateway-Transaction-Base-isSuccess'></a>
### isSuccess() `method`

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

<a name='T-Clearhaus-Gateway-Util-RestRequest'></a>
## RestRequest `type`

##### Namespace

Clearhaus.Gateway.Util

<a name='T-Clearhaus-Gateway-Util-RestRequestBuilder'></a>
## RestRequestBuilder `type`

##### Namespace

Clearhaus.Gateway.Util

##### Summary

Helper class for building rest request helper.

<a name='T-Clearhaus-Gateway-Transaction-Status'></a>
## Status `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Status of a query

<a name='P-Clearhaus-Gateway-Transaction-Status-code'></a>
### code `property`

##### Summary

See http://docs.gateway.clearhaus.com/#Transactionstatuscodes

<a name='P-Clearhaus-Gateway-Transaction-Status-message'></a>
### message `property`

##### Summary

Message associated with status code

<a name='T-Clearhaus-Gateway-Transaction-Void'></a>
## Void `type`

##### Namespace

Clearhaus.Gateway.Transaction

##### Summary

Represents a completed void transaction
