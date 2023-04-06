# Authorization Support

When using "UseArfBlocksRequestHandlers" Middlware, JWT Authorization can be use. Also you can select default Authorization Policy.

Authorization Policies:
- AssumeAllAllowAnonymous
- AssumeAllAuthorized

You can mark any handler with theese tags. Thoose will work with Authorization Policy.
- [InternalHandler]
- [AuthorizedHandler]
- [AllowAnonymousHandler]

Example:
```c#
app.UseArfBlocksRequestHandlers((Action<UseRequestHandlersOptions>)(options =>
{
	options.AuthorizationType = UseRequestHandlersOptions.AuthorizationTypes.Jwt;
	options.AuthorizationPolicy = UseRequestHandlersOptions.AuthorizationPolicies.AssumeAllAuthorized;
	options.JwtAuthorizationOptions = new UseRequestHandlersOptions.JwtAuthorizationOptionsModel()
	{
		Audience = JwtService.Audience,
		Secret = JwtService.Secret,
	};
}));
```
