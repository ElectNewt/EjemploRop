# Railway Oriented programming Library
 
This code does references the post [Railway oriented programming](https://www.netmentor.es/entrada/railway-oriented-programming) based on Scott Wlaschin Idea of handling errors in a more functional way.


![NetMentor.ROP build result](https://github.com/ElectNewt/EjemploRop/actions/workflows/build.yml/badge.svg)


## Railway oriented programming en NUGET

| Pacakge in nuget                                                                                                     | .NET Standard | 
|----------------------------------------------------------------------------------------------------------------------|:-------------:|
| [Netmetnor.ROP](https://www.nuget.org/packages/Netmentor.ROP)                                                        | 2.0 | 
| [Netmetnor.ROP.ApiExtensions](https://www.nuget.org/packages/Netmentor.ROP.ApiExtensions/)                           | 2.0 | 
| [Netmetnor.ROP.ApiExtensions.Translations](https://www.nuget.org/packages/Netmentor.ROP.ApiExtensions.Translations/) | 2.0 | 


## Implement Railway Oriented programming into your application
You can find the `Result<T>` structure in Nexus, including the package `Netmentor.Rop"`.

The `Result` type is an immutable type that allows you to store success or errors, not both at the same time. It makes no sense to have errors where the result was a success or vice versa.

And `Result` is immutable because to mutate it, you have to use the framework built around it. For this reason, you have to use the extension methods available. If you feel like you need something else, please create a PR for it.
- Note 1: Remember that `void` is not a Type in C#, and we don't like non-typed stuff. For that reason, if in any method you feel that you don't want to return a type, you have to use the type called `Unit`, which is standard in the functional world.
- Note 2: We designed the framework to support both async and sync workflows. 
  
The `Result` structure contains three properties
1. `T` The value, populated if there are no errors.
2. `ImmutableArray<Error>` Array of errors, empty if none.
3. `Success` Boolean indicates if the result is in a success status (no errors).


### Return a `Result<T>`

To return a `Result<T>`, you only need to have your method return a `Result<T>` and a custom [implicit operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators) will do the rest.

So, for example, if you want to return an int, you will normally do something like:
````csharp
public int GetMyInt(){
  return 1;
}
````

Thanks to the implicit operator, you can do pretty much the same, change the return type
```csharp
public Result<int> GetMyInt()
{
  return 1;
}
```
Unfortunately, it is slightly different when you are returning `Task<Result<T>>`; If you have any `await` in the code, it is fine. but if you don't, you have to specify `Success().Async()` when returning the value:
```csharp
public Task<Result<int>> GetMyInt()
{
  return 1.Success().Async();
}
```
This is because we cannot have implicit task operators.

#### Use case
If we apply this logic into our use case, we can have the validation method looking like this:
````csharp
private Result<bool> ValidateNewAccount(Account account)
{
  return true;
}
````

### Return an error
We just saw how to return something that works, but what if something fails? We have to return an error.
For this, we will use `Result.Failure<T>("error")`.
Modify the method to see the example:
````csharp
private Result<Account> ValidateNewAccount(Account account)
{
  if(string.IsNullOrWhitespace(account.FirstName)
    return Result.Failure<Account>("first name must be populated")
    
  if(string.IsNullOrWhitespace(account.LastName)
    return Result.Failure<Account>("Last name must be populated")
    
  return account;
}
````
As you can see, you check every one of them and, if any fails, returns a "failure", which, if you remember our previous images, will move the flow to the "non-happy" path.
- Note: if you need to return all the errors, you can make a `List<Errors>` and a return like the next: `return Result.Failure<bool>(listOferrors);`

### Calling the second method

We have just seen what our method look like, but how do we call and link the second one?

We have to understand the `Result` as a chain, which means that one response is linked to the following input.

To accomplish the "call" to the next method we use the extension method called `.Bind`:
``` csharp
public Result<bool> BasicAccountCreation(Account account)
{
   return ValidateNewAccount(account)
    .Bind(SaveUser)
    .Bind(SendCode);
}
```
Bind is using a [delegate](https://www.netmentor.es/Entrada/delegados-csharp) `Func<T, Result<U>>` to receive the method as a parameter.

This also means that you can modify the input. Just imagine that `ValidateNewAccount` returns a bool, but you don't require the bool because you need the user to save the user. in this case, you can use a lambda expression to discard the result and pass the account:
````csharp
return ValidateNewAccount(account)
    .Bind(_ => SaveUser(account))
````

Also, in our case, `SaveUser` will be `Async` as it is storing information in the database. For that reason, we must convert our chain to `Async`:
``` csharp
public async Task<Result<bool>> BasicAccountCreation(Account account)
{
   return await ValidateNewAccount(account)
    .Async() //<- this line here
    .Bind(SaveUser)
    .Bind(SendCode);
}
```

#### Implementing the second method
I mentioned before that the output of the first method is the input for the second. This is true, but not entirely, as our method returns `Result<T>` in our input. We don't want to use `Result`. We only need `T`.

And that is how it works. Our `SaveUser` method will be something like the following.
````csharp
private async Task<Result<string>> SaveUser(Account account)
{
  return await _dependency.SaveUser(account)
    .Map(_=>account.email);
}
//This will be the implementation of the third method.
private async Task<Result<bool>> SendCode(string email)
{
    return await _dependencies.SendEmail(email);
}
````
Everything related to `Result` is managed by the library automatically.

#### How Bind works internally
The following example is only for Bind, but all the extension methods have the same pattern.
````csharp
public static Result<U> Bind<T, U>(this Result<T> r, Func<T, Result<U>> method)
{
    try
    {
        return r.Success
            ? method(r.Value)
            : Result.Failure<U>(r.Errors, r.HttpStatusCode);
    }
    catch (Exception e)
    {
        ExceptionDispatchInfo.Capture(e).Throw();
        throw;
    }
}
````
As you can see is an extension method of `Result<T>` which checks if it is a success;
- If it is a success, execute the method
- If it is not a success, it returns the failure errors. (sidenote: we will explain the "StatusCode" later). 
  
Also, don't forget that you can have chains inside chains.
  
### Usage of the Library

As mention, I designed the library to cover all the use cases I use it for, but it could be more, do not heasitate and create a PR!

These are the extension methods you can use in your chain.

#### Bind
Allows you to link two methods. Example:
````csharp
return ValidateNewAccount(account)
  .Bind(SaveUser)
  .Bind(SendCode);
````
#### Fallback
The `Fallback` method is executed when the previous method has a failure response. 

Example:
````csharp
return ValidateNewAccount(account)
  .Bind(SaveUser) //<- Assuming this fails
  .Fallback(SaveInAnotherDatabase) //<- then this is executed if it didnt failed, it does not get executed.
  .Bind(SendCode);
````

#### Combine

Allows you to combine two different responses
````csharp
return ValidateNewAccount(account)
  .Bind(SaveUser) 
  .Combine(SaveInAnotherDatabase) 
  .Bind(AnotherMethod);
````
In this case, `AnotherMethod` will receive the response of both previous methods in a tuple.

#### Ignore
Sometimes, the use case will contain certain logic, this logic may not affect the rest of the use case. If that is the case, you can use `.Ignore()` and it will convert your `Result<T>` into `Result<Unit>`.
````csharp
Result<Unit> result = ValidateNewAccount(account)
  .Bind(SaveUser) 
  .Ignore();
````

#### Map
Allow us to specify a method which is not returning a `Result<T>` into the chain. A mapper will be the most common usecase.
````csharp
Result<NetMentorAccount> result = ValidateNewAccount(account)
  .Bind(SaveUser) 
  .Map(_dependency.MapToNetmentor); //<- being this something like IMapper<Account, NetMentorAccount>
````

#### Then
Instead of using a delegate `Func<T, Result<U>>` in this case uses an `Action<T>` which means that the result of this then will be ignored.
````csharp
Result<Unit> result = ValidateNewAccount(account)
  .Bind(SaveUser) 
  .Then(TriggerExternalService)
  .Bind(SendCode);
````
The use case will be similar to a fire&Forget.

#### Throw
It will return to you the actual value of `T` if it is in a success status, but if is not success, it will throw a  `ErrorResultException` with the errors as message on the exception.
````csharp
NetMentorAccount result = ValidateNewAccount(account)
  .Bind(SaveUser) 
  .Map(_dependency.MapToNetmentor);
  .Throw()
````
You will not need to call this in 99.9% of the use case.

#### Traverse
Converts a list of `IEnumerable<Result<T>>` into `Rsult<List<T>>`
`````csharp
Result<List<NetMentorAccount>> result = GetUsersByIds(arrayIds) //<- assuing GetUsersByIds returns List<Result<T>>
  .Traverse();
`````

#### UseSuccessHttpStatusCode
In most of our use cases, we return the information using an API, and it is not always the same status code, so for that reason, we allow to change this status code when the chain is a success.
````csharp
Result<ValidationResult> result = ValidateNewAccount(account)
  .UseSuccessHttpStatusCode(HttpStatusCode.OK);
````
If you don't specify any status code, it will be `HttpStatusCode.Accepted` (202).

But as mentioned, this only matters if you are using API.

### The Error type

For the errors, we also created a type. This type contains three properties.
- `ErorMessage` allows you to introduce a message for the errors.
- `ErrorCode` Code set by you in your application which references the error. 
  
To create the errors you have to use the factory method: `Error  Create(string message, Guid? errorCode = null)`.
  
But when you do `Result.Failure<T>("errorMessage")` it creates the error automatically.

#### Error status codes
As there is a possibility with `UseSuccessHttpStatusCode` of setting up a success status code, there is also a way to set up a failure status code.

If you remember, when we create an error, we do it with `Result.Failure<T>(Error)` this generates a status code of  `HttpStatusCode.UnprocessableEntity` (422)

But what if you want another error code?

At the moment we support 3 additional Error codes
- `HttpStatusCode.BadRequest` (400)
`````csharp 
Result.BadRequest<T>(Error)
`````
- `HttpStatusCode.NotFound` (404)
`````csharp 
Result.NotFound<T>(Error)
`````
- `HttpStatusCode.Conflict` (409)
`````csharp 
Result.Conflict<T>(Error)
`````

### Response from an API
As we mentioned, most of our use cases are API calls, which means we have to return this information from an API.

To achieve this goal first of all you have to install the package `Common.API` from nexus.

You have to call the extension method `ToActionResult`, which will automatically translate the struct into the `IActionResult`.
````chsarp
public async Task<IActionResult> CreateNewAccount(Account account)
{
  return await _useCase.Execute(account)
    .ToActionResult();
}
````
If `Result` is a success, it will return the status code set with `UseSuccessHttpStatusCode` (or the default 422 if not set), and if the `Result` is not successful, it will return the one set when you specified the error.

#### ResultDto
During the execution of `.ToActionResult()` result gets converted into a `Data Transfer Object` (Dto) for that reason it returns a `ResultDto`.
- Note: We mainly did it due to serialisation issues with immutable classes in C#.


## Error Translations
The library can be extended with `ROP.ApiExtensions.Translations` to provide an out of the box functionality to translate errors at serialization time.

You need to create a resource per language you want to support and an empty class with the same name.
Then If there is an error to be returned in your API it will translate it automatically **IF** the message was left blank.

For example If you Create an error like the following:  
````csharp
return Result.Failure(Guid.Parse("ce6887fb-f8fa-49b7-bcb4-d8538b6c9932")).ToActionResult()
````
It will look for that guid in the translation file and show the message field as this exampole:

````json
{
    "value": null,
    "errors": [
        {
            "ErrorCode": "ce6887fb-f8fa-49b7-bcb4-d8538b6c9932",
            "Message": "Example message"
        }
    ],
    "success": false
}
````

### Error Translations with variables
The messages in the translations support variables on the messages using standard C# string format.

For example a message like `example variable1: {0}, variable2: {2}` will replace `{x}` for the variables you assign on the error creation.

To accomplish the expected result, all the failure creation messages that support the `ErrorCode` have an optional parameter that accpets an array of string (`string[]`) 
which should match the number of variables in the string message, example:

````csharp
Result.NotFound("Guid-NOT-found", new []{"identifier1"}); // "The id {0} cannot be found"
-> "The id identifier1 cannot be found"
````


Note: If the `TranslationVariables` is empty the library will not try to format the string message.



### Add the custom serializer.

You can add it manually with the next command: 
````csharp
services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.Converters.Add(new ErrorDtoSerializer<TranslationFile>(httpContextAccessor)));
````
But alternatively you can use the following one:
````csharp
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.AddTranslation<TraduccionErrores>(services);
} );
````


You can find more information in [My blog *in spanish*](https://www.netmentor.es/entrada/custom-json-serializer).

## Tuples
[Tuples](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples) in C# are a powerful type that allows you to return more than one value in a single response.

They are very useful for us when developing using ROP. because It allows us to query a value and propagate the input with the new value. for example in the next scenario:
````csharp
Result<Unit> result = ValidateNewAccount(account)
  .Bind(SaveUser)
  .Bind(GetManagerEmail)  //<- new method
  .Bind(SendCode);
````
Here, we see a new method called `GetManagerEmail` which basically will check the user ManagerID and get its email; this could be useful to specify in the `sendCode` an email to contact if something does not work.

The implementation of `GetManagerEmail` will look like the next.
`````csharp
private async Task<Result<(string, string)>> GetManagerEmail(Account account){
  string managerEmail = await _dependencies.GetManagerEmail(account.ManagerId);
  return (account.Email, managerEmail)
}
`````
As you can see, we are returning a tuple into the method, which converts automatically into `Result<T>` being `T` the tuple `(Account, string)`.

Then in the following method, you have to receive the tuple. Tuples are a type, not two different types, so that they will be together. There is nothing mandatory here, but normally is easy to identify if you call them in the parameters as `args`
`````csharp
//This will be the implementation of the third method.
private async Task<Result<bool>> SendCode((string, string) args)
{
    return await _dependencies.SendEmailWithManagerReference(args.Item1, args.Item2);
}
`````
You also have the option to name the tuples `SendCode((string UserEmail, string ManagerEmail) args)`, and then you will access with `args.UserEmail` and `args.ManagerEmail`.



## Issues and contributing

Please do not hesitate in adding some issue or contribute in the code.

