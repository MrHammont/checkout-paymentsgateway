using System.Collections.Generic;
using AutoMapper;

namespace Checkout.PaymentsGateway.Infrastructure.UnitTests.Repositories
{
    public class MapperUtils
    {
        public static IMapper GetMapper(IEnumerable<Profile> profiles)
        {
            var configuration = new MapperConfiguration(cfg => AddProfiles(cfg, profiles));
            var mapper = new Mapper(configuration);

            return mapper;
        }

        private static void AddProfiles(IMapperConfigurationExpression cfg, IEnumerable<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                cfg.AddProfile(profile);
            }
        }
    }
}