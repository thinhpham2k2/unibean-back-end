using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Invitations;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class InvitationService : IInvitationService
{
    private readonly Mapper mapper;

    private readonly IInvitationRepository invitationRepository;

    public InvitationService(IInvitationRepository invitationRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Invitation, InvitationModel>()
            .ForMember(t => t.Inviter, opt => opt.MapFrom(src => src.Inviter.FullName))
            .ForMember(t => t.Invitee, opt => opt.MapFrom(src => src.Invitee.FullName))
            .ReverseMap();
            cfg.CreateMap<Invitation, CreateInvitationModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.invitationRepository = invitationRepository;
    }

    public InvitationModel Add(CreateInvitationModel creation)
    {
        return mapper.Map<InvitationModel>
            (invitationRepository.Add(mapper.Map<Invitation>(creation)));
    }

    public bool ExistInvitation(string invitee)
    {
        return invitationRepository.ExistInvitation(invitee);
    }
}
