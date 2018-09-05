<a name='assembly'></a>
# MPI

## Contents

- [CheckResponse](#T-Clearhaus-MPI-Representers-CheckResponse 'Clearhaus.MPI.Representers.CheckResponse')
  - [amount](#F-Clearhaus-MPI-Representers-CheckResponse-amount 'Clearhaus.MPI.Representers.CheckResponse.amount')
  - [cavv](#F-Clearhaus-MPI-Representers-CheckResponse-cavv 'Clearhaus.MPI.Representers.CheckResponse.cavv')
  - [cavvAlgorithm](#F-Clearhaus-MPI-Representers-CheckResponse-cavvAlgorithm 'Clearhaus.MPI.Representers.CheckResponse.cavvAlgorithm')
  - [currency](#F-Clearhaus-MPI-Representers-CheckResponse-currency 'Clearhaus.MPI.Representers.CheckResponse.currency')
  - [eci](#F-Clearhaus-MPI-Representers-CheckResponse-eci 'Clearhaus.MPI.Representers.CheckResponse.eci')
  - [last4](#F-Clearhaus-MPI-Representers-CheckResponse-last4 'Clearhaus.MPI.Representers.CheckResponse.last4')
  - [merchantID](#F-Clearhaus-MPI-Representers-CheckResponse-merchantID 'Clearhaus.MPI.Representers.CheckResponse.merchantID')
  - [status](#F-Clearhaus-MPI-Representers-CheckResponse-status 'Clearhaus.MPI.Representers.CheckResponse.status')
  - [xid](#F-Clearhaus-MPI-Representers-CheckResponse-xid 'Clearhaus.MPI.Representers.CheckResponse.xid')
- [Constants](#T-Clearhaus-MPI-Constants 'Clearhaus.MPI.Constants')
  - [MPITestURL](#F-Clearhaus-MPI-Constants-MPITestURL 'Clearhaus.MPI.Constants.MPITestURL')
  - [MPIURL](#F-Clearhaus-MPI-Constants-MPIURL 'Clearhaus.MPI.Constants.MPIURL')
- [EnrollCheckBuilder](#T-Clearhaus-MPI-Builder-EnrollCheckBuilder 'Clearhaus.MPI.Builder.EnrollCheckBuilder')
  - [amount](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-amount 'Clearhaus.MPI.Builder.EnrollCheckBuilder.amount')
  - [cardExpireMonth](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardExpireMonth 'Clearhaus.MPI.Builder.EnrollCheckBuilder.cardExpireMonth')
  - [cardExpireYear](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardExpireYear 'Clearhaus.MPI.Builder.EnrollCheckBuilder.cardExpireYear')
  - [cardholderIP](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardholderIP 'Clearhaus.MPI.Builder.EnrollCheckBuilder.cardholderIP')
  - [cardNumber](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardNumber 'Clearhaus.MPI.Builder.EnrollCheckBuilder.cardNumber')
  - [currency](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-currency 'Clearhaus.MPI.Builder.EnrollCheckBuilder.currency')
  - [merchantAcquirerBin](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantAcquirerBin 'Clearhaus.MPI.Builder.EnrollCheckBuilder.merchantAcquirerBin')
  - [merchantCountry](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantCountry 'Clearhaus.MPI.Builder.EnrollCheckBuilder.merchantCountry')
  - [merchantID](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantID 'Clearhaus.MPI.Builder.EnrollCheckBuilder.merchantID')
  - [merchantName](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantName 'Clearhaus.MPI.Builder.EnrollCheckBuilder.merchantName')
  - [merchantUrl](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantUrl 'Clearhaus.MPI.Builder.EnrollCheckBuilder.merchantUrl')
  - [orderID](#F-Clearhaus-MPI-Builder-EnrollCheckBuilder-orderID 'Clearhaus.MPI.Builder.EnrollCheckBuilder.orderID')
- [EnrollmentStatus](#T-Clearhaus-MPI-Representers-EnrollmentStatus 'Clearhaus.MPI.Representers.EnrollmentStatus')
  - [acsUrl](#F-Clearhaus-MPI-Representers-EnrollmentStatus-acsUrl 'Clearhaus.MPI.Representers.EnrollmentStatus.acsUrl')
  - [eci](#F-Clearhaus-MPI-Representers-EnrollmentStatus-eci 'Clearhaus.MPI.Representers.EnrollmentStatus.eci')
  - [enrolled](#F-Clearhaus-MPI-Representers-EnrollmentStatus-enrolled 'Clearhaus.MPI.Representers.EnrollmentStatus.enrolled')
  - [error](#F-Clearhaus-MPI-Representers-EnrollmentStatus-error 'Clearhaus.MPI.Representers.EnrollmentStatus.error')
  - [pareq](#F-Clearhaus-MPI-Representers-EnrollmentStatus-pareq 'Clearhaus.MPI.Representers.EnrollmentStatus.pareq')
- [Error](#T-Clearhaus-MPI-Representers-Error 'Clearhaus.MPI.Representers.Error')
  - [detail](#F-Clearhaus-MPI-Representers-Error-detail 'Clearhaus.MPI.Representers.Error.detail')
  - [message](#F-Clearhaus-MPI-Representers-Error-message 'Clearhaus.MPI.Representers.Error.message')
- [MPI](#T-Clearhaus-MPI-MPI 'Clearhaus.MPI.MPI')
  - [#ctor(apiKey)](#M-Clearhaus-MPI-MPI-#ctor-System-String- 'Clearhaus.MPI.MPI.#ctor(System.String)')
  - [#ctor(apiKey,mpiUrl)](#M-Clearhaus-MPI-MPI-#ctor-System-String,System-String- 'Clearhaus.MPI.MPI.#ctor(System.String,System.String)')
  - [timeout](#F-Clearhaus-MPI-MPI-timeout 'Clearhaus.MPI.MPI.timeout')
  - [CheckPARes(pares)](#M-Clearhaus-MPI-MPI-CheckPARes-System-String- 'Clearhaus.MPI.MPI.CheckPARes(System.String)')
  - [CheckPAResAsync(pares)](#M-Clearhaus-MPI-MPI-CheckPAResAsync-System-String- 'Clearhaus.MPI.MPI.CheckPAResAsync(System.String)')
  - [Dispose()](#M-Clearhaus-MPI-MPI-Dispose 'Clearhaus.MPI.MPI.Dispose')
  - [Dispose()](#M-Clearhaus-MPI-MPI-Dispose-System-Boolean- 'Clearhaus.MPI.MPI.Dispose(System.Boolean)')
  - [EnrollCheck(builder)](#M-Clearhaus-MPI-MPI-EnrollCheck-Clearhaus-MPI-Builder-EnrollCheckBuilder- 'Clearhaus.MPI.MPI.EnrollCheck(Clearhaus.MPI.Builder.EnrollCheckBuilder)')
  - [EnrollCheckAsync(builder)](#M-Clearhaus-MPI-MPI-EnrollCheckAsync-Clearhaus-MPI-Builder-EnrollCheckBuilder- 'Clearhaus.MPI.MPI.EnrollCheckAsync(Clearhaus.MPI.Builder.EnrollCheckBuilder)')
  - [Finalize()](#M-Clearhaus-MPI-MPI-Finalize 'Clearhaus.MPI.MPI.Finalize')
  - [SetEndpoint(mpiUrl)](#M-Clearhaus-MPI-MPI-SetEndpoint-System-String- 'Clearhaus.MPI.MPI.SetEndpoint(System.String)')

<a name='T-Clearhaus-MPI-Representers-CheckResponse'></a>
## CheckResponse `type`

##### Namespace

Clearhaus.MPI.Representers

##### Summary

Represents a response of a check call

<a name='F-Clearhaus-MPI-Representers-CheckResponse-amount'></a>
### amount `constants`

##### Summary

amount on transaction

<a name='F-Clearhaus-MPI-Representers-CheckResponse-cavv'></a>
### cavv `constants`

##### Summary

CAVV (Cardholder Authentication Verification Value)

<a name='F-Clearhaus-MPI-Representers-CheckResponse-cavvAlgorithm'></a>
### cavvAlgorithm `constants`

##### Summary

The algorithm used for the CAVV algorithm

<a name='F-Clearhaus-MPI-Representers-CheckResponse-currency'></a>
### currency `constants`

##### Summary

currency of the transaction

<a name='F-Clearhaus-MPI-Representers-CheckResponse-eci'></a>
### eci `constants`

##### Summary

Electronic Commerce Indicator containing the result

<a name='F-Clearhaus-MPI-Representers-CheckResponse-last4'></a>
### last4 `constants`

##### Summary

Last 4 digits of PAN

<a name='F-Clearhaus-MPI-Representers-CheckResponse-merchantID'></a>
### merchantID `constants`

##### Summary

Merchant ID of associated merchant

<a name='F-Clearhaus-MPI-Representers-CheckResponse-status'></a>
### status `constants`

##### Summary

Status of the `PARes`

##### Remarks

Corresponds to the `TX.Status`field in the `PARes`XML

<a name='F-Clearhaus-MPI-Representers-CheckResponse-xid'></a>
### xid `constants`

##### Summary

Merchant transaction ID

<a name='T-Clearhaus-MPI-Constants'></a>
## Constants `type`

##### Namespace

Clearhaus.MPI

##### Summary

Namespace for constants

<a name='F-Clearhaus-MPI-Constants-MPITestURL'></a>
### MPITestURL `constants`

##### Summary

In theory a testing endpoint for the clearhaus MPI service, it is
practically non-functional since DS test servers appear to be
mostly non-funcional.

<a name='F-Clearhaus-MPI-Constants-MPIURL'></a>
### MPIURL `constants`

##### Summary

HTTP Endpoint for requests to the Clearhaus MPI service.

<a name='T-Clearhaus-MPI-Builder-EnrollCheckBuilder'></a>
## EnrollCheckBuilder `type`

##### Namespace

Clearhaus.MPI.Builder

##### Summary

Contains information for performing 3D-Secure flow

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-amount'></a>
### amount `constants`

##### Summary

Amount of currency in transaction

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardExpireMonth'></a>
### cardExpireMonth `constants`

##### Summary

Month of card expiry

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardExpireYear'></a>
### cardExpireYear `constants`

##### Summary

Year of card expiry

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardholderIP'></a>
### cardholderIP `constants`

##### Summary

IP of cardholder

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-cardNumber'></a>
### cardNumber `constants`

##### Summary

PAN

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-currency'></a>
### currency `constants`

##### Summary

Currency of transaction

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantAcquirerBin'></a>
### merchantAcquirerBin `constants`

##### Summary

Acquirer BIN for the card scheme

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantCountry'></a>
### merchantCountry `constants`

##### Summary

Country of merchant

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantID'></a>
### merchantID `constants`

##### Summary

ID of merchant

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantName'></a>
### merchantName `constants`

##### Summary

Merchant Name

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-merchantUrl'></a>
### merchantUrl `constants`

##### Summary

URL of merchant page

<a name='F-Clearhaus-MPI-Builder-EnrollCheckBuilder-orderID'></a>
### orderID `constants`

##### Summary

Unique transaction identifier

<a name='T-Clearhaus-MPI-Representers-EnrollmentStatus'></a>
## EnrollmentStatus `type`

##### Namespace

Clearhaus.MPI.Representers

##### Summary

Response of EnrollCheck request

<a name='F-Clearhaus-MPI-Representers-EnrollmentStatus-acsUrl'></a>
### acsUrl `constants`

##### Summary

URL of ACS

<a name='F-Clearhaus-MPI-Representers-EnrollmentStatus-eci'></a>
### eci `constants`

##### Summary

Status as ECI

<a name='F-Clearhaus-MPI-Representers-EnrollmentStatus-enrolled'></a>
### enrolled `constants`

##### Summary

Whether or not card is enrolled for 3D-Secure

<a name='F-Clearhaus-MPI-Representers-EnrollmentStatus-error'></a>
### error `constants`

##### Summary

Any errors

<a name='F-Clearhaus-MPI-Representers-EnrollmentStatus-pareq'></a>
### pareq `constants`

##### Summary

`PAReq`to forward to `acsUrl`

<a name='T-Clearhaus-MPI-Representers-Error'></a>
## Error `type`

##### Namespace

Clearhaus.MPI.Representers

##### Summary

Error class for  the MPI service

<a name='F-Clearhaus-MPI-Representers-Error-detail'></a>
### detail `constants`

##### Summary

No information

<a name='F-Clearhaus-MPI-Representers-Error-message'></a>
### message `constants`

##### Summary

Error message

<a name='T-Clearhaus-MPI-MPI'></a>
## MPI `type`

##### Namespace

Clearhaus.MPI

##### Summary

MPI is used adding 3D-Secure to a payment transaction flow. Is uses https://3dsecure.io as an MPI service.

##### Example

```C#
 using Clearhaus.MPI;
 using Clearhaus.MPI.Builder;
 public static void main()
 {
         string apiKey = "SOME UUID APIKEY";
         // Can be disposed by `#Dispose()` or will be GC'd automatically.
         var mpiAccount = new MPI(apiKey);
         var builder = new EnrollCheckBuilder {
             amount              = "100",
             currency            = "DKK",
             orderID             = "SOME ID",
             cardholderIP        = "1.1.1.1",
             cardNumber          = "SOME PAN",
             cardExpireMonth     = "04",
             cardExpireYear      = "2030",
             merchantAcquirerBin = "SOME BIN",
             merchantCountry     = "DK",
             merchantID          = "SOME ID",
             merchantName        = "MyMerchant",
             merchantUrl         = "http://mymerchant.com"
         };
         EnrollmentStatus response;
         try
         {
             response = mpiAccount.EnrollCheck(builder);
         }
         catch(ClrhsNetException e)
         {
             // Handle
         }
         catch(ClrhsGatewayException e)
         {
             // Something is wrong on server-side
         }
         catch(ClrhsAuthException e)
         {
             // Invalid APIKey. This should not happen if you have tested your
             // key.
         }
         catch(ClrhsException e)
         {
             // Last effort exception
         }
         if (response.enrolled == "Y")
         {
             // Continue 3D-Secure procedure
         }
 }
  
```

<a name='M-Clearhaus-MPI-MPI-#ctor-System-String-'></a>
### #ctor(apiKey) `constructor`

##### Summary

Temporary documentation

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| apiKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | UUID representing your 3dsecure.io account |

<a name='M-Clearhaus-MPI-MPI-#ctor-System-String,System-String-'></a>
### #ctor(apiKey,mpiUrl) `constructor`

##### Summary

Temporary documentation

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| apiKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | UUID representing your 3dsecure.io account |
| mpiUrl | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | URL to use as API mpiUrl |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | If mpiUrl is null |
| [System.UriFormatException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UriFormatException 'System.UriFormatException') | If mpiUrl is invalid URI |

<a name='F-Clearhaus-MPI-MPI-timeout'></a>
### timeout `constants`

##### Summary

The default timespan used for HttpClient, 40s)

<a name='M-Clearhaus-MPI-MPI-CheckPARes-System-String-'></a>
### CheckPARes(pares) `method`

##### Summary

Checks the `PARes`, returning results.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pares | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The `PARes`(possibly) returned from the EnrollCheck call. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-MPI-MPI-CheckPAResAsync-System-String-'></a>
### CheckPAResAsync(pares) `method`

##### Summary

Checks the `PARes`, returning results.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pares | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The `PARes`(possibly) returned from the EnrollCheck call. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-MPI-MPI-Dispose'></a>
### Dispose() `method`

##### Summary

IDisposable Interface

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-MPI-MPI-Dispose-System-Boolean-'></a>
### Dispose() `method`

##### Summary

IDisposable Interface

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-MPI-MPI-EnrollCheck-Clearhaus-MPI-Builder-EnrollCheckBuilder-'></a>
### EnrollCheck(builder) `method`

##### Summary

Query the MPI service, returning `PARes`and `ACSUrl`to allow continuing the 3DS flow.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| builder | [Clearhaus.MPI.Builder.EnrollCheckBuilder](#T-Clearhaus-MPI-Builder-EnrollCheckBuilder 'Clearhaus.MPI.Builder.EnrollCheckBuilder') | The information associated with the 3D-Secure flow |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-MPI-MPI-EnrollCheckAsync-Clearhaus-MPI-Builder-EnrollCheckBuilder-'></a>
### EnrollCheckAsync(builder) `method`

##### Summary

Query the MPI service, returning `PARes`and `ACSUrl`to allow continuing the 3DS flow.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| builder | [Clearhaus.MPI.Builder.EnrollCheckBuilder](#T-Clearhaus-MPI-Builder-EnrollCheckBuilder 'Clearhaus.MPI.Builder.EnrollCheckBuilder') | The information associated with the 3D-Secure flow |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [Clearhaus.ClrhsNetException](#T-Clearhaus-ClrhsNetException 'Clearhaus.ClrhsNetException') | Network error communicating with gateway |
| [Clearhaus.ClrhsAuthException](#T-Clearhaus-ClrhsAuthException 'Clearhaus.ClrhsAuthException') | Thrown if APIKey is invalid |
| [Clearhaus.ClrhsGatewayException](#T-Clearhaus-ClrhsGatewayException 'Clearhaus.ClrhsGatewayException') | Thrown if gateway responds with internal server error |
| [Clearhaus.ClrhsException](#T-Clearhaus-ClrhsException 'Clearhaus.ClrhsException') | Unexpected connection error |

<a name='M-Clearhaus-MPI-MPI-Finalize'></a>
### Finalize() `method`

##### Summary

Disposes unmanaged objects.

##### Parameters

This method has no parameters.

<a name='M-Clearhaus-MPI-MPI-SetEndpoint-System-String-'></a>
### SetEndpoint(mpiUrl) `method`

##### Summary

Override the default 3DSecure mpiUrl.

 [MPIURL](#F-Clearhaus-MPI-Constants-MPIURL 'Clearhaus.MPI.Constants.MPIURL')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| mpiUrl | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | URL to use as mpiUrl |
