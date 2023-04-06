# Inter-Block Communications

You can call any block in another blocks with "ArfBlocksCommunicator" component. It is important for DRY.

Example:
```c#
// Trainee
var traineeRequestModel = new Trainees.Commands.Login.RequestModel()
{
	Email = requestPayload.Email,
	Password = requestPayload.Password,
};

var traineeLoginResponse = await _communicator.CommunicateDirect<Trainees.Commands.Login.Handler>(traineeRequestModel);

if (!traineeLoginResponse.HasError)
{
	var createTraineeResponsePayload = (Trainees.Commands.Login.ResponseModel)traineeLoginResponse.Payload;
	return ArfBlocksResults.Success(createTraineeResponsePayload);
}
```