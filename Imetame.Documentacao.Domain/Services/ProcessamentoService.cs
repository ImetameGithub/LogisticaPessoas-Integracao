using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Imetame.Documentacao.Domain.Core.Enum;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Exceptions;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public class ProcessamentoService : IProcessamentoService
    {
        private readonly IProcessamentoRepository _processamentoRepository;
        private readonly ILogProcessamentoRepository _logprocessamentoRepository;
        private readonly IResultadoCadastroRepository _resultadoCadastroRepository;
        private readonly IMapper _mapper;

        public ProcessamentoService(IProcessamentoRepository processamentoRepository,
            ILogProcessamentoRepository logprocessamentoRepository,
            IResultadoCadastroRepository resultadoCadastroRepository,
            IMapper mapper)
        {
            this._processamentoRepository = processamentoRepository;
            this._logprocessamentoRepository = logprocessamentoRepository;
            this._resultadoCadastroRepository = resultadoCadastroRepository;
            this._mapper = mapper;
        }
        public async Task AtualizarAsync(Models.Processamento model, CancellationToken cancellationToken)
        {
            if (model.Id == null)
            {
                throw new NotFoundException("O pedido não existe");
            }
            var entity = await _processamentoRepository.GetByIdAsync(model.Id.Value, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("O pedido não existe");
            }

            _mapper.Map(model, entity);


            await _processamentoRepository.SaveChangesAsync();
        }

        public async Task AtualizarStatusAsync(Guid id, StatusProcessamento statusProcessamento, CancellationToken cancellationToken)
        {

            var entity = await _processamentoRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("O pedido não existe");
            }

            entity.Status = statusProcessamento;
            if (statusProcessamento == StatusProcessamento.Finalizado || statusProcessamento == StatusProcessamento.FinalizadoComErro)
                entity.FinalProcessamento = DateTime.Now;

            await _processamentoRepository.SaveChangesAsync();
        }

        public async Task<Models.Processamento> CriarAsync(Models.Processamento model, CancellationToken cancellationToken)
        {

            var processamento = _mapper.Map<Entities.Processamento>(model);
            processamento.Status = StatusProcessamento.Iniciado;
            processamento.InicioProcessamento=DateTime.Now; 
            _processamentoRepository.Add(processamento);
            await _processamentoRepository.SaveChangesAsync();
            return _mapper.Map<Models.Processamento>(processamento);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Models.PaginatedItemsViewModel<Models.LogProcessamento>> GetLogs(Guid id, DateTime? ultimoLog, int skip, int take, CancellationToken cancellationToken)
        {
            (long count, IEnumerable<Models.LogProcessamento> models) = await _logprocessamentoRepository.ListaAsync(id, ultimoLog, skip, take, cancellationToken);



            return new Models.PaginatedItemsViewModel<Models.LogProcessamento>(skip, take, count, models);

        }

        public async Task<Models.Processamento> GetProcessamentoAtivo(Guid idPedido, CancellationToken cancellationToken)
        {
            var processoAtivo = await _processamentoRepository.GetProcessamentoAtivo(idPedido);
            if (processoAtivo == null)
                return null;


            var ultimoLog = await _logprocessamentoRepository.GetUltimoLog(processoAtivo.Id, cancellationToken);
            if (ultimoLog != null && DateTime.Now.Subtract(ultimoLog.DataEvento).TotalMinutes > 30)
            {
                await AtualizarStatusAsync(processoAtivo.Id, StatusProcessamento.FinalizadoComErro, cancellationToken);

                _logprocessamentoRepository.Add(new Entities.LogProcessamento()
                {
                    Evento = $"Encerramento conpulsorio do processo. {DateTime.Now.Subtract(ultimoLog.DataEvento).TotalMinutes} minutos de inatividade.",
                    IdProcessamento = processoAtivo.Id,
                });

                _processamentoRepository.Update(processoAtivo);
                await _processamentoRepository.SaveChangesAsync();


                return null;
            }


            return _mapper.Map<Models.Processamento>(processoAtivo);
        }

        public async Task<Models.PaginatedItemsViewModel<Models.ResultadoCadastro>> GetResultados(Guid id, int skip, int take, CancellationToken cancellationToken) { 
            (long count, IEnumerable<Models.ResultadoCadastro> models) = await _resultadoCadastroRepository.ListaAsync(id, skip, take, cancellationToken);

            return new Models.PaginatedItemsViewModel<Models.ResultadoCadastro>(skip, take, count, models);
        }

        public async Task<Models.PaginatedItemsViewModel<Models.ProcessamentoList>> ListarPaginadoAsync(string query, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            if (pageSize > 100)
            {
                throw new DomainException("Tamanho máximo da páxima é 100");
            }

            var skip = (pageIndex * pageSize);
            var take = pageSize;

            (long count, IEnumerable<Models.ProcessamentoList> models) = await _processamentoRepository.ListaAsync(query, skip, take, cancellationToken);



            return new Models.PaginatedItemsViewModel<Models.ProcessamentoList>(pageIndex, pageSize, count, models);
        }

        public async Task<Models.Processamento> ObterPeloIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _processamentoRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("O Processamento não existe");
            }

            var model = _mapper.Map<Models.Processamento>(entity);
            return model;
        }
    }
}
