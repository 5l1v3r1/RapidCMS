using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;

namespace RapidCMS.Repositories.ApiBridge.Models
{
    public class EditContextModel<TEntity>
        where TEntity : IEntity
    {
        public EditContextModel(IEditContext<TEntity> editContext)
        {
            Entity = editContext.Entity;
            ParentPath = editContext.Parent?.GetParentPath()?.ToPathString();
        }

        public TEntity Entity { get; set; }
        public string? ParentPath { get; set; }
    }
}
