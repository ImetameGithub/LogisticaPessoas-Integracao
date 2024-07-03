using AutoMapper;
using Imetame.Documentacao.Domain.Exceptions;
using Imetame.Documentacao.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public class CredenciadoraDeParaService : ICredenciadoraDeParaService
    {
        public readonly ICredenciadoraDeParaRepository _repository;

        private readonly IMapper _mapper;

        public CredenciadoraDeParaService(ICredenciadoraDeParaRepository repository, IMapper mapper)
        {
            _repository = repository;

            _mapper = mapper;
        }


        public async Task<Models.PaginatedItemsViewModel<Models.CredenciadoraDeParaList>> ListarPaginadoAsync(string query, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            if (pageSize > 100)
            {
                throw new DomainException("Tamanho máximo da páxima é 100");
            }

            var skip = (pageIndex * pageSize);
            var take = pageSize;

            (long count, IEnumerable<Models.CredenciadoraDeParaList> models) = await _repository.ListaAsync(query, skip, take, cancellationToken);



            return new Models.PaginatedItemsViewModel<Models.CredenciadoraDeParaList>(pageIndex, pageSize, count, models);

        }

        public async Task<Models.CredenciadoraDePara> ObterPeloIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("A CredenciadoraDePara não existe");
            }

            var model = _mapper.Map<Models.CredenciadoraDePara>(entity);
            return model;


        }

        public async Task<Models.CredenciadoraDePara> CriarAsync(Models.CredenciadoraDePara model, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.CredenciadoraDePara>(model);

            #region validation
            if (await _repository.JaExisteAsync(entity.Id, entity.Credenciadora, entity.De, entity.Para, cancellationToken))
            {
                throw new DomainException("Já exite uma CredenciadoraDePara com essa característica");
            }
            #endregion
            entity.Id = Guid.NewGuid();
            _repository.Add(entity);

            //para dapper não precisa desta linha
            await _repository.SaveChangesAsync();
            model.Id = entity.Id;
            return model;


        }
        public async Task AtualizarAsync(Models.CredenciadoraDePara model, CancellationToken cancellationToken)
        {
            if (model.Id == null)
            {
                throw new NotFoundException("A CredenciadoraDePara não existe");
            }
            var entity = await _repository.GetByIdAsync(model.Id.Value, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException("A CredenciadoraDePara não existe");
            }

            _mapper.Map(model, entity);


            #region validation
            if (await _repository.JaExisteAsync(entity.Id, entity.Credenciadora, entity.De, entity.Para, cancellationToken))
            {
                throw new DomainException("Já exite uma CredenciadoraDePara com essa característica");
            }
            #endregion


            //para EF não precisa desta linha
            _repository.Update(entity);

            //para dapper não precisa desta linha
            await _repository.SaveChangesAsync();

        }



        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(id, cancellationToken);
            #region validation
            if (item == null)
            {
                throw new NotFoundException("O item não foi localizado pelo ID");
            }
            #endregion



            _repository.Remove(item);

            //para dapper não precisa desta linha
            await _repository.SaveChangesAsync(cancellationToken);
        }




    }
}
