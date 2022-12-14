using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Services
{
    /// <summary>
    /// Classe responsável pela regra de negócio referente aos registros de navegação das vendas.
    /// </summary>
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método responsável pela pesquisa de registro de venda por data.
        /// </summary>
        /// <param name="minDate">Define uma data mínima para a pesquisa.</param>
        /// <param name="maxDate">Define uma data maxima para a pesquisa.</param>
        /// <returns>Registro da uma venda de acordo com a data especificada.</returns>
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate) 
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue) 
            {
                result = result.Where(x => x.Date >= minDate);
            }
            if (maxDate.HasValue) 
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Método responsável pela pesquisa de grupo de registro de vendas por data.
        /// </summary>
        /// <param name="minDate">Define uma data mínima para a pesquisa.</param>
        /// <param name="maxDate">Define uma data maxima para a pesquisa.</param>
        /// <returns>Registro da um grupo de vendas de acordo com a data especificada.</returns>
        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
