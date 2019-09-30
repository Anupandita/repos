namespace Sfc.Wms.App.Api.Contracts.Enums
{
    public enum ResultTypes
    {
        NotSet = 0,
        Ok = 200,
        Created = 201,
        OkNoContent = 204,
        OkResetContent = 205,
        BadRequest = 400,
        UnathorizedRequest = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        InternalServerError = 500,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
    }
}