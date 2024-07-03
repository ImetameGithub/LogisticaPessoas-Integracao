//using Dapper;
//using Imetame.Documentacao.Domain.Entities;
//using Imetame.Documentacao.Domain.Repositories;
//using Imetame.Documentacao.Infra.Data.Context;
//using Imetame.Documentacao.Infra.Data.Repositories.Common;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Imetame.Documentacao.Infra.Data.Repositories
//{
//    public class TarefaDapperRepository : DapperRepositoryBase<Domain.Entities.Tarefa, Guid>, ITarefaRepository
//    {

//        public TarefaDapperRepository(IConfiguration configuration) : base(configuration)
//        {

//        }


//        public async Task<bool> JaExisteAsync(Guid id, string nome, CancellationToken cancellationToken)
//        {
//            return (await conn.QueryAsync("select top 1 1 from Tarefa where id != @Id and nome = @Nome", new { Id = id, Nome = nome })).Any();
//        }

//        //public async Task<(long count, IEnumerable<Tarefa> entities)> ListaAsync(string searchTerm, Tarefa.StatusEnum? status, int skip, int take, CancellationToken cancellationToken)
//        //{
//        //    var sqlSelect = @"; SELECT * FROM Tarefa";
//        //    var sqlCount = @"SELECT COUNT(*) FROM Tarefa";
//        //    var where = string.Empty;

//        //    var filtros = new List<string>();
//        //    if (!string.IsNullOrEmpty(searchTerm))
//        //    {
//        //        filtros.Add("(Nome like @SearchTerm OR Descricao like @SearchTerm ");
//        //    };
//        //    if (status != null)
//        //    {
//        //        filtros.Add("[Status] = @StatusFiter");
//        //    }

//        //    if (filtros.Count() > 0)
//        //    {
//        //        where = " WHERE " + filtros.Aggregate((full, part) => full + " AND " + part);

//        //    }

//        //    var sql = sqlCount + where + sqlSelect + where + " ORDER BY Nome OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

//        //    //using (var multi = await this.conn.QueryMultipleAsync(sql, new { SearchTerm = $"%{searchTerm}%", Start = skip + 1, End = skip + take, statusFiter = status, filtroSolicitante = userName.ToUpper(), DataEmissao = doisAnos }))
//        //    using (var multi = await this.conn.QueryMultipleAsync(sql, new { SearchTerm = $"%{searchTerm}%", Skip = skip, Take = take, StatusFiter = status }))
//        //    {
//        //        var count = multi.Read<long>().Single();
//        //        var entities = multi.Read<Tarefa>();
//        //        return (count, entities);
//        //    }
//        //}

//        public async Task<(long count, IEnumerable<Tarefa> entities)> ListaAsync(string searchTerm, Tarefa.StatusEnum? status, int skip, int take, CancellationToken cancellationToken)
//        {
//            var sqlSelect = @" SELECT *,ROW_NUMBER() OVER (ORDER BY nome) AS RowNumber FROM Tarefa";
//            var sqlCount = @"SELECT COUNT(*) FROM Tarefa";
//            var where = string.Empty;

//            var filtros = new List<string>();
//            if (!string.IsNullOrEmpty(searchTerm))
//            {
//                filtros.Add("(Nome like @SearchTerm OR Descricao like @SearchTerm )");
//            };
//            if (status != null)
//            {
//                filtros.Add("[Status] = @StatusFiter");
//            }

//            if (filtros.Count() > 0)
//            {
//                where = " WHERE " + filtros.Aggregate((full, part) => full + " AND " + part);

//            }


//            var sql = sqlCount + where + @$";WITH OS AS ({sqlSelect + where})
//            SELECT*
//            FROM OS
//            WHERE RowNumber >= @Start AND RowNumber <= @End; ";



//            //using (var multi = await this.conn.QueryMultipleAsync(sql, new { SearchTerm = $"%{searchTerm}%", Start = skip + 1, End = skip + take, statusFiter = status, filtroSolicitante = userName.ToUpper(), DataEmissao = doisAnos }))
//            using (var multi = await this.conn.QueryMultipleAsync(sql, new { SearchTerm = $"%{searchTerm}%", Start = skip + 1, End = skip + take, StatusFiter = status }))
//            {
//                var count = multi.Read<long>().Single();
//                var entities = multi.Read<Tarefa>();
//                return (count, entities);
//            }
//        }

//        public Task<IEnumerable<Tarefa>> ListaAsync(string searchTerm, Tarefa.StatusEnum? status, CancellationToken cancellationToken)
//        {
//            var sqlSelect = @"SELECT * FROM Tarefa";

//            var where = string.Empty;

//            var filtros = new List<string>();
//            if (!string.IsNullOrEmpty(searchTerm))
//            {
//                filtros.Add("(Nome like @SearchTerm OR Descricao like @SearchTerm )");
//            };
//            if (status != null)
//            {
//                filtros.Add("[Status] = @StatusFiter");
//            }

//            if (filtros.Count() > 0)
//            {
//                where = " WHERE " + filtros.Aggregate((full, part) => full + " AND " + part);

//            }

//            var sql = sqlSelect + where + " ORDER BY Nome ";

//            return (conn.QueryAsync<Tarefa>(sql, new { SearchTerm = $"%{searchTerm}%", StatusFiter = status }));
//        }
//    }
//}
