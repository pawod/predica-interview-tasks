using task_2.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb;
using System.Threading.Tasks;

namespace task_2.Contracts
{
    public interface IDeploymentService
    {
        void RunAsync(ArmTemplateParams tParams);
    }
}