using AutoMapper;
using Imetame.Documentacao.Domain.Exceptions;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository pedidoRepository, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        public async Task AtualizarAsync(Pedido model, CancellationToken cancellationToken)
        {
            if (model.Id == null)
            {
                throw new NotFoundException("O pedido não existe");
            }
            var entity = await _pedidoRepository.GetByIdAsync(model.Id.Value, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("O pedido não existe");
            }

            _mapper.Map(model, entity);


            await _pedidoRepository.SaveChangesAsync();
        }

        public async Task<Pedido> CriarAsync(Pedido model, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.Pedido>(model);

            #region validation
            if (await _pedidoRepository.JaExisteAsync(entity.Id, entity.NumPedido, cancellationToken))
            {
                throw new DomainException("Já existe um pedido com número informado");
            }
            #endregion
            entity.Id = Guid.NewGuid();
            _pedidoRepository.Add(entity);

            await _pedidoRepository.SaveChangesAsync();
            model.Id = entity.Id;
            return model;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _pedidoRepository.GetByIdAsync(id, cancellationToken);
            #region validation
            if (item == null)
            {
                throw new NotFoundException("O item não foi localizado pelo ID");
            }
            #endregion

            _pedidoRepository.Remove(item);

            await _pedidoRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginatedItemsViewModel<PedidoList>> ListarPaginadoAsync(string query, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            if (pageSize > 100)
            {
                throw new DomainException("Tamanho máximo da páxima é 100");
            }

            var skip = (pageIndex * pageSize);
            var take = pageSize;

            (long count, IEnumerable<Models.PedidoList> models) = await _pedidoRepository.ListaAsync(query, skip, take, cancellationToken);



            return new Models.PaginatedItemsViewModel<Models.PedidoList>(pageIndex, pageSize, count, models);
        }

        public async Task<Pedido> ObterPeloIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _pedidoRepository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("A Pedido não existe");
            }

            var model = _mapper.Map<Models.Pedido>(entity);
            return model;
        }


    }
}
