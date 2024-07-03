using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Domain.Entities;

namespace Track.Order.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<string> AgregarCategoria(AgregarCategoriaRequest categoria);

        Task<List<CategoriaGasto>> GetCategoriesAsync();
    }
}
