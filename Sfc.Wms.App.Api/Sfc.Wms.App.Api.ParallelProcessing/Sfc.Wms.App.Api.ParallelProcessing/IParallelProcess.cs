using SimpleInjector;

namespace Sfc.Wms.App.Api.ParallelProcessing
{
    public interface IParallelProcess
    {
        int WakeUp(Container container);
    }
}