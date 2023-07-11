using AutoMapper;
using SmartWallet.ApplicationService.Dto.Request;
using SmartWallet.ApplicationService.Dto.Response;
using SmartWallet.DomainModel.Dto.Request;
using SmartWallet.DomainModel.Entities.Response;
using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.ApplicationService.Extension
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BalanceHistoricRequestDto, BalanceHistoricRequestEntity>();
            CreateMap<BalanceHistoricRequestEntity, BalanceHistoric>();            
            CreateMap<BalanceHistoric, BalanceHistoricResponseEntity>();
            CreateMap<BalanceHistoricResponseEntity, BalanceHistoricResponseDto>();

            CreateMap<CoinRequestDto, CoinRequestEntity>();
            CreateMap<CoinRequestEntity, Coin>();            
            CreateMap<Coin, CoinResponseEntity>();
            CreateMap<CoinResponseEntity, CoinResponseDto>();

            CreateMap<CustomerRequestDto, CustomerRequestEntity>();
            CreateMap<CustomerRequestEntity, Customer>();
            CreateMap<Customer, CustomerResponseEntity>();
            CreateMap<CustomerResponseEntity, CustomerResponseDto>();

            CreateMap<WalletRequestDto, WalletRequestEntity>();
            CreateMap<WalletRequestEntity, Wallet>();
            CreateMap<Wallet, WalletResponseEntity>();
            CreateMap<WalletResponseEntity, WalletResponseDto>();
        }
    }
}