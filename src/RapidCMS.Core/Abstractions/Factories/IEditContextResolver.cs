using System;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Forms;

namespace RapidCMS.Core.Abstractions.Factories
{
    internal interface IEditContextFactory
    {
        IEditContext<IEntity> GetEditContextWrapper(EditContext editContext);
    }
}
