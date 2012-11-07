using System.Web.Mvc;
using NHibernate;

namespace Pwipper.Filters
{
    public class TransactionAction : ActionFilterAttribute
    {
        private ITransaction transaction;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            transaction = DependencyResolver.Current.GetService<ISession>().BeginTransaction();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (transaction == null)
                return;

            if (transaction.IsActive && filterContext.Exception == null)
                transaction.Commit();

            transaction.Dispose();
        }
    }
}
