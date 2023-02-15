using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service
{
    public interface IBaseService<TModel> where TModel : class
    {
        Task<List<TModel>?> GetListAsync(Func<TModel,bool>? expression = null,string? path = null, Dictionary<string, string>? param = null, string? token = null);
        Task<TModel?> GetModelAsync(Func<TModel,bool>? expression = null, string? path = null, Dictionary<string, string>? param = null, string? token = null);
        Task<bool> Update(TModel model, string? path = null, Dictionary<string, string>? param = null, string? token = null);
        Task<bool> Delete(TModel model, string? path = null, Dictionary<string, string>? param = null, string? token = null);
        Task<bool> Add(TModel model, string? path = null, string? token = null);
    }
}
