using Ahedfi.Component.Core.Domain.Models.Entities;
using Ahedfi.Component.Data.Domain.Interfaces;
using AutoMapper;
using System.Collections.Generic;

namespace Ahedfi.Component.Data.Infrastructure
{
    public class MapEngine : IMapEngine
    {
        private readonly IMapper _mapper;

        public MapEngine(IMapper mapper)
        {
            _mapper = mapper;
        }
        public TMapTo Map<TMapTo>(object source) where TMapTo : Entity
        {
            return _mapper.Map<TMapTo>(source);
        }

        public IEnumerable<TMapTo> MapList<TMapTo>(object source) where TMapTo : Entity
        {
            return _mapper.Map<IEnumerable<TMapTo>>(source);
        }
    }
}
