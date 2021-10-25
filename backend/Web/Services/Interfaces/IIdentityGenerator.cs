namespace Web.Services.Interfaces
{
    public interface IIdentityGenerator<out T>
    {
        T Generate();
    }
}