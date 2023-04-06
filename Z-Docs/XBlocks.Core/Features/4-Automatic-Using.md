# Automatic Block Using to Handler with "UseArfBlocksRequestHandlers" Middleware

When using "UseArfBlocksRequestHandlers" Middleware, if request path is "..../Users/All", 
The selected handler will be "Application.Users.Command.All" or "Application.Users.Queries.All"

Example:
```c#
app.UseArfBlocksRequestHandlers((Action<UseRequestHandlersOptions>)(options => {}));
```
