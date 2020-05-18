using Newtonsoft.Json;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Models.Data;
using static RapidCMS.Core.Models.Data.ParentPath;

namespace RapidCMS.Repositories.ApiBridge.Models
{
    public class EditContextModel<TEntity>
        where TEntity : IEntity
    {
        public EditContextModel() { }

        public EditContextModel(IEditContext<TEntity> editContext)
        {
            Entity = editContext.Entity;
            ParentPath = editContext.Parent?.GetParentPath()?.ToPathString();
        }

        public TEntity Entity { get; set; }
        public string? ParentPath { get; set; }

        [JsonIgnore]
        public ParentPath? Parent
        {
            get
            {
                return TryParse(ParentPath);
            }
        }
    }
}
