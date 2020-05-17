using System;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Factories;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Forms;

namespace RapidCMS.Core.Factories
{
    internal class EditContextWrapperFactory : IEditContextFactory
    {
        public IEditContext<IEntity> GetEditContextWrapper(EditContext editContext)
        {
            var contextType = typeof(EditContextWrapper<>).MakeGenericType(editContext.Entity.GetType());
            return (IEditContext<IEntity>)Activator.CreateInstance(contextType, editContext);
        }
    }
}
